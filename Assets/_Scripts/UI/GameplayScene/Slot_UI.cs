using System;
using UnityEngine;

public class Slot_UI : MonoBehaviour, IPooled<Slot_UI>
{
    public RectTransform RectTransform;
    public Tile_UI TileUI;
    
    public void SetSlot(Tile_UI _tileUI)
    {
        if (_tileUI == null && TileUI != null)
        {
            TileUI.Release();
            TileUI = null;
        }
        else
        {
            TileUI = _tileUI;
        }
    }
    
    
    public void Release() => ReleaseCallback?.Invoke(this);
    public Action<Slot_UI> ReleaseCallback { get; set; }
}
