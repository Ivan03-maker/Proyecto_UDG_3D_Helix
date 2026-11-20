using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public class PlayerController : MonoBehaviour
{
    [Header("Física en 3 Ejes (X, Y, Z)")]
    public float bounceForceY = 8f;
    public float fallMultiplier = 2.8f;
    public float maxFallSpeed = 18f;

    [Header("Racha y Velocidad Dinámica")]
    public int streakTarget = 3;
    public float speedBoostMultiplier = 1.65f;
    [Range(0.2f, 3.0f)]
    public float boostDuration = 2.0f;

    [Header("Efecto Visual de Estela")]
    public TrailRenderer trailRenderer;

    private Rigidbody rb;
    private GameObject lastScoredPlatform;
    private int currentStreak;
    private float boostTimer;
    private bool isBoostActive;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        if (trailRenderer == null)
        {
            trailRenderer = GetComponentInChildren<TrailRenderer>(true);
        }

        SetTrailActive(false);
    }

    void Update()
    {
        if (!isBoostActive) return;

        boostTimer -= Time.deltaTime;
        if (boostTimer <= 0f)
        {
            DeactivateSpeedBoost();
        }
    }

    void FixedUpdate()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");
        float baseMoveSpeed = GameManager.Instance != null ? GameManager.Instance.CurrentMoveSpeed : 6f;
        float finalSpeed = isBoostActive ? baseMoveSpeed * speedBoostMultiplier : baseMoveSpeed;

        float currentY = rb.linearVelocity.y;
        if (currentY < 0f)
        {
            currentY += Physics.gravity.y * (fallMultiplier - 1f) * Time.fixedDeltaTime;
            currentY = Mathf.Max(currentY, -maxFallSpeed);
        }

        rb.linearVelocity = new Vector3(inputX * finalSpeed, currentY, inputZ * finalSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Platform")) return;

        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, bounceForceY, rb.linearVelocity.z);

                if (collision.gameObject != lastScoredPlatform)
                {
                    lastScoredPlatform = collision.gameObject;
                    currentStreak++;

                    if (GameManager.Instance != null)
                    {
                        GameManager.Instance.RegisterSafeHeight(transform.position.y);
                        GameManager.Instance.AddScore(1);
                    }

                    if (currentStreak >= streakTarget)
                    {
                        currentStreak = 0;
                        ActivateSpeedBoost();
                    }
                }

                break;
            }
        }
    }

    private void ActivateSpeedBoost()
    {
        isBoostActive = true;
        boostTimer = Mathf.Clamp(boostDuration, 0.1f, 3.0f);
        SetTrailActive(true);
    }

    private void DeactivateSpeedBoost()
    {
        isBoostActive = false;
        boostTimer = 0f;
        SetTrailActive(false);
    }

    private void SetTrailActive(bool active)
    {
        if (trailRenderer == null) return;

        if (active)
        {
            trailRenderer.Clear();
            trailRenderer.enabled = true;
            trailRenderer.emitting = true;
        }
        else
        {
            trailRenderer.emitting = false;
            trailRenderer.enabled = false;
        }
    }
}