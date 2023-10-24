using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Source")] 
    public AudioSource MusicSource;
    public AudioSource SFXSource;
    
    public void Play(AudioClip _audioClip, AudioMode _audioMode)
    {
        switch (_audioMode)
        {
            case AudioMode.Music:
                MusicSource.clip = _audioClip;
                MusicSource.Play();
                break;
            
            case AudioMode.SFX:
                SFXSource.PlayOneShot(_audioClip);
                break;
        }
    }
    
    public void Mute(AudioMode _mode, bool isMute)
    {
        switch (_mode)
        {
            case AudioMode.Music:
                MusicSource.mute = isMute;
                break;
            
            case AudioMode.SFX:
                SFXSource.mute = isMute;
                break;
        }
    }
}


public enum AudioMode
{
    Music,
    SFX
}