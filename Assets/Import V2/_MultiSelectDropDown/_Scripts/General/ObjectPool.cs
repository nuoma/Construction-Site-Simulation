using System;
using System.Linq;
using UnityEngine;


// If you're using Odin, you can serialize this in the inspector and won't have to initialize this at runtime.
// Or you could create your own editor script for that ;) 
[System.Serializable]
public class ObjectPool<T> where T : Component
{
    private int m_ActualSize;
    [SerializeField] [HideInInspector] protected T[] m_Pool = new T[0], m_Rented = new T[0];
    [SerializeField] public int poolSize;

    [SerializeField] public GameObject prefab;

    public ObjectPool(int poolSize, GameObject prefab)
    {
        this.poolSize = poolSize;
        this.prefab = prefab;
        Setup();
    }

    public T[] rentedObjects => m_Rented.Length > 0 ? m_Rented.Where(x => x != null).ToArray() : new T[0];
    public T[] allObjects => m_Pool.Length > 0 ? m_Pool.Where(x => x != null).ToArray() : new T[0];

    private int m_ReturnedIndex = 0;

    private bool m_Setup = false;

    public bool initialized
    {
        get
        {
            if (!m_Setup)
                Setup();
            return m_Setup;
        }
    }

    public void Setup()
    {
        m_Setup = true;
        m_Pool = new T[poolSize];
        m_Rented = new T[poolSize];
    }

    public T Rent()
    {
        if (prefab == null)
        {
            Debug.LogError("Prefab is null for type:  " + typeof(T).ToString());
            return null;
        }

        if (!m_Setup)
            Setup();
        if (m_ActualSize > 0)
        {
            for (var i = 0; i < poolSize; i++)
                if (m_Pool[i] != null)
                {
                    var item = m_Pool[i];
                    m_Pool[i] = null;
                    m_Rented[i] = item;

                    item.gameObject.SetActive(true);
                    return item;
                }

            if (m_ActualSize >= poolSize)
            {
//                Debug.LogError("No vacancy in pool: " + gameObject.name);
                return ReturnFirst();
            }

            return Create();
        }

        return Create();
    }

    public T Rent(Transform parent)
    {
        if (!m_Setup)
            Setup();
        T item = null;
        if (m_ActualSize > 0)
        {
            for (var i = 0; i < poolSize; i++)
                if (m_Pool[i] != null)
                {
                    item = m_Pool[i];
                    m_Pool[i] = null;
                    m_Rented[i] = item;

                    item.gameObject.SetActive(true);
                    item.transform.SetParent(parent);
                    item.transform.localScale = Vector3.one;
                    return item;
                }

            if (m_ActualSize >= poolSize)
            {
//                Debug.LogError("No vacancy in pool: " + gameObject.name);
                item = ReturnFirst();
                item.transform.SetParent(parent);
                item.transform.localScale = Vector3.one;
                return item;
            }

            return Create(parent);
        }

        return Create(parent);
    }

    public T Create()
    {
        var newGo = MonoBehaviour.Instantiate(prefab);
        T newItem;
        if (newGo.GetComponent<T>())
            newItem = newGo.GetComponent<T>();
        else
            newItem = newGo.AddComponent<T>();
        m_Rented[m_ActualSize] = newItem;
        m_ActualSize++;
        newItem.gameObject.SetActive(true);
        return newItem;
    }

    public T Create(Transform parent)
    {
        var newGo = MonoBehaviour.Instantiate(prefab);
        T newItem;
        if (newGo.GetComponent<T>())
            newItem = newGo.GetComponent<T>();
        else
            newItem = newGo.AddComponent<T>();
        m_Rented[m_ActualSize] = newItem;
        m_ActualSize++;
        newItem.gameObject.SetActive(true);
        newItem.transform.SetParent(parent);
        newItem.transform.localScale = Vector3.one;
        return newItem;
    }

    public void Return(T item)
    {
        if (!m_Rented.Contains(item) || item == null) return;
        var id = Array.FindIndex(m_Rented, row => row == item);
        m_Rented[id] = null;
        m_Pool[id] = item;
//        Debug.Log(item.gameObject.name+" is supposed to be returned");
        item.gameObject.SetActive(false);
    }

    public void Return(T item, Transform parent)
    {
        if (!m_Rented.Contains(item) || item == null) return;
        var id = Array.FindIndex(m_Rented, row => row == item);
        m_Rented[id] = null;
        m_Pool[id] = item;
//        Debug.Log(item.gameObject.name+" is supposed to be returned");
        item.transform.SetParent(parent);
        item.gameObject.SetActive(false);
    }

    public void Reset()
    {
        if (!m_Setup)
            Setup();
        foreach (var obj in m_Rented.Where(x => x != null))
        {
            if (obj == null) continue;
            Return(obj);
        }
    }

    public void Reset(Transform newParent)
    {
        if (!m_Setup)
            Setup();
        foreach (var obj in m_Rented.Where(x => x != null))
        {
            if (obj == null) continue;
            obj.transform.SetParent(newParent);
            Return(obj);
        }
    }

    public T ReturnFirst()
    {
        Return(m_Rented[m_ReturnedIndex]);
        m_ReturnedIndex++;
        if (m_ReturnedIndex >= poolSize)
            m_ReturnedIndex = 0;
        return Rent();
    }
}