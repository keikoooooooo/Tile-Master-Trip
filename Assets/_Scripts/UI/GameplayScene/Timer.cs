using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    
    private int minute, second;
    private string formated;

    
    public void RegisterCountdownTime(CoreManager _coreManager)
    {
        OnChangeTimeEvent(_coreManager.timer);
        _coreManager.OnChangeTimeEvent += OnChangeTimeEvent;
    }

    public void UnRegisterCountdownTime(CoreManager _coreManager)
    {
        _coreManager.OnChangeTimeEvent -= OnChangeTimeEvent;
    }
    
    public void OnChangeTimeEvent(int _value)
    {
        minute = _value / 60;
        second = _value % 60;

        formated = $"{minute:00}:{second:00}";
        timerText.text = formated;
    }
    

}
