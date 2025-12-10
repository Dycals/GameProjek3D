using UnityEngine;

public class ObjectiveMarker : MonoBehaviour
{
    private Renderer rend;
    private Material mat;

    public float pulseSpeed = 2f;

    public Color baseColor = Color.red;
    public float maxIntensity = 1.5f;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            mat = rend.material;
            mat.SetColor("_Color", baseColor);
        }
    }

    void Update()
    {
        float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;

        Color finalColor = baseColor * (1f + pulse * maxIntensity);

        mat.SetColor("_Color", finalColor);
    }
}