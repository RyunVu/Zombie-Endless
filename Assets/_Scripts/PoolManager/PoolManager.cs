using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PoolManager : SingletonMonobehaviour<PoolManager>
{
    [SerializeField] private Pool[] poolArray = null;
    private Transform _objectPoolTransform;
    private Dictionary<int, Queue<Component>> _poolDictionary = new();

    [System.Serializable]
    public struct Pool
    {
        public int poolSize;
        public GameObject prefab;
        public string componentType;
    }

    private void Start()
    {
        _objectPoolTransform = this.gameObject.transform;

        for (int i = 0; i < poolArray.Length; i++)
        {
            CreatePool(poolArray[i].prefab, poolArray[i].poolSize, poolArray[i].componentType);
        }
    }

    private void CreatePool(GameObject prefab, int poolSize, string componentType)
    {
        int poolKey = prefab.GetInstanceID();
        string objectName = prefab.name;
        GameObject parentObject = new GameObject(objectName + "Anchor");
        parentObject.transform.SetParent(_objectPoolTransform);

        if (!_poolDictionary.ContainsKey(poolKey))
        {
            _poolDictionary.Add(poolKey, new Queue<Component>());

            for (int i = 0; i < poolSize; i++)
            {
                GameObject newObject = Instantiate(prefab, parentObject.transform) as GameObject;
                newObject.SetActive(false);
                _poolDictionary[poolKey].Enqueue(newObject.GetComponent(Type.GetType(componentType)));
            }
        }
    }

    /// <summary>
    /// Reuse a component from the pool.
    /// </summary>
    /// <param name="prefab">is the prefab gameobject containing the component</param>
    /// <param name="position">is the world position for the gameobject where it should appear when enabled</param>
    /// <param name="rotation">should be set if the gameobject needs to be rotated</param>
    public Component ReuseComponent(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        int poolKey = prefab.GetInstanceID();

        if (_poolDictionary.ContainsKey(poolKey))
        {
            Component componentToReuse = GetComponentFromPool(poolKey);

            ResetObject(position, rotation, componentToReuse, prefab);

            return componentToReuse;
        }
        else
        {
            Debug.LogWarning("Pool with key " + poolKey + " doesn't exist. Please create the pool first.");
            return null;
        }
    }

    private Component GetComponentFromPool(int poolKey)
    {
        Component componentToReuse = _poolDictionary[poolKey].Dequeue();
        _poolDictionary[poolKey].Enqueue(componentToReuse);

        if (componentToReuse.gameObject.activeSelf == true)
        {
            componentToReuse.gameObject.SetActive(false);
        }

        return componentToReuse;
    }

    private void ResetObject(Vector3 position, Quaternion rotation, Component componentToReuse, GameObject prefab)
    {
        componentToReuse.transform.position = position;
        componentToReuse.transform.rotation = rotation;
        componentToReuse.gameObject.transform.localScale = prefab.transform.localScale;
    }
}