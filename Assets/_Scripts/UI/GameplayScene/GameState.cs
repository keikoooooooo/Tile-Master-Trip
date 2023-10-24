using System;
using UnityEngine;

public class GameState : MonoBehaviour, IData
{
    [SerializeField] private CoreManager coreManager;
    [Space] 
    [SerializeField] private Timer timer;
    [SerializeField] private Score score;
    [SerializeField] private PlayerGUI playerGUI;
    [SerializeField] private ToolButton toolButton;
    
    [Space, Header("Panel PopUp")]
    public Animator timeIsUpPanelAnim;
    public Animator losePanelAnim;
    public Animator endgameAnim;
    public Animator purchaseAnim;
    public Animator notEnoughPopUpText;
    
    private UserData _userData;
    
    
    private void OnEnable()
    {
        DataReference.Add(this);
    }
    private void Start()
    {
        timer.RegisterCountdownTime(coreManager);
    }
    private void OnDisable()
    {
        DataReference.Remove(this);
    }
    private void OnDestroy()
    {
        timer.UnRegisterCountdownTime(coreManager);
    }
    
    public void SendData(GameManager gameManager)
    {
        _userData = gameManager.UserData;
        Spawner.Instance.Spawn(_userData.userLevel);
    }

    
    private bool CheckPrice(int price)
    {
        if (_userData.userCoin < price)
        {
            notEnoughPopUpText.Play("NotEnoughText");
            return false;
        }
        _userData.userCoin -= price;
        playerGUI.SetCoinText();
        return true;
    }
    
    
    // Onlick Tools Button
        // Tool PanelPopUp
    public void OnClickRemoveTileButton(int price) // chơi lại (quay ngược lại 3 khối) 
    {
        if(!CheckPrice(price))
            return;
        
        losePanelAnim.Play("GUI_OUT");
        coreManager.RemoveTile(3);
    }
    public void OnClickContinueButton()            // chơi tiếp màn tiếp theo 
    {
        score.scoreTemp = 0;
        
        _userData.userCoin += 500;
        _userData.userLevel += 1;
        if (_userData.userLevel > 3)
        {
            endgameAnim.Play("GUI_IN");
            coreManager.OpenUI();
            return;
        }
        
        playerGUI.SetLevelText();
        playerGUI.SetCoinText();
        
        coreManager.CloseUI();
        coreManager.ResetCountdown();
        toolButton.SetToolButton();
        Spawner.Instance.Spawn(_userData.userLevel);
    }
        // Tool Ingame
    public void OnClickReturnButton(int price)     // quay lại 1 khối trước đó 
    {
        purchaseAnim.Play("GUI_OUT");
        if (_userData.userCoin < price)
        {
            notEnoughPopUpText.Play("NotEnoughText");
            return;
        }
        
        if (!coreManager.RemoveTile(1)) 
            return;
        
        toolButton.returnCountdown.StartCountdown();
        _userData.userCoin -= price;
        playerGUI.SetCoinText();
    }
    public void OnClickAutoMergeButton(int price)  // tự động merge 
    {
        if(!CheckPrice(price))
            return;
        
        toolButton.autoMergeCountdown.StartCountdown();
        purchaseAnim.Play("GUI_OUT");
        coreManager.AutoMerge();
    }
    public void OnClickAddTimeButton(int price)    // thêm thời gian (+60s) 
    {
        if(!CheckPrice(price))
            return;
        
        toolButton.addTimeCountdown.StartCountdown();
        purchaseAnim.Play("GUI_OUT");
        timeIsUpPanelAnim.Play("GUI_OUT");
        coreManager.AddTime(60);
    }
 
    
}
