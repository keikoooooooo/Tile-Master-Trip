using System;
using TMPro;
using UnityEngine;

public class PlayerGUI : MonoBehaviour, IData
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI levelText;

    public UserData UserData { get; private set; }


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
        UserData = gameManager.UserData;

        SetScoreText();
        SetCoinText();
        SetLevelText();
    }

    public void SetScoreText()
    {
        if(!scoreText || UserData == null) return;
        
        scoreText.text = $"{UserData.userScore}";
    }
    public void SetCoinText()
    {
        if(!coinText || UserData == null) return;
        
        coinText.text = $"{UserData.userCoin}";
    }
    public void SetLevelText()
    {
        if(!levelText || UserData == null) return;
        
        levelText.text = $"{UserData.userLevel}";
    }

    
}
