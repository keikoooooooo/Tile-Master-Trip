using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Khởi tạo 1 Pool Object với ràng buộc object tạo Pool phải kể thừa MonoBehaviour và IPooled
/// </summary>
/// <typeparam name="T"></typeparam>
public class ObjectPooler<T> where T : MonoBehaviour, IPooled<T>
{
    public readonly Queue<T> Pool;

    private readonly T prefab;
    private readonly Transform parent;

    /// <summary>
    /// Khởi tạo Pool
    /// </summary>
    /// <param name="_prefab"> Phần tử cần khởi tạo </param>
    /// <param name="_parent"> Object chứa các phần tử  </param>
    /// <param name="size"> Số lượng phần tử </param>
    public ObjectPooler(T _prefab, Transform _parent, int size)
    {
        Pool = new Queue<T>(size);
        prefab = _prefab;
        parent = _parent;
        for (var i = 0; i < size; i++)
        {
            Pool.Enqueue(Create());
        }
    }
    private T Create()
    {
        var NewObj = Object.Instantiate(prefab, parent);
        NewObj.ReleaseCallback = FreeTheObject;
        NewObj.gameObject.SetActive(false);
        return NewObj;
    }
    private void FreeTheObject(T _Object)
    {
        _Object.gameObject.SetActive(false);
        Pool.Enqueue(_Object);
    }


    #region Get Object
    /// <summary>
    /// Lấy 1 Object đầu tiên trong Pool
    /// </summary>
    /// <returns></returns>
    public T Get()
    {
        if (Pool.Count == 0)
        {
            var NewObj = Create();
            Pool.Enqueue(NewObj);
        }

        var Obj = Pool.Dequeue();
        Obj.gameObject.SetActive(true);
        return Obj;
    }
    /// <summary>
    /// Lấy 1 Object đầu tiên trong Pool tại vị trí ?(_position)
    /// </summary>
    /// <param name="_position"> Vị trí cần lấy Object </param>
    /// <returns></returns>
    public T Get(Vector3 _position)
    {
        if (Pool.Count == 0)
        {
            var NewObj = Create();
            Pool.Enqueue(NewObj);
        }

        var Obj = Pool.Dequeue();
        Obj.gameObject.transform.position = _position;
        Obj.gameObject.SetActive(true);
        return Obj;
    }
    /// <summary>
    /// Lấy 1 Object đầu tiên trong Pool tại vị trí ?(_position) và theo góc quay ?(_rotation)
    /// </summary>
    /// <param name="_position"> Vị trí cần lấy Object </param>
    /// <param name="_quaternion"> Góc quay của Object </param>
    /// <returns></returns>
    public T Get(Vector3 _position, Quaternion _quaternion)
    {
        if (Pool.Count == 0)
        {
            var NewObj = Create();
            Pool.Enqueue(NewObj);
        }

        var Obj = Pool.Dequeue();
        Obj.gameObject.transform.position = _position;
        Obj.gameObject.transform.rotation = _quaternion;
        Obj.gameObject.SetActive(true);
        return Obj;
    }
    #endregion
    
}
