using UnityEngine;

public static class Extensions
{

    /// <summary>
    /// Trả về True nếu gameObject có cùng LayerMask
    /// </summary>
    /// <param name="gameObject"> Object kiểm tra Layer </param>
    /// <returns></returns>
    public static bool Contains(this LayerMask layers, GameObject gameObject)
        => 0 != (layers.value & 1 << gameObject.layer);



}
