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

    private void Start()
    {
        SpawnForestSide(-1); // Левая сторона
        SpawnForestSide(1);  // Правая сторона
    }

    private void SpawnForestSide(int sideSign)
    {
        for (int i = 0; i < treesPerSide; i++)
        {
            // Считаем позицию в чистых метрах, полностью игнорируя масштаб чанка
            float localX = (distanceFromCenter + Random.Range(0f, forestWidth)) * sideSign;
            float localZ = Random.Range(-chunkLength / 2f, chunkLength / 2f);
            
            // Вычисляем мировую позицию на основе направления движения чанка, а не его масштабированных осей
            Vector3 worldSpawnPos = transform.position + (transform.right.normalized * localX) + (transform.forward.normalized * localZ);

            // Выравнивание по высоте земли
            if (Physics.Raycast(worldSpawnPos + Vector3.up * 20f, Vector3.down, out RaycastHit hit, 40f))
            {
                worldSpawnPos.y = hit.point.y;
            }

            GameObject randomTreePrefab = treePrefabs[Random.Range(0, treePrefabs.Length)];
            Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            // Спавним дерево в корень сцены (БЕЗ РОДИТЕЛЯ).
            GameObject newTree = Instantiate(randomTreePrefab, worldSpawnPos, randomRotation);

            // Задаем случайный размер
            float randomScale = Random.Range(minScale, maxScale);
            newTree.transform.localScale = Vector3.one * randomScale;

            // Добавляем компонент следования, чтобы дерево двигалось и удалялось вместе с чанком
            var follower = newTree.AddComponent<SimpleFollower>();
            follower.Setup(transform); 
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
        Gizmos.DrawWireCube(rightCenter, new Vector3(forestWidth, 1f, chunkLength));
        
        Gizmos.matrix = oldMatrix;
    }
}