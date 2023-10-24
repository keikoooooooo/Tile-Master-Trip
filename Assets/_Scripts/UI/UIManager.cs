using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void QuitGame() => Application.Quit();
    public void LoadScene(string sceneName) => SceneLoading.Instance.LoadScene(sceneName);
    public void ReleaseTile() => Spawner.Instance.ReleaseAll();
}
