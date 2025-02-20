using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class OptionsMenuController : MonoBehaviour
{
    private Canvas canvas;

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    void Start()
    {
        // Get the Canvas component on the same GameObject
        canvas = GetComponent<Canvas>();

        // Ensure the menu starts as disabled
        canvas.enabled = false;

        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);


    }

    void Update()
    {
        // Check for Escape key press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        // Toggle the Canvas on/off
        canvas.enabled = !canvas.enabled;
        if (canvas.enabled == true){
            // Initialize sliders with saved values or default to max volume
            masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        }
    }


    public void ExitToMain(string Scene)
    {
        Debug.Log("Going back to main menu");
        SceneManager.LoadScene(Scene);
    }
}
