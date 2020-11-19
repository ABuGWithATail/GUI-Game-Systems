using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    //super basic script for the pause menu and loss menu that takes us to where we need to go.
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void QuitGame()
    {
        Debug.Log("Exiting Game.");
        Application.Quit();
    }

}
