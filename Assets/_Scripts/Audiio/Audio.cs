using UnityEngine;

public class Audio : MonoBehaviour
{
    public AudioClip audioClip;
    public AudioMode audioMode;

    public void Play() => AudioManager.Instance.Play(audioClip, audioMode);
    
    public void Mute(bool isMute) => AudioManager.Instance.Mute(audioMode, !isMute);

}
