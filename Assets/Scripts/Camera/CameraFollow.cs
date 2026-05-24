using UnityEngine;
/// <summary>
/// Скрипт для следования камеры за машиной.
/// Гарантирует, что камера всегда находится на определенной высоте и расстоянии.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    public Transform carTransform;

    public Vector3 offset = new Vector3(0, 5, -10); // Смещение относительно машины
    public float smoothSpeed = 10f; // Плавность следования
    
    public bool lockY = true; // Фиксировать высоту камеры
    public float fixedY = 5f; // Высота, если lockY = true

    private void Start()
    {
        if (carTransform == null)
        {
            // Поиск машины по тегу
            GameObject carObj = GameObject.FindGameObjectWithTag("MyCar");
            carTransform = carObj.transform;
        }

        if (carTransform != null && lockY)
        {
            // Текущая высота
            if (fixedY == 0)
            {
                fixedY = transform.position.y;
            }
        }
    }

    private void LateUpdate()
    {
        // Целевая позиция - следование за машиной по X и Z
        Vector3 desiredPosition = carTransform.position + offset;
        
        // Если заблокирована высота, игнорируем Y машины
        if (lockY)
        {
            desiredPosition.y = fixedY;
        }

        // Плавное перемещение к целевой точке
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}
