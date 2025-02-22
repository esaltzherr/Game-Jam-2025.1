using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    // public string SceneToStart;
    // public GameObject OptionsMenuObject;
    // public GameObject MainMenuObject;
    public AudioClip UIClickSound;
    public void StartGame(string Scene)
    {
        AudioManager.Instance.PlaySFX(UIClickSound);
        Debug.Log("Starting Game");
        SceneManager.LoadScene(Scene);
    }

    // public void GoToOptions(){
    //     Debug.Log("Moving To Options Menu");
    //     MainMenuObject.SetActive(false);
    //     OptionsMenuObject.SetActive(true);
    // }

    // public void GoToMain(){
    //     Debug.Log("Moving To Main Menu");
    //     MainMenuObject.SetActive(true);
    //     OptionsMenuObject.SetActive(false);
    // }

    public void ExitApplication() { 
        Debug.Log("BYE BYE");
        Application.Quit();
    }
}