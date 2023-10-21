using System;
using UnityEngine;
using UnityEngine.UI;

public class Tile_UI : MonoBehaviour, IPooled<Tile_UI>
{
 
    public Image icon;
    public TileTypeEnums typeEnums;

    public Tile tile { get; set; }
    
    
    public void SetTileUI(Tile _tile)
    {
        if (_tile == null)
        {
            Release();
        }
        else
        {
            icon.sprite = _tile.spriteRenderer.sprite;
            typeEnums = _tile.typeEnums;
            tile = _tile;
        }
    }
    
    
    
    public void Release() => ReleaseCallback?.Invoke(this);
    public Action<Tile_UI> ReleaseCallback { get; set; }
}
