using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject titleScreen;
    public GameObject mainMenuObject;
    public GameObject settingsMenu;

    //this sscript is my original script from the first assignment, but I have lost the project and got the script, So I'm going to need to remake this script in a new project to get it to work again.
    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Debug.Log("Escape Pressed");
            if (titleScreen.activeInHierarchy == false)
            {
                Debug.Log("Hierarchy False");
                titleScreen.SetActive(true);
                mainMenuObject.SetActive(false);
                settingsMenu.SetActive(false);
            }
        }
    }

    private void OnGUI()
    {
        Event e = Event.current;
        if (e.isKey)
        {
            if(titleScreen.activeInHierarchy == true)
            {
                titleScreen.SetActive(false);
                mainMenuObject.SetActive(true);
                settingsMenu.SetActive(false);
            }
        }
    }

    public void QuitGame()
    {
        Debug.Log("Exiting Game.");
        Application.Quit();
    }
    
    public void GoToGame()
    {
        Debug.Log("See you in the game.");
        SceneManager.LoadScene("Alans");
    }


}
