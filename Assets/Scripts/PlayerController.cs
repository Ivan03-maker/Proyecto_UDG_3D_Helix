using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public class PlayerController3D : MonoBehaviour
{
    [Header("Física en 3 Ejes (X, Y, Z)")]
    public float bounceForceY = 8f;
    public float fallMultiplier = 2.8f;
    public float maxFallSpeed = 18f;
    public float stepDistanceForScore = 4f;

    private Rigidbody rb;
    private Vector3 lastScoredPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        lastScoredPosition = transform.position;
    }

    void FixedUpdate()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");
        float speed = GameManager.Instance != null ? GameManager.Instance.CurrentMoveSpeed : 6f;

        float currentY = rb.linearVelocity.y;
        if (currentY < 0f)
        {
            currentY += Physics.gravity.y * (fallMultiplier - 1f) * Time.fixedDeltaTime;
            currentY = Mathf.Max(currentY, -maxFallSpeed);
        }

        rb.linearVelocity = new Vector3(inputX * speed, currentY, inputZ * speed);

        // Incremento uniforme de puntaje según el avance horizontal/profundidad en 3D
        float distanceTraveled = Vector3.Distance(
            new Vector3(transform.position.x, 0f, transform.position.z),
            new Vector3(lastScoredPosition.x, 0f, lastScoredPosition.z)
        );

        if (distanceTraveled >= stepDistanceForScore)
        {
            lastScoredPosition = transform.position;
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(1);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
{
    if (!collision.gameObject.CompareTag("Platform")) return;

    foreach (ContactPoint contact in collision.contacts)
    {
        if (contact.normal.y > 0.5f)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, bounceForceY, rb.linearVelocity.z);
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(1);
            }
            break;
        }
    }
}
}