using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.Pool
{
    public static class TreePool
    {
        private static readonly Queue<GameObject> PoolQueue = new Queue<GameObject>();
        private static GameObject[] _prefabs;
        private static Transform _poolRoot;

        public static void Initialize(GameObject[] prefabs)
        {
            _prefabs = prefabs;
            CheckPoolRoot();
        }

        private static void CheckPoolRoot()
        {
            if (_poolRoot == null)
            {
                _poolRoot = new GameObject("[Global_Tree_Pool]").transform;
                Object.DontDestroyOnLoad(_poolRoot.gameObject);
            }
        }

        public static GameObject GetTree(Vector3 position, Quaternion rotation)
        {
            if (_prefabs == null || _prefabs.Length == 0) return null;

            CheckPoolRoot();

            // Цикл работает, пока в очереди есть хоть какие-то объекты
            while (PoolQueue.Count > 0)
            {
                GameObject tree = PoolQueue.Dequeue();
                
                if (tree == null) continue;
                
                // Если оно activeInHierarchy == true, значит это дубликат, который уже стоит на трассе. Пропускаем его!
                if (tree.activeInHierarchy == false)
                {
                    tree.transform.position = position;
                    tree.transform.rotation = rotation;
                    tree.SetActive(true);
                    return tree; // Успешно переиспользовали старое дерево!
                }
            }
            
            GameObject prefab = _prefabs[Random.Range(0, _prefabs.Length)];
            GameObject newTree = Object.Instantiate(prefab, position, rotation);
            return newTree;
        }

        public static void ReturnTree(GameObject tree)
        {
            if (tree == null) return;

            CheckPoolRoot();
            
            tree.SetActive(false);
            tree.transform.SetParent(_poolRoot, false);
            
            if (!PoolQueue.Contains(tree))
            {
                PoolQueue.Enqueue(tree);
            }
        }
    }
}