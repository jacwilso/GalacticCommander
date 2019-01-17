using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{

    [SerializeField]
    int initialSize = 5, maxSize = 10;
    [SerializeField]
    T prefab;

    Queue<T> pool;
    int size;

    void Start()
    {
        pool = new Queue<T>();
        size = initialSize;
        for (int i = 0; i < initialSize; i++)
        {
            T obj = Instantiate<T>(prefab, transform);
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    void OnValidate()
    {
        if (initialSize < 0 || initialSize > maxSize)
            Debug.LogError("Initial size can't be less than 0 or greater than max.");
    }

    public T Get()
    {
        T obj;

        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }

        IncreasePool();
        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
            return null;
    }

    public void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }

    public void IncreasePool()
    {
        if (size >= maxSize)
            return;

        int stepSize = size + 5;
        for (int i = size; i < maxSize && i < stepSize; i++)
        {
            size++;
            T obj = Instantiate<T>(prefab, transform);
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }
}
