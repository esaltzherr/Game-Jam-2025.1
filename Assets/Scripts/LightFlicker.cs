using UnityEngine;
using UnityEngine.Rendering.Universal; // Required for Light2D

public class LightFlicker : MonoBehaviour
{
    private Light2D playerLight; 
    public float minIntensity = 0.8f; // Minimum light intensity
    public float maxIntensity = 1.2f; // Maximum light intensity
    public float flickerSpeed = 0.1f; // Speed of flickering

    private float targetIntensity;
    private float flickerTimer;

    void Start()
    {
        playerLight = GetComponent<Light2D>(); // Get the Light2D component
        targetIntensity = playerLight.intensity;
    }

    void Update()
    {
        flickerTimer -= Time.deltaTime;
        
        if (flickerTimer <= 0)
        {
            // Set a new random target intensity
            targetIntensity = Random.Range(minIntensity, maxIntensity);
            flickerTimer = flickerSpeed; // Reset the timer
        }

        // Smoothly transition to the new intensity
        playerLight.intensity = Mathf.Lerp(playerLight.intensity, targetIntensity, Time.deltaTime * 10f);
    }
}
