using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class CoreManager : MonoBehaviour
{
    public Camera mainCamera;
    public PlayerController playerController;
    public Transform parentSlots;
    [Space]
    public int timer; // thời gian mỗi game (s)
    
    [Header("Prefabs Spawn")]
    public Tile_UI tile_UIPrefab;
    public Slot_UI slot_UIPrefab;
    
    [Space]
    public UnityEvent OnMergeTileEvent, OnEndTimeEvent, OnLoseGameEvent, OnWinGameEvent;
    public event Action<int> OnChangeTimeEvent;
    
    
    // Create ObjectPools
    private ObjectPooler<Tile_UI> _poolTile;
    private readonly List<Slot_UI> currentSlots = new();  // 7 Slot chứa
    private readonly float _durationTween = .25f;
    private Coroutine _countdownCoroutine;
    private int timerTemp;

    
    private void Start()
    {
        Initialized();
        StartCountdown();
    }
    private void OnApplicationQuit()
    {
        DOTween.Clear();
    }

    private void Initialized()
    {
        for (var i = 0; i < 7; i++)
        {
            var slot = Instantiate(slot_UIPrefab, parentSlots);
            slot.gameObject.name = $"SLOT: {i + 1}";
            currentSlots.Add(slot);
        }
        
        _poolTile = new ObjectPooler<Tile_UI>(tile_UIPrefab, transform, 0);
        timerTemp = timer;
    }
    
    

    #region Handling choosing tile
    public void OnSelectTileEvent(Tile _selectedTile)
    {
        var newTile_UI = GetTileUI(_selectedTile);
        
        if (CheckTile(_selectedTile, out var emptyIndex) && currentSlots[emptyIndex].TileUI != null)
        {
            ChangeTile(emptyIndex);
        }
        
        currentSlots[emptyIndex].SetSlot(newTile_UI);
        newTile_UI.transform.DOMove(currentSlots[emptyIndex].RectTransform.position, _durationTween).OnComplete(() =>
        {
            if (CheckMerge(newTile_UI))
            {
                MergeTile(emptyIndex);
            }
            else if(currentSlots[^1].TileUI != null)
            {
                OnLoseGameEvent?.Invoke();
                OpenUI();
            }
        });
    }
    
    
    /// <summary>
    /// Lấy 1 TileUI từ pool tại vị trí Tile vừa chọn trên game
    /// </summary>
    /// <param name="_tile"> Tile vừa được chọn </param>
    /// <returns></returns>
    public Tile_UI GetTileUI(Tile _tile)
    {
        var screenPoint = mainCamera.WorldToScreenPoint(_tile.transform.position);
        var newTileUI = _poolTile.Get(screenPoint);
        newTileUI.SetTileUI(_tile);
        newTileUI.gameObject.name = newTileUI.typeEnums.ToString();
        return newTileUI;
    }
    
    
    /// <summary>
    /// Di chuyển các khối từ slot[fromIndex] ra sau 1 slot
    /// </summary>
    /// <param name="fromIndex"></param>
    private void ChangeTile(int fromIndex)
    {
        for (var i = currentSlots.Count - 2; i >= fromIndex; i--)
        {
            if(currentSlots[i].TileUI == null) continue;
            
            currentSlots[i + 1].SetSlot(currentSlots[i].TileUI);
            currentSlots[i].TileUI.transform.DOMove(currentSlots[i + 1].RectTransform.position, _durationTween);
        } 
    }
    
    
    /// <summary>
    /// Kiểm tra trong hàng chờ có Tile nào có cùng type với Tile vừa chọn ?. Nếu có trả về vị trí tiếp theo của Type đó trong hàng chờ.
    /// </summary>
    /// <param name="_tile"> Tile cần kiểm tra </param>
    /// <returns></returns>
    private bool CheckTile(Tile _tile, out int emptyIndex)
    {
        emptyIndex = currentSlots.FindIndex(slot => slot.TileUI == null); // tìm slot đang trống tile
        
        for (var i = currentSlots.Count - 1; i >= 0; i--)
        {
            if (currentSlots[i].TileUI == null || currentSlots[i].TileUI.typeEnums != _tile.tileType.typeEnums) 
                continue;
            emptyIndex = i + 1;
            return true;
        }
        return false;
    }
    
    
    /// <summary>
    /// Trong hàng chờ có trên 3 khối có cùng type không ?
    /// </summary>
    private bool CheckMerge(Tile_UI tileUI) => currentSlots.Count(slotUI => slotUI.TileUI != null && slotUI.TileUI.typeEnums == tileUI.typeEnums) >= 3;
    
    
    /// <summary>
    /// Hợp khối
    /// </summary>
    private void MergeTile(int _currentIndex)
    {
        OnMergeTileEvent?.Invoke();
        
        for (var i = _currentIndex - 2; i <= _currentIndex; i++)
        {
            Debug.Log("Slot: " + currentSlots[i]);
            currentSlots[i].SetSlot(null);
        }

        var _sequence = DOTween.Sequence();
        for (var i = _currentIndex + 1; i < currentSlots.Count; i++)
        {
            if(currentSlots[i].TileUI == null) 
                continue;
            
            var newTileUI = _poolTile.Get(currentSlots[i].RectTransform.position);
            newTileUI.SetTileUI(currentSlots[i].TileUI.tile);
            _sequence.Join(newTileUI.transform.DOMove(currentSlots[i - 3].RectTransform.position, _durationTween)).SetEase(Ease.Linear);
            currentSlots[i - 3].SetSlot(newTileUI);
            currentSlots[i].SetSlot(null);
        }
        
        _sequence.Play().OnComplete(() =>
        {
            if (Spawner.Instance.IsWin())
            {
                OnWinGameEvent?.Invoke();
                OpenUI();
            }
            else if(currentSlots[^1].TileUI != null)
            {
                OnLoseGameEvent?.Invoke();
                OpenUI();
            }
        });
    }
    #endregion
    
    
    // UI
    public void OpenUI()
    {
        playerController.PauseGame();
        StopCountdown();
    }
    public void CloseUI()
    {
        playerController.ResumeGame();
        StartCountdown();
    }
    
    
    // Countdown
    public void ResetCountdown()
    {
        timer = timerTemp;
        StartCountdown();
    }
    public void StartCountdown()
    {
        if(_countdownCoroutine != null)
            StopCoroutine(_countdownCoroutine);
        _countdownCoroutine = StartCoroutine(CountdownCoroutine());
    }
    public void StopCountdown()
    {
        if(_countdownCoroutine != null)
            StopCoroutine(_countdownCoroutine);
    }
    private IEnumerator CountdownCoroutine()
    {
        while (timer > 0)
        {
            timer -= 1;    
            OnChangeTimeEvent?.Invoke(timer);
            yield return new WaitForSeconds(1f);
        }
        OnEndTimeEvent?.Invoke();
        OpenUI();
        timer = timerTemp;
    }



    // Tools
    public void AddTime(int _seconds)
    {
        timer += _seconds;
        CloseUI();
    }
    public void AutoMerge()
    {
        var emptyIndex = currentSlots.FindIndex(slot => slot.TileUI == null);   // tìm vị trí slot đang trống Tile
       
        if (emptyIndex >= 4 || !Spawner.Instance.GetRandomTiles(3, out var _tiles))
        {
            Debug.Log("Không thực hiện được Tool !!!");
            return;
        } 
        
        var count = 0;
        var _sequence = DOTween.Sequence();
        for (var i = emptyIndex; i <= emptyIndex + 2; i++) // duyệt từ vị trí đang trống -> 2 vị trí tiếp theo
        {
            var newTileUI = GetTileUI(_tiles[count]);
            _sequence.Join(newTileUI.transform.DOMove(currentSlots[i].RectTransform.position, _durationTween));

            currentSlots[i].SetSlot(newTileUI);
            count++;
        }

        _tiles.ForEach(item => item.Release());
        _sequence.Play().OnComplete(() =>
        {
            MergeTile(emptyIndex + 2);
        });
        
        CloseUI();
    }
    public bool RemoveTile(int _count)
    {
        var findIndex = currentSlots.FindLastIndex(item => item.TileUI != null);
        CloseUI();
        switch (findIndex)
        {
            case -1:
                return false;
            case 0:
                Spawner.Instance.GetTile(currentSlots[0].TileUI.tile.tileType);
                currentSlots[0].SetSlot(null);
                break;
            default:
            {
                for (var i = findIndex; i > findIndex - _count; i--)
                {
                    Debug.Log(currentSlots[i].TileUI);
                    Spawner.Instance.GetTile(currentSlots[i].TileUI.tile.tileType);
                    currentSlots[i].SetSlot(null);
                }
                break;
            }
        }
        return true;
    }

}
