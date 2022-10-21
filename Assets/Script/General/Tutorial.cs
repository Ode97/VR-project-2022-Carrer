using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject[] scenes;
    private int i = 0;
    private bool placed = false;


    void Update()
    {
        GetComponent<Image>().enabled = true;
        scenes[i].SetActive(true);
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0))
        {
            scenes[i].SetActive(false);
            if (i == 0 && !placed)
            {
                placed = true;
                GetComponent<Image>().enabled = false;
                i++;
                enabled = false;
                return;
            }

            i++;
            if (i < scenes.Length)
                scenes[i].SetActive(true);
            else
            {
                i = 0;
                //scenes[i].SetActive(true);
                placed = false;
                GetComponent<Image>().enabled = false;
                FindObjectOfType<DayManager>().StartTime();
                gameObject.SetActive(false);
            }
        }
    }
}
