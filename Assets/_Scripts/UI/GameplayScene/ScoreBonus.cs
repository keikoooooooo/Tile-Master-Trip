using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBonus : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI progressText;
    public Image fill;
 
    [Tooltip("Thời gian sẽ đặt lại điểm thưởng")]
    public float duration = 5f;

    public int Bonus { get; private set; }
    private Tween _progressTween;


    private void Start()
    {
        EndBonus();
    }
    private void OnDisable()
    {
        _progressTween?.Kill();
    }


    public void AddBonus()
    {
        panel.SetActive(true);
        Bonus += 1;
        progressText.text = $"x{Bonus}";
        
        fill.fillAmount = 1;
        
        _progressTween?.Kill();
        _progressTween = fill.DOFillAmount(0, duration).SetEase(Ease.Linear).OnComplete(EndBonus);
    }
    private void EndBonus()
    {
        Bonus = 0;
        progressText.text = $"x{Bonus}";
        fill.fillAmount = 0;
        panel.SetActive(false);
    }
    
}
