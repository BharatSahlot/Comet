using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Pool<T> where T : MonoBehaviour
{
    private readonly T _obj;
    private readonly Queue<T> _queue;

    private List<T> _active = new List<T>();

    public IReadOnlyList<T> Active => _active;

    public Pool(T obj, int hint = 0)
    {
        _obj = obj;
        _queue = new Queue<T>();
        for (int i = 0; i < hint; i++)
        {
            var go = Object.Instantiate(obj);
            go.gameObject.SetActive(false);
            _queue.Enqueue(go);
        }
    }

    public T Borrow(bool setActive = true)
    {
        if (_queue.Count > 0)
        {
            var go = _queue.Dequeue();
            go.gameObject.SetActive(setActive);
            _active.Add(go);
            return go;
        }
        var obj = Object.Instantiate(_obj);
        _active.Add(obj);
        return obj;
    }

    public void Return(T obj)
    {
        _queue.Enqueue(obj);
        obj.gameObject.SetActive(false);
        _active.Remove(obj);
    }
}