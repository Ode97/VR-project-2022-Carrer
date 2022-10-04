using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameManager _gameManager;
    public void StartGame()
    {
        if(SystemInfo.deviceType == DeviceType.Handheld)
            SceneManager.LoadScene("BlankAR");
        else
        {
            SceneManager.LoadScene("New Scene");
        }
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
    }
}
