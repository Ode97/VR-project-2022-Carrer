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
    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject tutorial;


    public void OpenSettings()
    {
        setting.SetActive(true);
        mainMenu.SetActive(false);
        GetComponent<Image>().enabled = false;
        if(!SceneManager.GetSceneByName("MenuScene").isLoaded)
            backToMenu.SetActive(true);
        
        GameManager.GM().audioManager.Move();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuScene");
        setting.SetActive(false);
        cityInfo.SetActive(false);
        mainMenu.SetActive(true);
        GetComponent<Image>().enabled = true;
        backToMenu.SetActive(false);
        GameManager.GM().endPanel.SetActive(false);
        GameManager.GM().load = false;
        GameManager.GM().Reset();
        Time.timeScale = 1;
        GameManager.GM().audioManager.Move();
        GameManager.GM().audioManager.PlaySoundtrack();
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
        credits.SetActive(false);
        GetComponent<Image>().enabled = true;
        GameManager.GM().audioManager.Move();
    }

    public void Credits()
    {
        settingButtons.SetActive(false);
        credits.SetActive(true);
        GetComponent<Image>().enabled = false;
        GameManager.GM().audioManager.Move();
    }

    public void Exit()
    {
        Save.SaveSeenTutorial(GameManager.GM().start);
        Application.Quit();
    }

    public void AudioSlider()
    {
        settingButtons.SetActive(false);
        audioSlider.SetActive(true);
        GameManager.GM().audioManager.Move();
    }
    
    public void StartGame()
    {
        GameManager.GM().data = new Data();
        
        if(SystemInfo.deviceType == DeviceType.Handheld)
            SceneManager.LoadScene("BlankAR");
        else
        {
            SceneManager.LoadScene("New Scene");
        }
        
        GameManager.GM().SetNewGame();
        cityInfo.SetActive(true);
        mainMenu.SetActive(false);
        GameManager.GM().audioManager.Confirm();
        if(!GameManager.GM().start)
            tutorial.SetActive(true);
        
        GameManager.GM().audioManager.StopSoundtrack();
    }
    
    public void LoadGame()
    {
        GameManager.GM().data = new Data();
        var data = Save.loadData();
        if (data != null)
        {
            if(SystemInfo.deviceType == DeviceType.Handheld)
                SceneManager.LoadScene("BlankAR");
            else
            {
                SceneManager.LoadScene("New Scene");
            }
            cityInfo.SetActive(true);
            mainMenu.SetActive(false);
            GameManager.GM().audioManager.Confirm();
            GameManager.GM().audioManager.StopSoundtrack();
        }
        else
        {
            StartCoroutine(GameManager.GM().WarningText("Nessun Salvataggio"));
        }
        
        
    }
}
