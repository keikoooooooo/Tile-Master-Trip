using System;

public interface IPooled<out T>
{
    /// <summary>
    /// Giải phóng về lại Pool
    /// </summary>
    public void Release();
    Action<T> ReleaseCallback { set; }
}
