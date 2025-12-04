using UnityEngine;

public class CameraFixedAngle : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;

    
    private Vector3 offset;

    
    void Start()
    {
        
        if (target == null)
        {
            Debug.LogError("Target untuk kamera belum diatur!");
            return;
        }

        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        if (target == null) return;

        
        Vector3 desiredPosition = target.position + offset;

        
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}