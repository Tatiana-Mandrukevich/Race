using System.Collections.Generic;
using DefaultNamespace.Pool;
using UnityEngine;

public class ChunkForestGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] treePrefabs; 
    
    private int treesPerSide = 45;     
    private float chunkLength = 10f;   
    private float distanceFromCenter = 15f; 
    private float forestWidth = 50f;
    private float minScale = 0.7f;
    private float maxScale = 1.4f;

    // Список созданных деревьев для этого чанка
    private readonly List<GameObject> _myTrees = new();
    private Transform _forestRoot;
    
    public void GenerateForest()
    {
        TreePool.Initialize(treePrefabs);
        
        CleanUpForest();

        GameObject rootObj = new GameObject("Forest_Root");
        _forestRoot = rootObj.transform;
        
        // Теперь позиция и поворот будут идеальными, так как чанк уже на месте!
        _forestRoot.position = transform.position;
        _forestRoot.rotation = transform.rotation;
        _forestRoot.localScale = Vector3.one; 
        _forestRoot.SetParent(transform, true); 

        SpawnForestSide(-1); 
        SpawnForestSide(1);  
    }

    private void SpawnForestSide(int sideSign)
    {
        for (int i = 0; i < treesPerSide; i++)
        {
            float localX = (distanceFromCenter + Random.Range(0f, forestWidth)) * sideSign;
            float localZ = Random.Range(-chunkLength / 2f, chunkLength / 2f);
            
            Vector3 worldSpawnPos = transform.position + (transform.right.normalized * localX) + (transform.forward.normalized * localZ);

            if (Physics.Raycast(worldSpawnPos + Vector3.up * 20f, Vector3.down, out RaycastHit hit, 40f))
            {
                worldSpawnPos.y = hit.point.y;
            }

            Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            GameObject newTree = TreePool.GetTree(worldSpawnPos, randomRotation);
            if (newTree == null) continue;

            newTree.transform.SetParent(_forestRoot, true);

            float randomScale = Random.Range(minScale, maxScale);
            newTree.transform.localScale = Vector3.one * randomScale;

            _myTrees.Add(newTree);
        }
    }
    
    public void CleanUpForest()
    {
        foreach (var tree in _myTrees)
        {
            if (tree != null)
            {
                TreePool.ReturnTree(tree);      
            }
        }
        _myTrees.Clear();

        if (_forestRoot != null)
        {
            Destroy(_forestRoot.gameObject);
            _forestRoot = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.4f);
        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        
        float halfWidth = forestWidth / 2f;
        Vector3 leftCenter = new Vector3(-distanceFromCenter - halfWidth, 0f, 0f);
        Gizmos.DrawWireCube(leftCenter, new Vector3(forestWidth, 1f, chunkLength));

        Vector3 rightCenter = new Vector3(distanceFromCenter + halfWidth, 0f, 0f);
        Gizmos.DrawWireCube(rightCenter, new Vector3(distanceFromCenter + halfWidth, 0f, 0f));
        
        Gizmos.matrix = oldMatrix;
    }
}