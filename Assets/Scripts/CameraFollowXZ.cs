using UnityEngine;

public class CameraFollowXZ : MonoBehaviour
{
    public Transform target;
    public float fixedZOffset = -8f;
    public float fixedY;

    void Start()
    {
        fixedY = transform.position.y;
        if (target != null && Mathf.Approximately(fixedZOffset, 0f))
        {
            fixedZOffset = transform.position.z - target.position.z;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        float newX = target.position.x;
        float newZ = target.position.z + fixedZOffset;
        transform.position = new Vector3(newX, fixedY, newZ);
    }
}