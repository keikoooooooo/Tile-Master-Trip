using System.Collections;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private ScoreBonus scoreBonus;
    [SerializeField] private PlayerGUI playerGUI;

    [Space, Tooltip("Số điểm của level hiện tại chơi được")]
    [SerializeField] private TextMeshProUGUI scoreIngameText;

    public int scoreTemp { get; set; }
    private Coroutine _animatedScoreCoroutine;


    private void Start()
    {
        scoreTemp = 0;
    }

    public void AddScore()
    {
        var _score = 3;
        if (scoreBonus.Bonus != 0)
        {
            _score *= scoreBonus.Bonus;
        }
        
        if(_animatedScoreCoroutine != null) StopCoroutine(_animatedScoreCoroutine);
        _animatedScoreCoroutine = StartCoroutine(AnimatedScoreCoroutine(_score));
    }

    private IEnumerator AnimatedScoreCoroutine(int score)
    {
        scoreTemp += score;
        scoreIngameText.text = $"+{scoreTemp}";

        while (score > 0)
        {
            playerGUI.UserData.userScore += 1;
            playerGUI.SetScoreText();
            
            score -= 1;
            yield return new WaitForSeconds(.01f);
        }
    }
    
   
}
