using DefaultNamespace.Buff;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private CoinController _coinController;
    
    public void Initialize(CoinController coinController)
    {
        _coinController = coinController;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MyCar"))
        {
            _coinController.AddCoin();
            Destroy(gameObject);
        }
    }
}
