using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Referencias de Juego")]
    public Transform player;
    public TMP_Text scoreText;
    public TMP_Text highScoreText;

    [Header("Dificultad Progresiva")]
    public float baseSpeed = 6f;
    public float maxSpeed = 14f;
    public float speedIncreasePerPoint = 0.15f;

    [Header("Bucle de Juego (Game Loop)")]
    public float fallDeathOffsetY = 12f;
    public float restartDelay = 0.35f;

    [Header("Transición de Nivel")]
    public bool advanceToNextLevel = true;
    public int scoreToAdvanceLevel = 15;
    public string nextLevelSceneName = "Level2Scene";

    [Header("Indicador de Nivel")]
    public TMP_Text levelText;
    public string levelLabel = "Nivel 1";

    public int Score { get; private set; }
    public int HighScore { get; private set; }
    public float CurrentMoveSpeed => Mathf.Min(baseSpeed + (Score * speedIncreasePerPoint), maxSpeed);

    private const string HighScoreKey = "Helix3D_HighScore";
    private float highestYReached;
    private bool isGameOver;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        HighScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        UpdateUI();
        UpdateLevelUI();
    }

    void Start()
    {
        if (player != null)
        {
            highestYReached = player.position.y;
        }
        UpdateLevelUI();
    }

    void Update()
    {
        if (isGameOver || player == null) return;

        if (player.position.y > highestYReached)
        {
            highestYReached = player.position.y;
        }

        // Si el jugador cae mucho por debajo de su última altura segura, reinicia el ciclo
        if (player.position.y < highestYReached - fallDeathOffsetY)
        {
            TriggerGameOver();
        }
    }

    public void RegisterSafeHeight(float newY)
    {
        if (newY > highestYReached || Mathf.Abs(highestYReached - newY) > fallDeathOffsetY * 0.5f)
        {
            highestYReached = newY;
        }
    }

    public void AddScore(int amount = 1)
    {
        if (isGameOver) return;

        Score += Mathf.Max(1, amount);

        if (Score > HighScore)
        {
            HighScore = Score;
            PlayerPrefs.SetInt(HighScoreKey, HighScore);
            PlayerPrefs.Save();
        }

        UpdateUI();

        if (advanceToNextLevel && !string.IsNullOrWhiteSpace(nextLevelSceneName) && Score >= scoreToAdvanceLevel)
        {
            isGameOver = true;
            PlayerPrefs.Save();
            SceneManager.LoadScene(nextLevelSceneName);
        }
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        PlayerPrefs.Save();
        Invoke(nameof(RestartCurrentScene), restartDelay);
    }

    private void RestartCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    private void UpdateUI()
    {
        if (scoreText != null) scoreText.text = $"Score: {Score}";
        if (highScoreText != null) highScoreText.text = $"Best: {HighScore}";
    }

    private void UpdateLevelUI()
    {
        if (levelText != null && !string.IsNullOrWhiteSpace(levelLabel))
        {
            levelText.text = levelLabel;
        }
    }
}