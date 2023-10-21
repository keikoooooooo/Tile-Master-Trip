using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public Camera _camera;
    
    [Space, Tooltip("Khi chọn Tile gọi sự kiện trả về Tile đó")]
    public UnityEvent<Tile> E_SelectTile;
    
    private Ray _ray;
    private Touch _touch;
    private RaycastHit _hit;
    private Tile _tile;
    
    private void Start()
    {
        _camera = Camera.main;
        Application.targetFrameRate = 60;
        //Application.targetFrameRate = -1;
    }
    private void Update()
    {
        HandleInput();
    }
    
    #if !UNITY_ANDROID
    private void HandleInput()
    {
        if (Input.touchCount <= 0)
        {
            if (_tile == null) return;
            
            _tile.outlinable.enabled = false;
            _tile = null;
            return;
        }
        
        _touch = Input.GetTouch(0);
        _ray = _camera.ScreenPointToRay(_touch.position);
        
        switch (_touch.phase)
        {
            case TouchPhase.Began or TouchPhase.Moved:
                if (Physics.Raycast(_ray, out _hit) && TileData.Contains(_hit.collider.gameObject, out var tile))
                {
                    if (_tile != null && _tile != tile) 
                        _tile.outlinable.enabled = false;
                    _tile = tile;
                    _tile.outlinable.enabled = true;
                }
                break;
            
            case TouchPhase.Ended when _tile != null:
                E_SelectedTile?.Invoke(_tile);
                _tile.Release();
                _tile = null;
                break;
        }
    }
    #else
    private void HandleInput()
    {
        switch (_tile != null)
        {
            case true when !Input.GetMouseButton(0):
                _tile.outlinable.enabled = false;
                _tile = null;
                break;
            
            case true when Input.GetMouseButton(0):
                E_SelectTile?.Invoke(_tile);
                _tile.Release();
                _tile = null;
                return;
        }
        
        _ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(_ray, out _hit) || !TileData.Contains(_hit.collider.gameObject, out var tile)) 
            return;
        
        _tile = tile;
        _tile.outlinable.enabled = true;
    }
    #endif
    
    
    
    

    
  
}
 