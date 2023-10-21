using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public Camera _camera;
    
    [Space,Tooltip("Prefab của Tile")]
    public Tile tilePrefab;

    public MapSO mapSo; 
    
    private ObjectPooler<Tile> _tilePool;
    
    private float _randomX, _randomY;
    private readonly int numberOfBlocks = 3;
    
    
    private void Start()
    {
        _tilePool = new ObjectPooler<Tile>(tilePrefab, transform, 10);
        
        if(mapSo == null) return;
        StartCoroutine(SpawnCoroutine());
    }
    

    private IEnumerator SpawnCoroutine()
    {
        var typeCurrent = 0;
        foreach (var VARIABLE in mapSo.tileTypes)
        {
            for (var i = 0; i < VARIABLE.chance * numberOfBlocks; i++)
            {
                var tile = _tilePool.Get(GetRandomPosOffScreen(), GetRandomRotOffScreen());
                tile.SetTile(mapSo.tileTypes[typeCurrent]);
                tile._mainCamera = _camera;
                tile.gameObject.name = $"TILE: {tile.typeEnums}";
                TileData.Add(tile.gameObject, tile);
                yield return new WaitForSeconds(0.05f);
            }
            typeCurrent ++;
        }
    }
    
    
    
    /// <summary>
    /// Trả về vị trí ngẫu nhiên trong tấm nhìn của MainCamera
    /// </summary>
    /// <returns></returns>
    private Vector3 GetRandomPosOffScreen()
    {
        _randomX = Random.Range(0.05f, .95f);
        _randomY = Random.Range(0.05f, .95f);
        return _camera.ViewportToWorldPoint(new Vector3(_randomX, _randomY, 10f));
    }
    /// <summary>
    /// Trả về góc quay ngẫu nhiên từ 0-360 của 3 góc X, Y, Z
    /// </summary>
    /// <returns></returns>
    private static quaternion GetRandomRotOffScreen()
    {
        return quaternion.Euler(Random.Range(0, 360f), Random.Range(0, 360f), Random.Range(0, 360f));
    }

  
}
