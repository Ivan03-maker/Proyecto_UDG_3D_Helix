using UnityEngine;

public class CameraFollowXZ : MonoBehaviour
{
    public Transform target;
    public float fixedZOffset = -7f;
    public float heightOffsetY = 3.5f;
    public float smoothSpeed = 8f;

    void LateUpdate()
    {
        if (target == null) return;

        float desiredY = Mathf.Min(transform.position.y, target.position.y + heightOffsetY);
        // Si el jugador cae mucho por debajo de la cámara, baja suavemente con él
        if (target.position.y + heightOffsetY < transform.position.y)
        {
            desiredY = target.position.y + heightOffsetY;
        }

        Vector3 desiredPosition = new Vector3(
            target.position.x,
            desiredY,
            target.position.z + fixedZOffset
        );

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}