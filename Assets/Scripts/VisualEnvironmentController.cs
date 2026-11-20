using UnityEngine;

public class VisualEnvironmentController : MonoBehaviour
{
    [Header("Fondo Dinámico del Entorno")]
    public Camera targetCamera;
    public Color[] backgroundPalette = new Color[]
    {
        new Color(0.14f, 0.18f, 0.32f),
        new Color(0.10f, 0.28f, 0.34f),
        new Color(0.28f, 0.14f, 0.28f),
        new Color(0.12f, 0.24f, 0.18f)
    };
    public int pointsPerColorChange = 5;
    public float transitionSpeed = 3f;

    [Header("Personalización Visual de Player")]
    public Renderer playerRenderer;
    public Color[] playerColors = new Color[]
    {
        new Color(1.0f, 0.42f, 0.21f),
        new Color(0.20f, 0.80f, 0.95f),
        new Color(0.98f, 0.78f, 0.18f),
        new Color(0.65f, 0.35f, 0.95f)
    };
    [Range(0f, 1f)] public float smoothness = 0.65f;
    [Range(0f, 2f)] public float emissionIntensity = 0.35f;
    public bool changePlayerColorWithScore = true;

    private Material playerMaterialInstance;
    private int currentBackgroundIndex = -1;
    private int currentPlayerColorIndex = -1;

    void Awake()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        if (playerRenderer != null)
        {
            playerMaterialInstance = playerRenderer.material;
        }
    }

    void Start()
    {
        ApplyBackgroundIndex(0, instant: true);
        ApplyPlayerVisual(0);
    }

    void Update()
    {
        int score = GameManager.Instance != null ? GameManager.Instance.Score : 0;

        if (backgroundPalette != null && backgroundPalette.Length > 0)
        {
            int nextBgIndex = (score / Mathf.Max(1, pointsPerColorChange)) % backgroundPalette.Length;
            if (nextBgIndex != currentBackgroundIndex)
            {
                currentBackgroundIndex = nextBgIndex;
            }

            if (targetCamera != null)
            {
                targetCamera.backgroundColor = Color.Lerp(
                    targetCamera.backgroundColor,
                    backgroundPalette[currentBackgroundIndex],
                    transitionSpeed * Time.deltaTime
                );
            }
        }

        if (changePlayerColorWithScore && playerColors != null && playerColors.Length > 0)
        {
            int nextPlayerIndex = (score / Mathf.Max(1, pointsPerColorChange)) % playerColors.Length;
            if (nextPlayerIndex != currentPlayerColorIndex)
            {
                ApplyPlayerVisual(nextPlayerIndex);
            }
        }
    }

    public void SetPlayerColor(Color newColor)
    {
        if (playerMaterialInstance == null) return;

        playerMaterialInstance.color = newColor;

        if (playerMaterialInstance.HasProperty("_Smoothness"))
        {
            playerMaterialInstance.SetFloat("_Smoothness", smoothness);
        }

        if (playerMaterialInstance.HasProperty("_EmissionColor"))
        {
            playerMaterialInstance.EnableKeyword("_EMISSION");
            playerMaterialInstance.SetColor("_EmissionColor", newColor * emissionIntensity);
        }
    }

    private void ApplyBackgroundIndex(int index, bool instant)
    {
        if (backgroundPalette == null || backgroundPalette.Length == 0 || targetCamera == null) return;
        currentBackgroundIndex = Mathf.Abs(index) % backgroundPalette.Length;
        if (instant)
        {
            targetCamera.backgroundColor = backgroundPalette[currentBackgroundIndex];
        }
    }

    private void ApplyPlayerVisual(int index)
    {
        if (playerColors == null || playerColors.Length == 0) return;
        currentPlayerColorIndex = Mathf.Abs(index) % playerColors.Length;
        SetPlayerColor(playerColors[currentPlayerColorIndex]);
    }
}