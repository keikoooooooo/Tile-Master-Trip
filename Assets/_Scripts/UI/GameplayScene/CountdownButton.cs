using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CountdownButton : MonoBehaviour
{
    public Button bttHandle;
    public Image fill;
    public float _durationTween = .5f;
    
    private Tween _fillTween;
    
    public void StartCountdown()
    {
        bttHandle.interactable = false;
        _fillTween?.Kill();

        fill.fillAmount = 1;
        fill.DOFillAmount(0, _durationTween).OnComplete(() =>
        {
            bttHandle.interactable = true;
        });
    }
    
}
