using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


public class PauseAndSettings : MonoBehaviour
{
    public GameObject settingsMenu;
    public Resolution[] resolutions;
    public Dropdown resolution;
    private Scene currentScene;
    public AudioMixer masterAudio;

    //this script is my original script from the first assignment, but I have lost the project and got the script, So I'm going to need to remake this script in a new project to get it to work again.

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        Time.timeScale = 1;

        resolutions = Screen.resolutions;
        resolution.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option); 
            

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
            resolution.AddOptions(options);
            resolution.value = currentResolutionIndex;
            resolution.RefreshShownValue();
        }
    }

    public void SetResolution(int _resolutionIndex)
    {
        Resolution res = resolutions[_resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void Quality(int _qualityIndex)
    {
        QualitySettings.SetQualityLevel(_qualityIndex);
    }

   public void ChangeMusicVolume(float volume)
    {
        masterAudio.SetFloat("MusicVolume", volume);

    }
    public void ChangeSFXVolume(float volume)
    {
        masterAudio.SetFloat("SfxVolume", volume);
    }

    public void ToggleMute(bool isMuted)
    {
        if (isMuted)
        {
            masterAudio.SetFloat("MusicVolume", -80);
            masterAudio.SetFloat("SfxVolume", -80);
        }
        else
        {
            masterAudio.SetFloat("MusicVolume", 0);
            masterAudio.SetFloat("SfxVolume", 0);
        }
    }
   
    public void UnPause()
    {
        Time.timeScale = 1;
        settingsMenu.SetActive(false);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("mainMenu");
    }

    private void Update()
    {
        string sceneName = currentScene.name;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsMenu.activeInHierarchy == false)
            {
                Time.timeScale = 1;
                settingsMenu.SetActive(false);
            }
            if (sceneName == "Alans")
            {
                Time.timeScale = 0;
                settingsMenu.SetActive(true);
            }
            else
            {
                return;
            }


        }
    }

}
