using System.Collections.Generic;
using UnityEngine;

public interface IChunkRecycler
{
    void RecycleChunks(List<Transform> activeChunks, float recycleThreshold, float blockLength, IChunkSpawner spawner);
}
