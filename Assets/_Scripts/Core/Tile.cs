using System;
using UnityEngine;
using EPOOutline;

public class Tile : MonoBehaviour, IPooled<Tile>
{
    [Header("Reference")]
    public Rigidbody rb;
    public LookAtCamera lookAtCamera;
    public SpriteRenderer spriteRenderer;
    public Outlinable outlinable;

    [Space]
    [Tooltip("Giới hạn Tile hiển thị trong View Camera: Cạnh trái và Cạnh phải")]
    public Vector2 clampX;
    [Tooltip("Giới hạn Tile hiển thị trong View Camera: Cạnh dưới và Cạnh trên")]
    public Vector2 clampY;
    
    public Camera _mainCamera { get; set; }
    public TileTypeEnums typeEnums { get; private set; }
    
    
    public void SetTile(TileType _tileType)
    {
        spriteRenderer.sprite = _tileType.sprite;
        typeEnums = _tileType.typeEnums;
    }
    
    
    private void FixedUpdate()
    {
        if (_mainCamera == null)
            return;
        
        var viewportPosition = _mainCamera.WorldToViewportPoint(transform.position);
        viewportPosition.x = Mathf.Clamp(viewportPosition.x, clampX.x, clampX.y);
        viewportPosition.y = Mathf.Clamp(viewportPosition.y, clampY.x, clampY.y);
        transform.position = _mainCamera.ViewportToWorldPoint(viewportPosition);
    }
    private void OnCollisionEnter(Collision other)
    {
        if(_mainCamera == null) 
            return;
        
        var camPosition = _mainCamera.transform.position;
        camPosition.y = transform.position.y;
        lookAtCamera.StartRotate(transform.rotation, Quaternion.LookRotation(camPosition - transform.position));
    }



    // // Release to Pool
    public void Release()
    {
        outlinable.enabled = false;
        ReleaseCallback?.Invoke(this);
    }
    public Action<Tile> ReleaseCallback { get; set; }
}



