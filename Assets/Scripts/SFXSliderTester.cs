using UnityEngine;
using UnityEngine.UI;

public class SFXSliderTester : MonoBehaviour
{
    public Slider sfxSlider;
    public AudioClip[] testSounds; // List of test SFX clips
    public float cooldownTime = 0.5f; // Cooldown between sounds

    private float lastPlayTime = 0f; // Tracks last time a sound played

    void Start()
    {
        if (sfxSlider != null)
            sfxSlider.onValueChanged.AddListener(OnSliderChanged);
    }

    void OnSliderChanged(float value)
    {
        // Check cooldown
        if (Time.time - lastPlayTime < cooldownTime)
            return;

        lastPlayTime = Time.time; // Update last play time

        // Play a test sound
        if (testSounds.Length > 0)
        {
            AudioClip clip = testSounds[Random.Range(0, testSounds.Length)];
            AudioManager.Instance.PlaySFX(clip);
        }
    }
}
