using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class Chunk : MonoPooled
    {
        public List<GameObject> spawnObjects;
        public List<Transform> spawnPositions;

        private List<GameObject> spawnedObjects = new List<GameObject>();
        private TrafficCone[] _allCones;

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
        
        public void ChunkSpawned(int amountObjects)
        {
            for (int i = 0; i < amountObjects; i++)
            {
                GameObject randomObject = spawnObjects[Random.Range(0, spawnObjects.Count)];
                Transform spawnPosition = spawnPositions[Random.Range(0, spawnPositions.Count)];
                GameObject newObject = Instantiate(randomObject, spawnPosition);
                newObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                spawnedObjects.Add(newObject);
            }
        }

        public override void ReturnToPool()
        {
            base.ReturnToPool();
            foreach (var spawnedObject in spawnedObjects)
            {
                Destroy(spawnedObject.gameObject);
            }
            spawnedObjects.Clear();
        }
    }
}