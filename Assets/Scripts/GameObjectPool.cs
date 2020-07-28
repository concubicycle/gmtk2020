using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Assets.Scripts
{
    class GameObjectPool
    {
        public GameObjectPool(GameObject prefab)
        {
            _prefab = prefab;
        }

        public GameObject Instantiate(Vector3 position, Vector3 scale, Quaternion rotation)
        {
            GameObject go = _pool.Any()
                ? _pool.Pop()
                : GameObject.Instantiate(_prefab, position, Quaternion.identity);

            var go_t = go.GetComponent<Transform>();
            go_t.localScale = scale;
            go_t.position = position;
            go_t.rotation = rotation;
            go.SetActive(true);
            _tracked.Add(go);
            return go;
        }

        public GameObject Instantiate(Vector3 position, Vector3 scale)
        {
            GameObject go = _pool.Any()
                ? _pool.Pop()
                : GameObject.Instantiate(_prefab, position, Quaternion.identity);

            var go_t = go.GetComponent<Transform>();
            go_t.localScale = scale;
            go_t.position = position;
            go_t.rotation = Quaternion.identity;
            go.SetActive(true);
            _tracked.Add(go);
            return go;
        }

        public void Free(GameObject obj)
        {
            obj.SetActive(false);
            _tracked.Remove(obj);
            _pool.Push(obj);
        }

        public void DestroyAll(GameObject obj)
        {
            foreach(var go in _tracked)
            {
                GameObject.Destroy(go);
            }

            while (_pool.Any())
                GameObject.Destroy(_pool.Pop());
        }

        private GameObject _prefab;
        private Stack<GameObject> _pool = new Stack<GameObject>();
        public List<GameObject> _tracked = new List<GameObject>();
    }
}
