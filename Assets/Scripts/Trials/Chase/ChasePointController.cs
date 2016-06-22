using UnityEngine;
using System.Collections;

public class ChasePointController : MonoBehaviour
{
    static float LightFillDuration = 5;

    public Color FillColor;

    public bool BeingFilled { get; set; }
    public bool Full { get { return lightLevel >= 1; } }

    Color startColor;
    float lightLevel = 0;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (!BeingFilled) return;

        lightLevel += Time.deltaTime / LightFillDuration;
        spriteRenderer.color = Color.Lerp(startColor, FillColor, lightLevel);
    }
}