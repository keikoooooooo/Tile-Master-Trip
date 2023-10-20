using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileData
{
    private static readonly Dictionary<Collider, Tile> _dictionary = new();

    
    public static bool Contains(Collider _col, out Tile tile) => _dictionary.TryGetValue(_col, out tile);

    
    public static void Add(Collider _col, Tile _tile)
        => _dictionary.TryAdd(_col, _tile);
    
    public static void Remove(Collider _col)
    {
        if (_dictionary.ContainsKey(_col))
            _dictionary.Remove(_col);
    }

    
}
