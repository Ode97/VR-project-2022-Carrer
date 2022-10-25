using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{

    // Update is called once per frame
    public void OnPointerDown(PointerEventData eventData)
    {
        if (DeviceType.Handheld == SystemInfo.deviceType)
        {
            if (Input.touchCount > 0)
            {
                FindObjectOfType<Settings>().BackToMenu();
                SceneManager.LoadScene("MenuScene");
            }
        }
        else
        {
            FindObjectOfType<Settings>().BackToMenu();
            SceneManager.LoadScene("MenuScene");
        }
    }
}
