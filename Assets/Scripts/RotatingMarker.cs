using UnityEngine;

public class RotatingMarker : MonoBehaviour
{
    public float rotationSpeed = 90f;

    public float floatSpeed = 0.5f;
    public float floatMagnitude = 0.2f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.localPosition;
    }

    void Update()
    {        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);

        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatMagnitude;
        transform.localPosition = new Vector3(startPosition.x, newY, startPosition.z);
    }
}