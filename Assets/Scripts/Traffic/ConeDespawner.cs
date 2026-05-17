using UnityEngine;

public class ConeDespawner : MonoBehaviour
{
    public float despawnZoneDistance = 15f;
    public float zoneHeight = 5f;
    public float zoneWidth = 10f;

    private Collider despawnZone;

    private void OnEnable()
    {
        despawnZone = GetComponent<Collider>();
        if (despawnZone == null)
        {
            CreateDespawnZone();
        }
    }

    private void CreateDespawnZone()
    {
        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        boxCollider.size = new Vector3(zoneWidth, zoneHeight, despawnZoneDistance);
        boxCollider.center = new Vector3(0, 0, -despawnZoneDistance / 2f);
        despawnZone = boxCollider;
    }

    private void OnTriggerEnter(Collider other)
    {
        TrafficCone cone = other.GetComponent<TrafficCone>();
        if (cone != null)
        {
            // Конус вошел в зону исчезновения
            cone.ReturnToPool();
        }
    }
}


