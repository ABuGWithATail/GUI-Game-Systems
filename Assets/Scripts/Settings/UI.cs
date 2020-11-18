using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
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
