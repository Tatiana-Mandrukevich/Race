using UnityEngine;

// Вспомогательный класс для связи дерева и чанка в обход иерархии Unity
public class SimpleFollower : MonoBehaviour
{
    private Transform _targetChunk;
    private Vector3 _localOffset;

    public void Setup(Transform chunkTransform)
    {
        _targetChunk = chunkTransform;
        
        // Запоминаем позицию дерева относительно чанка БЕЗ влияния его масштаба
        Vector3 worldOffset = transform.position - _targetChunk.position;
        _localOffset = new Vector3(
            Vector3.Dot(worldOffset, _targetChunk.right.normalized),
            Vector3.Dot(worldOffset, _targetChunk.up.normalized),
            Vector3.Dot(worldOffset, _targetChunk.forward.normalized)
        );
    }

    private void Update()
    {
        // Если ChunkManager удалил чанк дороги, дерево мгновенно удаляет само себя
        if (_targetChunk == null)
        {
            Destroy(gameObject);
            return;
        }

        // Перемещаем дерево вслед за чанком, сохраняя его идеальную 3D-форму
        transform.position = _targetChunk.position + 
                             (_targetChunk.right.normalized * _localOffset.x) + 
                             (_targetChunk.up.normalized * _localOffset.y) + 
                             (_targetChunk.forward.normalized * _localOffset.z);
    }
}