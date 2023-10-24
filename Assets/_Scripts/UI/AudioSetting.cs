using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour, IData
{
    public Toggle bttSound, bttMusic;

    [Space]
    public Slider soundSlider;
    public Slider musicSlider;
    
    [Space]
    public Image soundFrame;
    public Image musicFrame;
    
    [Space]
    public Color onColorButton;
    public Color offColorButton;
    
    private SettingData _settingData;
    private readonly float _durationTween = .15f;

    private Sequence _sequence;

    
    private void OnEnable()
    {
        RegisterEvent();
        DataReference.Add(this);
        
        if(GameManager.Instance) GameManager.Instance.SendData();
    }
    private void OnDisable()
    {
        UnRegisterEvent();
        DataReference.Remove(this);
        _sequence?.Kill();
    }
    
    private void RegisterEvent()
    {
        bttSound.onValueChanged.AddListener(OnClickSoundButton);   
        bttMusic.onValueChanged.AddListener(OnClickMusicButton);   
    }
    private void UnRegisterEvent()
    {
        bttSound.onValueChanged.RemoveListener(OnClickSoundButton);   
        bttMusic.onValueChanged.RemoveListener(OnClickMusicButton);   
    }

    
    public void SendData(GameManager gameManager)
    {
        _settingData = gameManager.SettingData;

        if(_settingData == null)
            return;
        
        bttSound.isOn = _settingData.sound;
        bttMusic.isOn = _settingData.music;

        SetInitAudioSource();
    }

    private void SetInitAudioSource()
    {
        AudioManager.Instance.SFXSource.gameObject.SetActive(false);
        AudioManager.Instance.SFXSource.mute = !bttSound.isOn;
        AudioManager.Instance.MusicSource.mute = !bttMusic.isOn;
        AudioManager.Instance.SFXSource.gameObject.SetActive(true);
    }
    public void OnClickOpenSettingButton()
    {
        soundSlider.value = bttSound.isOn ? 1 : 0;
        soundFrame.color = bttSound.isOn ? onColorButton : offColorButton;

        musicSlider.value = bttMusic.isOn ? 1 : 0;
        musicFrame.color = bttMusic.isOn ? onColorButton : offColorButton;
    }
    

    private void OnClickSoundButton(bool isOn)
    {
        _settingData.sound = isOn;
        
        _sequence = DOTween.Sequence();
        if (isOn)
        {
            _sequence.Join(soundSlider.DOValue(1, _durationTween));
            _sequence.Join(soundFrame.DOColor(onColorButton, _durationTween));
        }
        else
        {
            _sequence.Join(soundSlider.DOValue(0, _durationTween));
            _sequence.Join(soundFrame.DOColor(offColorButton, _durationTween));
        }
        _sequence.Play();
    }
    private void OnClickMusicButton(bool isOn)
    {
        _settingData.music = isOn;
        
        _sequence = DOTween.Sequence();
        if (isOn)
        {
            _sequence.Join(musicSlider.DOValue(1, _durationTween));
            _sequence.Join(musicFrame.DOColor(onColorButton, _durationTween));
        }
        else
        {
            _sequence.Join(musicSlider.DOValue(0, _durationTween));
            _sequence.Join(musicFrame.DOColor(offColorButton, _durationTween));
        }
        _sequence.Play();
    }
    
    
}
