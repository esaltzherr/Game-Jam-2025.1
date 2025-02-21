using UnityEngine;
using UnityEngine.Rendering.Universal; // Required for Light2D

public class LightFlicker : MonoBehaviour
{
    private Light2D playerLight;
    
    [Header("Torch Setup")]
    public TorchBar torchBar; // If assigned, intensity scales with torch
    
    [Header("Intensity Settings")]
    public float baseIntensity = 3f;  // Max intensity
    public float minDrop       = 0.8f; // Min intensity

    [Header("Flicker Settings")]
    public float flickerSpeed = 0.1f; // How often a new flicker target is chosen

    private float targetIntensity;
    private float flickerTimer;

    void Start()
    {
        playerLight = GetComponent<Light2D>();
        // If torchBar is null, we'll just do a normal flicker
        targetIntensity = playerLight.intensity;
    }

    void Update()
    {
        // 1) Determine the adjustedIntensity
        float adjustedIntensity;
        
        if (torchBar != null)
        {
            // We have a TorchBar, so fade intensity based on torch percentage
            float torchPercentage = torchBar.GetTorchPercentage() / 100f; // 0 to 1

            // Exponential drop-off
            adjustedIntensity = Mathf.Lerp(minDrop, baseIntensity, torchPercentage * torchPercentage);

            // If torch is empty, force light off
            if (torchPercentage <= 0)
            {
                playerLight.intensity = 0;
                return; // Skip flicker logic
            }
        }
        else
        {
            // No TorchBar, just flicker between minDrop and baseIntensity
            adjustedIntensity = Random.Range(minDrop, baseIntensity);
        }

        // 2) Flicker logic
        flickerTimer -= Time.deltaTime;
        if (flickerTimer <= 0)
        {
            // Pick a random target around our adjustedIntensity
            targetIntensity = Random.Range(adjustedIntensity - 0.2f, adjustedIntensity + 0.2f);
            flickerTimer = flickerSpeed; // reset timer
        }

        // 3) Smoothly move towards targetIntensity
        playerLight.intensity = Mathf.Lerp(playerLight.intensity, targetIntensity, Time.deltaTime * 10f);
    }
}
