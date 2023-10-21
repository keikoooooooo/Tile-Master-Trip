using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public Camera mainCamera;
    public Transform parentSlots;
    
    [Header("Prefabs Spawn")]
    public Tile_UI tile_UIPrefab;
    public Slot_UI slot_UIPrefab;

    // Create ObjectPools
    private ObjectPooler<Tile_UI> tileUI_Pool;
    
    // List
    public List<Slot_UI> currentSlots = new();         // 7 Slot chứa các Tile_UI
    
    private readonly float _durationTween = .35f;

    public TextMeshProUGUI textDemo;
    private void Start()
    {
        Initialized();
    }
    private void Update()
    {
        var Slot1 = currentSlots[0].TileUI == null ? "" : currentSlots[0].TileUI.typeEnums.ToString();
        var Slot2 = currentSlots[1].TileUI == null ? "" : currentSlots[1].TileUI.typeEnums.ToString();
        var Slot3 = currentSlots[2].TileUI == null ? "" : currentSlots[2].TileUI.typeEnums.ToString();
        var Slot4 = currentSlots[3].TileUI == null ? "" : currentSlots[3].TileUI.typeEnums.ToString();
        var Slot5 = currentSlots[4].TileUI == null ? "" : currentSlots[4].TileUI.typeEnums.ToString();
        var Slot6 = currentSlots[5].TileUI == null ? "" : currentSlots[5].TileUI.typeEnums.ToString();
        var Slot7 = currentSlots[6].TileUI == null ? "" : currentSlots[6].TileUI.typeEnums.ToString();
        
        textDemo.text = $"Slot 1: {Slot1}\n" +
                        $"Slot 2: {Slot2}\n" +
                        $"Slot 3: {Slot3}\n" +
                        $"Slot 4: {Slot4}\n" +
                        $"Slot 5: {Slot5}\n" +
                        $"Slot 6: {Slot6}\n" +
                        $"Slot 7: {Slot7}\n";
    }

    private void Initialized()
    {
        for (var i = 0; i < 7; i++)
        {
            var slot = Instantiate(slot_UIPrefab, parentSlots);
            slot.gameObject.name = $"SLOT: {i + 1}";
            currentSlots.Add(slot);
        }
        
        tileUI_Pool = new ObjectPooler<Tile_UI>(tile_UIPrefab, transform, 0);
    }
    
    
    public void OnSelectTileEvent(Tile _selectedTile)
    {
        if(_selectedTile == null) 
            return;
        
        var screenPosition = mainCamera.WorldToScreenPoint(_selectedTile.transform.position);
        var newTile_UI = tileUI_Pool.Get(screenPosition);
        newTile_UI.SetTileUI(_selectedTile);
        newTile_UI.gameObject.name = newTile_UI.typeEnums.ToString();
        
        if (CheckTile(_selectedTile, out var nextIndex))
        {
            if (currentSlots[nextIndex].TileUI == null)
            {
                newTile_UI.transform.DOMove(currentSlots[nextIndex].RectTransform.position, _durationTween).OnComplete(() =>
                {
                    MergeTile(nextIndex);
                });
            }
            else
            {
                MoveAndMergeTiles(nextIndex, newTile_UI);
            }
            currentSlots[nextIndex].SetSlot(newTile_UI);
        }
        else
        {
            currentSlots[FirstNonRef()].TileUI = newTile_UI;
            newTile_UI.transform.DOMove(currentSlots[nextIndex].RectTransform.position, _durationTween).OnComplete(() =>
            {
                if(currentSlots[^1].TileUI != null)
                {
                    GameOver();
                }
            });
        }
    }
    
    private void MoveAndMergeTiles(int _nextIndex, Component newTile_UI) // Di chuyển tile vừa chọn vào vị trí idx + 1 -> rồi Merge
    {
        for (var i = currentSlots.Count - 2; i >= _nextIndex; i--)
        {
            if (currentSlots[i].TileUI == null) continue;
            
            currentSlots[i + 1].SetSlot(currentSlots[i].TileUI);
            currentSlots[i].TileUI.transform.DOMove(currentSlots[i + 1].RectTransform.position, _durationTween);
        }
        newTile_UI.transform.DOMove(currentSlots[_nextIndex].RectTransform.position, _durationTween).OnComplete(() =>
        {
            Debug.Log("Đủ 3 Slot: MERGE");
            MergeTile(_nextIndex);
        });
    }
    private void MergeTile(int lastIndexMerge)
    {
        for (var i = lastIndexMerge - 2; i <= lastIndexMerge; i++)
        {
            currentSlots[i].SetSlot(null);
        }
        
        for (var i = lastIndexMerge + 1; i < currentSlots.Count; i++)
        {
            if (currentSlots[i].TileUI == null) 
                continue;
            
            var newTileUI = tileUI_Pool.Get(currentSlots[i].RectTransform.position);
            newTileUI.SetTileUI(currentSlots[i].TileUI.tile);
            currentSlots[i].SetSlot(null);
            currentSlots[i - 3].SetSlot(newTileUI);
            newTileUI.transform.DOMove(currentSlots[i - 3].RectTransform.position, _durationTween);
        }
    }
    
    
    /// <summary>
    /// Lấy vị trí đầu tiên trong hàng chờ Slot đang trống Tile
    /// </summary>
    /// <returns></returns>
    private int FirstNonRef() => currentSlots.FindIndex(slot => slot.TileUI == null);
    
    /// <summary>
    /// Nếu tile trong danh sách currentTiles có trên 2 phần tử có type == type của _tile tham số, Return True và trả về vị trí đằng sau của 2 tile.
    /// </summary>
    /// <param name="selectedTile"> Tile cần kiểm tra </param>
    /// <param name="nextIndex"> Vị trí để tile di chuyển tới </param>
    /// <returns></returns>
    private bool CheckTile(Tile selectedTile, out int nextIndex)
    {
        nextIndex = FirstNonRef();
        var foundFirst = false;
        var _currentIndex = 0;
        
        for (var i = 0; i < nextIndex; i++)
        {
            if (currentSlots[i].TileUI.typeEnums != selectedTile.typeEnums) 
                continue;
            
            if (!foundFirst)
            {
                foundFirst = true;
                _currentIndex = i;
            }
            else
            {
                if (Mathf.Abs(_currentIndex - i) != 1) return false;
                nextIndex = i + 1;
                return true;
            }
        }
        return false;
    }


    private void GameOver()
    {
        Debug.Log("GAMEOVER");
    }
    
    
    
}
