using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class ChunkSpawner : IChunkSpawner
    {
        private readonly List<GameObject> _chunks;
        private readonly List<GameObject> _lastChunks;
        private readonly Transform _parent;
        private readonly Dictionary<GameObject, Pool<Chunk>> _pools;
        private int amountSpawnedChunks;

        public ChunkSpawner(List<GameObject> chunks, List<GameObject> lastChunks, Transform parent)
        {
            _chunks = chunks;
            _lastChunks = lastChunks;
            _parent = parent;
            _pools = new Dictionary<GameObject, Pool<Chunk>>();

            // Создание pool для каждого prefab
            foreach (var prefab in _chunks)
            {
                var factory = new FactoryMonoObject<Chunk>(prefab.GetComponent<Chunk>(), _parent);
                _pools[prefab] = new Pool<Chunk>(factory);
            }
        }

        public Transform SpawnChunk(float zPosition)
        {
            Vector3 spawnPosition = new Vector3(_parent.position.x, _parent.position.y, zPosition);
            GameObject prefab = GetChunk();
            Chunk newChunk = _pools[prefab].Pull();
            
            amountSpawnedChunks++;
            switch (amountSpawnedChunks)
            {
                case < 10: newChunk.GetComponent<Chunk>().ChunkSpawned(Random.Range(0, 1)); break;
                case > 50: newChunk.GetComponent<Chunk>().ChunkSpawned(1); break;
            }
            
            if (_lastChunks.Count >= 2)
            {
                _lastChunks.Remove(_lastChunks[0]);
            }
            
            _lastChunks.Add(prefab);
            newChunk.transform.position = spawnPosition;
            newChunk.transform.SetParent(_parent);
            return newChunk.transform;
        }

        private GameObject GetChunk()
        {
            if (_lastChunks.Count >= 2)
            {
                var randomChunk = _chunks[Random.Range(0, _chunks.Count)];
                while (_lastChunks.Contains(randomChunk))
                {
                    randomChunk = _chunks[Random.Range(0, _chunks.Count)];
                }
                return randomChunk;
            }
            return _chunks[Random.Range(0, _chunks.Count)];
        }
    }
}