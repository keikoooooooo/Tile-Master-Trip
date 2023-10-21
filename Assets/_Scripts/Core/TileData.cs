using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileData
{
    private static readonly Dictionary<GameObject, Tile> _dictionary = new();

    
    public static bool Contains(GameObject _col, out Tile tile) => _dictionary.TryGetValue(_col, out tile);

    
    public static void Add(GameObject _col, Tile _tile)
        => _dictionary.TryAdd(_col, _tile);
    
    public static void Remove(GameObject _col)
    {
        if (_dictionary.ContainsKey(_col))
            _dictionary.Remove(_col);
    }

    
}
