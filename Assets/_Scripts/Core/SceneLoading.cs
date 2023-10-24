using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneLoading : Singleton<SceneLoading>
{
    public CanvasGroup panelLoading;

    private readonly float _durationTween = .15f;
    
    public void LoadScene(string _sceneName)
    {
        if (string.IsNullOrEmpty(_sceneName))
            return;
        
        var scene = SceneManager.LoadSceneAsync(_sceneName);
        scene.allowSceneActivation = false;
        
        panelLoading.alpha = 0;
        panelLoading.DOFade(1, _durationTween).OnComplete(() =>
        {
            scene.allowSceneActivation = true;
            panelLoading.DOFade(0, _durationTween);
        });
    }
}
