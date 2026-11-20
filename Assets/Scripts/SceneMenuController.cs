using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMenuController : MonoBehaviour
{
    [Header("Nombres de Escenas")]
    public string mainMenuSceneName = "MainMenuScene";
    public string level1SceneName = "GameplayScene";
    public string level2SceneName = "Level2Scene";

    public void LoadMainMenu()
    {
        LoadSceneSafe(mainMenuSceneName);
    }

    public void StartGameLevel1()
    {
        LoadSceneSafe(level1SceneName);
    }

    public void StartGameLevel2()
    {
        LoadSceneSafe(level2SceneName);
    }

    public void LoadNextLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            LoadSceneSafe(mainMenuSceneName);
        }
    }

    public void RestartCurrentLevel()
    {
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void LoadSceneSafe(string sceneName)
    {
        if (string.IsNullOrWhiteSpace(sceneName))
        {
            Debug.LogWarning("SceneMenuController: El nombre de la escena está vacío.");
            return;
        }

        if (SceneUtility.GetBuildIndexByScenePath($"Assets/Scenes/{sceneName}.unity") >= 0)
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError($"SceneMenuController: La escena '{sceneName}' no está registrada en Build Settings.");
        }
    }
}