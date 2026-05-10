using System.Collections.Generic;
using UnityEngine;

public class ChunkRecycler : IChunkRecycler
{
    public void RecycleChunks(List<Transform> activeChunks, float recycleThreshold, float blockLength, IChunkSpawner spawner)
    {
        while (activeChunks.Count > 0)
        {
            Transform oldestBlock = activeChunks[0];
            if (oldestBlock.position.z >= recycleThreshold)
            {
                return;
            }
            Transform recycleChunk = activeChunks[0];
            activeChunks.Remove(recycleChunk);
            float frontBlockZPosition = activeChunks.Count == 0 ? recycleChunk.position.z : GetFrontPositionZ(activeChunks);
            float nextBlockZPosition = frontBlockZPosition + blockLength;
            // Вместо Destroy вернуть в пул
            recycleChunk.GetComponent<IPooledObject>().ReturnToPool();
            Transform newBlock = spawner.SpawnChunk(nextBlockZPosition);
            activeChunks.Add(newBlock);
        }
    }

    private float GetFrontPositionZ(List<Transform> activeChunks)
    {
        float returnValue = float.MinValue;
        foreach (var activeChunk in activeChunks)
        {
            if (activeChunk.position.z > returnValue)
            {
                returnValue = activeChunk.position.z;
            }
        }
        return returnValue;
    }
}
