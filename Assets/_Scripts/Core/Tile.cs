using System;
using UnityEngine;
using EPOOutline;
using Random = UnityEngine.Random;

public class Tile : MonoBehaviour, IPooled<Tile>
{
    [Header("Reference")]
    public Rigidbody rb;
    public LookAtCamera lookAtCamera;
    public SpriteRenderer spriteRenderer;
    public Outlinable outlinable;
    public new Audio audio;
    
    [Space]
    [Tooltip("Giới hạn Tile hiển thị trong View Camera: Left & Right")]
    public Vector2 clampX;
    [Tooltip("Giới hạn Tile hiển thị trong View Camera: Top & Bottom")]
    public Vector2 clampY;
    
    
    public Camera _mainCamera { get; set; }
    public TileType tileType { get; private set; }
    
    private bool _isEnter;
    private readonly float _force = 15f;
    
    
    public void SetTile(TileType _tileType)
    {
        tileType = _tileType;
        spriteRenderer.sprite = tileType.sprite;
    }
    
    public void OnEnterTile()
    {
        if(_isEnter) return;
        _isEnter = true;
        
        audio.Play();
        rb.AddForce(RandomVector() * _force, ForceMode.Impulse);
    }
    public void OnExitTile()
    {
        _isEnter = false;
    }
    
    
    private void FixedUpdate()
    {
        var viewportPosition = _mainCamera.WorldToViewportPoint(transform.position);
        viewportPosition.x = Mathf.Clamp(viewportPosition.x, clampX.x, clampX.y);
        viewportPosition.y = Mathf.Clamp(viewportPosition.y, clampY.x, clampY.y);
        transform.position = _mainCamera.ViewportToWorldPoint(viewportPosition);
    }
    private void OnCollisionEnter(Collision other)
    {
        var camPosition = _mainCamera.transform.position;
        camPosition.y = transform.position.y;
        lookAtCamera.StartRotate(transform.rotation, Quaternion.LookRotation(camPosition - transform.position));
    }
    
    
    private static Vector3 RandomVector() =>  new(Random.Range(-.1f, .1f), Random.Range(0f, .2f), Random.Range(-.1f, .1f));
    
    

    // // Release to Pool
    public void Release()
    {
        OnExitTile();
        outlinable.enabled = false;
        ReleaseCallback?.Invoke(this);
    }
    public Action<Tile> ReleaseCallback { get; set; }
}



