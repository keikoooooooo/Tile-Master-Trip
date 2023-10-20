using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "KitGame/Map/MAP Level", fileName = "MAP_")]
public class MapSO : ScriptableObject
{
   public string mapName;

   public int level;

   public int playTime;
   
   [Space] 
   public List<TileType> tileTypes;
   
}


[Serializable]
public class TileType
{
   public TileTypeEnums typeEnums;

   public Sprite sprite;
   
   public enum TileTypeEnums{
      Tile_01,
      Tile_02,
      Tile_03,
      Tile_04,
      Tile_05,
      Tile_06
   }
}