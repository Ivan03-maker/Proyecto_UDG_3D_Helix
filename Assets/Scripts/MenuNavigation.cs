using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNavigation : MonoBehaviour
{
    public string gameplaySceneName = "GamePlayScene";

    public void StartGame()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }
}