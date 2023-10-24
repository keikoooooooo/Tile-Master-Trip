using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : Singleton<Spawner>
{
    public MapDataSO mapData;
    [Space, Tooltip("Prefab của Tile")] 
    public Tile tilePrefab;

    [Tooltip("Số lượng tile trong Pool ban đầu"), Range(0, 200)]
    public int sizePool;
    
    private ObjectPooler<Tile> _poolTile;
    
    private Camera _camera;
    private MapSO _mapSO;
    private Coroutine _spawnCoroutine;
    private List<int> _remainingCounts;
    
    private float _randomX, _randomY;
    private readonly int _numberOfBlocks = 3;

    
    private void Start()
    {
        _poolTile = new ObjectPooler<Tile>(tilePrefab, transform, sizePool);
    }
    

    /// <summary>
    /// Giải phóng toàn bộ tile mỗi khi thoát scene gameplay
    /// </summary>
    public void ReleaseAll()
    {
        foreach (Transform child in transform)
        {
            if (!TileData.Contains(child.gameObject, out var _tile) || !child.gameObject.activeSelf)
                continue;
            _tile.Release();
        }
    }
    
    
    /// <summary>
    /// Random 3 Tile ngẫu nhiên có cùng type và trả về List các tile này
    /// </summary>
    /// <param name="_count"> Số lượng cần lấy Tile </param>
    /// <returns></returns>
    public bool GetRandomTiles(int _count, out List<Tile> tiles)
    {
        tiles = new List<Tile>(); // lưu lại các tile được chọn
        
         // tìm 1 Tile ngẫu nhiên
        var _tileType = _mapSO.tileTypes[Random.Range(0, _mapSO.tileTypes.Count)];
        
        List<Tile> tileTempList = new ();     // tạo 1 list tạm thời để lưu lại tất cả tile vừa tìm được 

        foreach (var child in transform.Cast<Transform>().Where(c => c.gameObject.activeSelf))
        {
            if(tileTempList.Count >= 3) break;
            if(!TileData.Contains(child.gameObject, out var _currentTile) || _currentTile.tileType.typeEnums != _tileType.typeEnums)
                continue;
            
            tileTempList.Add(_currentTile);
        }
       
        for (var i = 0; i < _count; i++)
        {
            if (tileTempList.Count <= 0) break;
            
            var randIndex = Random.Range(0, tileTempList.Count);
            tiles.Add(tileTempList[randIndex]);
            tileTempList.RemoveAt(randIndex);
        }
        return tiles.Count >= 3;
    }


    public void Spawn(int level)
    {
        _camera = Camera.main;
        _mapSO = mapData.maps[level - 1];
        if (_mapSO == null) 
            return;

        if(_spawnCoroutine != null) StopCoroutine(_spawnCoroutine);
        _spawnCoroutine = StartCoroutine(SpawnCoroutine());
    }
    private IEnumerator SpawnCoroutine()
    {
        _remainingCounts = new List<int>(_mapSO.tileTypes.Count); // tạo 1 list với (tileTypes.Count) phần tử 
        
        // Tìm tổng số lượng cần spawn
        var totalTiles = 0;
        foreach (var VARIABLE in _mapSO.tileTypes)
        {
            totalTiles += VARIABLE.chance * _numberOfBlocks;
            _remainingCounts.Add(VARIABLE.chance * _numberOfBlocks);
        }
        while (totalTiles > 0)
        {
            var availTileTypes = _mapSO.tileTypes.Where((t, i) => _remainingCounts[i] > 0).ToList();
            if (availTileTypes.Count > 0)
            {
                var randIndex = Random.Range(0, availTileTypes.Count);
                var selectType = availTileTypes[randIndex];
                GetTile(selectType);
                
                _remainingCounts[_mapSO.tileTypes.IndexOf(selectType)]--; // giảm 1 tile trong danh sách cần spawn ra
                totalTiles--;
                yield return new WaitForSeconds(0.02f);
            }
            else
            {
                break;
            }
        }
    }
    
    
    public void GetTile(TileType _tileType)
    {
        var tile = _poolTile.Get(GetRandomPosOffScreen(), GetRandomRotOffScreen());
        tile.SetTile(_tileType);
        tile._mainCamera = _camera;
        TileData.Add(tile.gameObject, tile);
    }

    
    
    /// <summary>
    /// Trả về vị trí ngẫu nhiên trong tấm nhìn của MainCamera
    /// </summary>
    /// <returns></returns>
    private Vector3 GetRandomPosOffScreen()
    {
        _randomX = Random.Range(0.05f, .95f);
        _randomY = Random.Range(0.05f, .95f);
        return _camera.ViewportToWorldPoint(new Vector3(_randomX, _randomY, 13f));
    }
    /// <summary>
    /// Trả về góc quay ngẫu nhiên từ 0-360 của 3 góc X, Y, Z
    /// </summary>
    /// <returns></returns>
    private static quaternion GetRandomRotOffScreen()
    {
        return quaternion.Euler(Random.Range(0, 360f), Random.Range(0, 360f), Random.Range(0, 360f));
    }


    
    public bool IsWin() => transform.Cast<Transform>().All(child => !child.gameObject.activeInHierarchy);


}
