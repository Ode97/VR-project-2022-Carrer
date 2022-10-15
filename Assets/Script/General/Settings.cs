using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private GameObject setting;
    [SerializeField] private GameObject settingButtons;
    [SerializeField] private GameObject audioSlider;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject backToMenu;
    [SerializeField] private GameObject cityInfo;


    public void OpenSettings()
    {
        
        setting.SetActive(true);
        mainMenu.SetActive(false);
        GetComponent<Image>().enabled = false;
        if(!SceneManager.GetSceneByName("MenuScene").isLoaded)
            backToMenu.SetActive(true);
        

    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuScene");
        setting.SetActive(false);
        mainMenu.SetActive(true);
        GetComponent<Image>().enabled = true;
        cityInfo.SetActive(false);
        backToMenu.SetActive(false);
        GameManager.GM().load = false;
    }

    public void Back()
    {
        if (SceneManager.GetSceneByName("MenuScene").isLoaded)
        {
            mainMenu.SetActive(true);
        }
        
        setting.SetActive(false);
        audioSlider.SetActive(false);
        settingButtons.SetActive(true);
        backToMenu.SetActive(false);
        GetComponent<Image>().enabled = true;
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void AudioSlider()
    {
        settingButtons.SetActive(false);
        audioSlider.SetActive(true);
        
    }
    
    public void StartGame()
    {
        if(SystemInfo.deviceType == DeviceType.Handheld)
            SceneManager.LoadScene("BlankAR");
        else
        {
            SceneManager.LoadScene("New Scene");
        }
        
        cityInfo.SetActive(true);
        mainMenu.SetActive(false);
    }
    
    public void LoadGame()
    {
        var data = Save.loadData();
        if (data != null)
        {
            if(SystemInfo.deviceType == DeviceType.Handheld)
                SceneManager.LoadScene("BlankAR");
            else
            {
                SceneManager.LoadScene("New Scene");
            }
        }
        else
        {
            Debug.Log("Nessun salvataggio");
        }
        cityInfo.SetActive(true);
        mainMenu.SetActive(false);
    }
}
