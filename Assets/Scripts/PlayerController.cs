using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public class PlayerController3D : MonoBehaviour
{
    [Header("Movimiento Horizontal")]
    public float moveSpeedX = 6f;
    public float moveSpeedZ = 6f;

    [Header("Física de Rebote y Caída (Eje Y)")]
    public float bounceForceY = 8f;
    public float fallMultiplier = 2.8f;
    public float maxFallSpeed = 18f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // Evita que la esfera ruede de forma errática y descontrole los ejes
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        float currentY = rb.linearVelocity.y;

        // Acelera la caída cuando el personaje empieza a bajar (velocidad Y negativa)
        if (currentY < 0f)
        {
            currentY += Physics.gravity.y * (fallMultiplier - 1f) * Time.fixedDeltaTime;
            currentY = Mathf.Max(currentY, -maxFallSpeed);
        }

        rb.linearVelocity = new Vector3(inputX * moveSpeedX, currentY, inputZ * moveSpeedZ);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Platform")) return;

        // Verifica que el impacto sea desde arriba (la normal apunta hacia arriba)
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, bounceForceY, rb.linearVelocity.z);
                break;
            }
        }
    }
}