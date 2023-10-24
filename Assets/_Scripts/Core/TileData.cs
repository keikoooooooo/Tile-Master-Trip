using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileData
{
    private static readonly Dictionary<GameObject, Tile> _dictionary = new();

    
    public static bool Contains(GameObject _obj, out Tile tile) => _dictionary.TryGetValue(_obj, out tile);
    
    public static void Add(GameObject _obj, Tile _tile) => _dictionary.TryAdd(_obj, _tile);
    
    public static void Remove(GameObject _obj)
    {
        if (_dictionary.ContainsKey(_obj))
            _dictionary.Remove(_obj);
    }

    
}
