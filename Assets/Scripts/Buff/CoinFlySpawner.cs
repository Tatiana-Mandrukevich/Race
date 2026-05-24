using System.Linq;
using DG.Tweening;
using UnityEngine;

public class CoinFlySpawner : MonoBehaviour
{
    public ChunkManager ChunkManager;
    public Transform[] spawnCoinPosition;
    public GameObject CoinPrefab;
    public float SpawnInterval = 0.5f;

    private bool canSpawn = true;

    private int spawnLineId;
    private int cointInLine;

    public void SpawnCoin(float yPosition)
    {
        if (canSpawn == false || CoinPrefab == null) return;

        if (spawnCoinPosition == null || spawnCoinPosition.Length == 0)
        {
            // Попробуем найти дочерние объекты как позиции для спавна, если массив пуст
            spawnCoinPosition = GetComponentsInChildren<Transform>().Where(t => t != transform).ToArray();
            if (spawnCoinPosition.Length == 0) return;
        }

        if (ChunkManager == null)
        {
            ChunkManager = FindObjectOfType<ChunkManager>();
            if (ChunkManager == null) return;
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
        
        // Добавляем скрипт Coin на объект с коллайдером
        Collider coinCollider = newCoin.GetComponentInChildren<Collider>();
        if (coinCollider != null)
        {
            if (coinCollider.gameObject.GetComponent<Coin>() == null)
            {
                coinCollider.gameObject.AddComponent<Coin>();
            }
        }
        
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