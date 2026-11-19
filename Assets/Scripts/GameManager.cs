using UnityEngine;
using TMPro; // Si usas Text (Legacy), cambia TMP_Text por Text y quita using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Interfaz de Usuario")]
    public TMP_Text scoreText;
    public TMP_Text highScoreText;

    [Header("Dificultad Progresiva")]
    public float baseSpeed = 6f;
    public float maxSpeed = 14f;
    public float speedIncreasePerPoint = 0.15f;

    public int Score { get; private set; }
    public int HighScore { get; private set; }
    public float CurrentMoveSpeed => Mathf.Min(baseSpeed + (Score * speedIncreasePerPoint), maxSpeed);

    private const string HighScoreKey = "Helix3D_HighScore";

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        HighScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        UpdateUI();
    }

    public void AddScore(int amount = 1)
    {
        Score += Mathf.Max(1, amount);

        if (Score > HighScore)
        {
            HighScore = Score;
            PlayerPrefs.SetInt(HighScoreKey, HighScore);
            PlayerPrefs.Save();
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (scoreText != null) scoreText.text = $"Score: {Score}";
        if (highScoreText != null) highScoreText.text = $"Best: {HighScore}";
    }
}