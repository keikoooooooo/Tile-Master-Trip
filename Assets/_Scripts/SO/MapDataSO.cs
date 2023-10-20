using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "KitGame/Map/Data", fileName = "Data")]
public class MapDataSO : ScriptableObject
{
    public List<MapSO> maps;
}
