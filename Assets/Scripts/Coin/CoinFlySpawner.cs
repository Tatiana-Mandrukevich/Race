using System.Linq;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class CoinFlySpawner : MonoBehaviour
{
    public ChunkManager ChunkManager;
    public Transform[] spawnCoinPosition;
    public GameObject CoinPrefab;
    public float SpawnInterval = 0.5f;
    
    [Inject] private CoinController _coinController;

    private bool canSpawn = true;

    private int spawnLineId;
    private int cointInLine;

    public void SpawnCoin(float yPosition)
    {
        if (canSpawn == false) return;

        if (spawnCoinPosition == null || spawnCoinPosition.Length == 0)
        {
            // Попробуем найти дочерние объекты как позиции для спавна, если массив пуст
            spawnCoinPosition = GetComponentsInChildren<Transform>().Where(t => t != transform).ToArray();
        }

        if (cointInLine == 0)
        {
            spawnLineId = Random.Range(0, spawnCoinPosition.Length);
            cointInLine = Random.Range(20, 30);
        }

        canSpawn = false;
        DOVirtual.DelayedCall(SpawnInterval, () => { canSpawn = true; });

        Vector3 spawnPos = spawnCoinPosition[spawnLineId].position;
        spawnPos.x = Mathf.Clamp(spawnPos.x, -0.7f, 0.7f);
        spawnPos.y = yPosition;
        
        GameObject newCoin = Instantiate(CoinPrefab, spawnPos, Quaternion.identity);
        newCoin.SetActive(true);
        
        ChunkManager.GetLastChunk();
        MonoPooled lastChunk = ChunkManager.GetLastChunk();
        
        if (lastChunk != null)
        {
            newCoin.transform.parent = lastChunk.transform;
        }
        
        Destroy(newCoin.gameObject, 5);
        cointInLine--;
    }
}