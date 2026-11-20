using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public class PlayerController : MonoBehaviour
{
    [Header("Física en 3 Ejes (X, Y, Z)")]
    public float bounceForceY = 8f;
    public float fallMultiplier = 2.8f;
    public float maxFallSpeed = 18f;

    private Rigidbody rb;
    private GameObject lastScoredPlatform;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
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
                    if (GameManager.Instance != null)
                    {
                        GameManager.Instance.RegisterSafeHeight(transform.position.y);
                        GameManager.Instance.AddScore(1);
                    }
                }

                break;
            }
        }
    }
}