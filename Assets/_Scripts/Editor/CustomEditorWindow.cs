using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CustomEditorWindow : EditorWindow
{
    [MenuItem("Window/KitGame")]
    public static void ShowGameSetting()
    {
        var editor = GetWindow<CustomEditorWindow>();
        editor.titleContent = new GUIContent("Game Setting");
    }
    
    // Ref
    public MapDataSO mapDataSO;
    
    
    // Variables in ToolBar
    private readonly string[] _toolTitles = { "GAMEPLAY", "MAPS" };
    private int _selectedTool = -1;

    
    // Variables in Maps
    private Vector2 _mapScrollView;
    private Texture2D _iconArrowUp;
    private Texture2D _iconArrowDown;
    private TileType _tileTemp;
    private int _selectedMap = -1;
    
    
    private void OnEnable()
    {
        _iconArrowUp = EditorGUIUtility.Load("Assets/Resources/Sprite/Icon_Arrow_Up.png") as Texture2D;
        _iconArrowDown = EditorGUIUtility.Load("Assets/Resources/Sprite/Icon_Arrow_Down.png") as Texture2D;
    }
    public void OnGUI()
    {
        _selectedTool = GUILayout.Toolbar(_selectedTool, _toolTitles, Width(200), Height(35));
        switch (_selectedTool)
        {
            case 0: 
                HandleGamePlay();
                break;
            
            case 1:
                HandleMap();
                break;
            
            default: 
                Debug.Log("No Selected");
                break;
        }
    }

    
    #region GAMEPLAY
    private void HandleGamePlay()
    {
        
    }
    #endregion

    #region MAPS
    private void HandleMap()
    {
        mapDataSO = (MapDataSO)EditorGUILayout.ObjectField("Map Data", mapDataSO, typeof(MapDataSO), false);
        if (mapDataSO == null || mapDataSO.maps == null)
        {
            EditorGUILayout.HelpBox("Assign a Map Data object.", MessageType.Info);
            return;
        }
        LoadMap();
        
        if(_selectedMap == -1 || mapDataSO.maps.Count == 0 || mapDataSO.maps == null || mapDataSO.maps[_selectedMap] == null)
            return;
        
        EditorGUI.BeginChangeCheck();
        _mapScrollView = GUILayout.BeginScrollView(_mapScrollView);
        
        ShowMapDetails();
        ShowTileDetails();
        
        GUILayout.EndScrollView();
        if (EditorGUI.EndChangeCheck()) EditorUtility.SetDirty(mapDataSO.maps[_selectedMap]);
        
    }
    private void LoadMap() // Tải dữ liệu Map
    {
        Space(10);
        
        if (mapDataSO.maps.Count <= 0)
        {
            if (GUILayout.Button("+", Width(30), Height(30)))
            {
                CreateMapLevel(); // khi nhấn button thì sẽ tạo 1 MapSO
            }
        }
        else
        {
            GUILayout.BeginVertical();
            for (var i = 0; i < mapDataSO.maps.Count; i++)
            {
                if (i % 10 == 0)
                {
                    if(i > 0)
                        GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                }
                
                if (GUILayout.Button($"Map {i + 1}", Width(80), Height(30)))
                {
                    _selectedMap = i;
                }
            }

            if (GUILayout.Button("+", Width(30), Height(30)))
            {
                CreateMapLevel(); // khi nhấn button thì sẽ tạo 1 MapSO
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
        
        _selectedMap = Mathf.Clamp(_selectedMap, 0, mapDataSO.maps.Count - 1);
    }
    private void CreateMapLevel() // Tạo thêm 1 MapSO mới
    {
        var mapSO = CreateInstance<MapSO>();
        var _pathMapSO = $"Assets/Resources/SO/MAPS/MAP{mapDataSO.maps.Count + 1}.asset";
        
        AssetDatabase.CreateAsset(mapSO, _pathMapSO);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        mapDataSO.maps.Add(AssetDatabase.LoadAssetAtPath<MapSO>(_pathMapSO));
    }
    private void CreateTileType() // Thêm 1 loại ngói vào Map 
    {
        mapDataSO.maps[_selectedMap].tileTypes ??= new List<TileType>();
        mapDataSO.maps[_selectedMap].tileTypes.Add(new TileType());
    }
    private void OnButtonClick(int idxCurrent, int idxMax) // Khi nhấn các button trong Option chi tiết ngói 
    {
        if(idxCurrent < 1 || idxCurrent >= idxMax)
            return;
        
        if (GUILayout.Button(_iconArrowUp, Width(20), Height(20)))
        {
            SwapMap(idxCurrent, idxCurrent - 1);
        }
        if (GUILayout.Button(_iconArrowDown, Width(20), Height(20)))
        {
            SwapMap(idxCurrent, idxCurrent + 1);
        }
        if(GUILayout.Button("X", Width(20), Height(20)))
        {
            mapDataSO.maps[_selectedMap].tileTypes.Remove(mapDataSO.maps[_selectedMap].tileTypes[idxCurrent]);
        }
    }
    private void SwapMap(int min, int max) // Swap 2 ngói với nhau 
    {
        _tileTemp = mapDataSO.maps[_selectedMap].tileTypes[min];
        mapDataSO.maps[_selectedMap].tileTypes[min] = mapDataSO.maps[_selectedMap].tileTypes[max];
        mapDataSO.maps[_selectedMap].tileTypes[max] = _tileTemp;
        _tileTemp = null;
    }
    
    private void ShowMapDetails() // Hiển thị chi tiết Map 
    {        
        Space(10);
        GUILayout.Label("Map Details", EditorStyles.boldLabel);
        
        mapDataSO.maps[_selectedMap].mapName  = EditorGUILayout.TextField("Map Name",  mapDataSO.maps[_selectedMap].mapName, Width(300));
        mapDataSO.maps[_selectedMap].level    = EditorGUILayout.IntField( "Level",         mapDataSO.maps[_selectedMap].level,   Width(300));
        mapDataSO.maps[_selectedMap].playTime = EditorGUILayout.IntField( "Play Time (s)", mapDataSO.maps[_selectedMap].playTime,Width(300));
    }
    private void ShowTileDetails() // Hiển thị chi tiết ngói của Map
    {
        Space(15);
        GUILayout.Label("Tile Details", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Idx", Width(30));
        GUILayout.Label("Type", Width(100));
        GUILayout.Label("Sprite", Width(100));
        
        if (GUILayout.Button("+", Width(20), Height(20))) 
            CreateTileType();
        
        GUILayout.EndHorizontal();
        
        if(mapDataSO.maps[_selectedMap].tileTypes == null || mapDataSO.maps[_selectedMap].tileTypes.Count == 0)
            return;
        
        for (var i = 0; i <  mapDataSO.maps[_selectedMap].tileTypes.Count; i++)
        {
            GUILayout.BeginHorizontal();
            
            GUILayout.Label(i.ToString(), Width(30));
            var tile = mapDataSO.maps[_selectedMap].tileTypes[i];
            tile.typeEnums = (TileType.TileTypeEnums)EditorGUILayout.EnumPopup("", tile.typeEnums, Width(100));
            tile.sprite = (Sprite)EditorGUILayout.ObjectField(tile.sprite, typeof(Sprite), false, Width(75), Height(75));
            OnButtonClick(i, mapDataSO.maps[_selectedMap].tileTypes.Count - 1);
            
            GUILayout.EndHorizontal();
        }
    }
    #endregion


    
    
    // Static Methods
    private static void Space(float _space) => GUILayout.Space(_space);
    private static GUILayoutOption Width(float _width) => GUILayout.Width(_width);
    private static GUILayoutOption Height(float _height) => GUILayout.Height(_height);
}
