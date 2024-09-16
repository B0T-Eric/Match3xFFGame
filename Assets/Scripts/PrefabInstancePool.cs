using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PrefabInstancePool<T> where T : MonoBehaviour
{
    Stack<T> pool;

    public T GetInstance (T prefab)
    {
        if(pool == null)
        {
            pool = new();
        }
        else if(pool.TryPeek(out T i) && !i)
        {
            pool.Clear();
        }
        if(pool.TryPop (out T instance))
        {
            instance.gameObject.SetActive (true);
        }
        else
        {
            instance = Object.Instantiate (prefab);
        }
        return instance;
    }
    public void Recycle(T instance)
    {
        if(pool == null)
        {
            Object.Destroy (instance.gameObject);
            return;
        }
        pool.Push (instance);
        instance.gameObject.SetActive (false);
    }
}
