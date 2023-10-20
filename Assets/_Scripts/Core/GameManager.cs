using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Camera _camera;
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

    #if UNITY_ANDROID
    private void HandleInput()
    {
        if (Input.touchCount <= 0) 
            return;
        
        _touch = Input.GetTouch(0);
        _ray = _camera.ScreenPointToRay(_touch.position);
        
        switch (_touch.phase)
        {
            case TouchPhase.Began or TouchPhase.Moved:
                if (Physics.Raycast(_ray, out _hit) && TileData.Contains(_hit.collider, out var tile))
                {
                    if (_tile != null && _tile != tile) 
                        _tile.outlinable.enabled = false;
                    _tile = tile;
                    _tile.outlinable.enabled = true;
                }
                break;
            case TouchPhase.Ended when _tile != null:
                _tile.outlinable.enabled = false;
                _tile.gameObject.SetActive(false);
                _tile = null;
                break;
        }
    }
    #else
    private void HandleInput()
    {
        if (_tile != null && !Input.GetMouseButton(0))
        {
            _tile.outlinable.enabled = false;
            _tile = null;
        }
        _ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(_ray, out _hit) && TileData.Contains(_hit.collider, out var tile))
        {
            _tile = tile;
            _tile.outlinable.enabled = true;

            if (Input.GetMouseButtonDown(0))
            {
                // TODO ???
                _tile.gameObject.SetActive(false);
                _tile = null;
            }
        }
    }
    #endif
    

    
  
}
 