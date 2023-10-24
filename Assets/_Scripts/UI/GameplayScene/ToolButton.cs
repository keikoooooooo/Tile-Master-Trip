using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolButton : MonoBehaviour, IData
{
    [SerializeField] private List<GameObject> toolLocks;
    [SerializeField] private List<Button> bttTools;

    [Space]
    public CountdownButton returnCountdown;
    public CountdownButton autoMergeCountdown;
    public CountdownButton addTimeCountdown;
    
    private UserData _userData;
    
    
    private void OnEnable()
    {
        DataReference.Add(this);
    }
    private void OnDisable()
    {
        DataReference.Remove(this);
    }
    
    public void SendData(GameManager gameManager)
    {
        _userData = gameManager.UserData;
        SetToolButton();
    }
    public void SetToolButton()
    {
        var level = _userData.userLevel;
        
        for (var i = level - 1; i >= 0; i--)
        {
            toolLocks[i].SetActive(false);
            bttTools[i].interactable = true;
        }
    }
    
}
