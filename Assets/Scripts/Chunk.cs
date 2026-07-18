using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class Chunk : MonoPooled
    {
        public List<Transform> carsSpawnPositions;
        public List<Transform> coinSpawnPositions;
        
        [SerializeField] private List<GameObject> spawnObjects;
        [SerializeField] private Coin coinPrefab;

        private List<GameObject> spawnedObjects = new();
        private List<Coin> spawnedCoins = new();
        private TrafficCone[] _allCones;
        [Inject] private CoinController _coinController;

        private void Awake()
        {
            // Находим все конусы один раз при создании чанка
            _allCones = GetComponentsInChildren<TrafficCone>(true);
        }

        public override void Initialize()
        {
            base.Initialize();
            
            // Сбрасываем все конусы, используя сохраненный список, 
            // так как сбитые конусы могут быть не в иерархии в этот момент
            if (_allCones != null)
            {
                foreach (var cone in _allCones)
                {
                    if (cone != null) cone.ResetCone();
                }
            }
        }
        
        public void ObjectsOnChunkSpawned(int amountCarsOnChunkSpawned, int amountCoinOnChunkSpawned)
        {
            for (int i = 0; i < amountCarsOnChunkSpawned; i++)
            {
                CarsOnChunkSpawned();
            }
            
            for (int i = 0; i < amountCoinOnChunkSpawned; i++)
            {
                CoinsOnChunkSpawned();
            }
        }

        public override void ReturnToPool()
        {
            base.ReturnToPool();
            
            // Для машины:
            foreach (var spawnedObject in spawnedObjects)
            {
                Destroy(spawnedObject.gameObject);
            }
            spawnedObjects.Clear();
            
            // Для монеты:
            foreach (var spawnedCoin in spawnedCoins)
            {
                Destroy(spawnedCoin.gameObject);
            }
            spawnedCoins.Clear();
        }

        private void CarsOnChunkSpawned()
        {
            GameObject randomObject = spawnObjects[Random.Range(0, spawnObjects.Count)];
            Transform spawnPosition = carsSpawnPositions[Random.Range(0, carsSpawnPositions.Count)];
            GameObject newObject = Instantiate(randomObject, spawnPosition);
            newObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            spawnedObjects.Add(newObject);
        }

        private void CoinsOnChunkSpawned()
        {
            Transform spawnPosition = coinSpawnPositions[Random.Range(0, coinSpawnPositions.Count)];
            Coin newCoin = Instantiate(coinPrefab, spawnPosition);
            newCoin.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            spawnedCoins.Add(newCoin);
            
            var scale = newCoin.transform.localScale;
            var chunkScale=  transform.localScale;
            scale.x/=chunkScale.x;
            scale.y/=chunkScale.y;
            scale.z/=chunkScale.z;
            newCoin.transform.localScale = scale;
        }
    }
}