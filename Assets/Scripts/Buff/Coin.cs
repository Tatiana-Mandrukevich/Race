using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MyCar"))
        {
            // Здесь можно добавить начисление очков
            Destroy(gameObject);
        }
    }
}
