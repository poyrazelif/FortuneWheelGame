using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolObjects
{
    public string name;
    public GameObject objectToPool;
    public int poolCount;
    
    public List<GameObject> pooledObjects = new List<GameObject>();
}
public class ObjectPool : Singleton<ObjectPool>
{
        public PoolObjects[] objects;
        public delegate void OnGetFromPool();

        void Awake()
        {
            InitializePool();
        }

        public GameObject GetFromPool(string name)
        {
            GameObject _objectToReturn = null;
            foreach (PoolObjects pool in objects)
            {
                if (pool.name == name)
                {
                    foreach (var _pooledObject in pool.pooledObjects)
                    {
                        if (_pooledObject.activeInHierarchy)
                            continue;

                        _objectToReturn = _pooledObject;
                        break;
                    }
                }
            }
            return _objectToReturn;
        }

        public void Deposit(GameObject gameObject)
        {
            foreach (PoolObjects pool in objects)
            {
                if (pool.pooledObjects.Contains(gameObject))
                {
                    gameObject.transform.SetParent(transform);
                    gameObject.SetActive(false);
                    return;
                }
            }
        }

        public void DepositAll()
        {
            foreach (PoolObjects pool in objects)
            {
                foreach (var poolObject in pool.pooledObjects)
                {
                    poolObject.transform.SetParent(transform);
                    poolObject.SetActive(false);
                }
            }
        }
        private void InitializePool()
        {
            foreach (PoolObjects pool in objects)
            {
                for (int i = 0; i < pool.poolCount; i++)
                {
                    GameObject _pooled = Instantiate(pool.objectToPool, transform);
                    _pooled.name = pool.name;
                    _pooled.SetActive(false);
                    pool.pooledObjects.Add(_pooled);
                }
            }
        }
}
