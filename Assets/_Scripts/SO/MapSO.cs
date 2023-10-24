using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "KitGame/Map/MAP Level", fileName = "MAP_")]
public class MapSO : ScriptableObject
{
   public string mapName;
   
   [Space] 
   public List<TileType> tileTypes;
}


[Serializable]
public class TileType
{
   [Tooltip("Loại khối, phân loại bằng Enum tương ứng tên của Icon")]
   public TileTypeEnums typeEnums;

   [Tooltip("Icon khối")]
   public Sprite sprite;

   [Tooltip("Tỉ lệ xuất hiện khối (x3) / 1 Chance")]
   public int chance = 1;
}

public enum TileTypeEnums{
   Tile_01,
   Tile_02,
   Tile_03,
   Tile_04,
   Tile_05,
   Tile_06
}