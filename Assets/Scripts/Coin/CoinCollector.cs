using UnityEngine;
using Zenject;

public class CoinCollector : MonoBehaviour
{
    [Inject] private CoinController _coinController;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            var coin = other.transform.parent.GetComponent<Coin>();
            coin.DoSmall(() => Destroy(other.gameObject));

            _coinController.AddCoin();
        }
    }
}