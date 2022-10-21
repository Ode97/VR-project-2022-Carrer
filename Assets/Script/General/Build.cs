using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.XR.ARFoundation.Samples;

public class Build : MonoBehaviour, IPointerDownHandler
{
    private GameManager gameManager;
    public int x, y;

    private void Start()
    { 
        gameManager = GameManager.GM();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (DeviceType.Handheld == SystemInfo.deviceType)
        {
            if (Input.touchCount == 1)
                gameManager.SetCell(gameObject);
            else if(Input.touchCount >= 2 )
                GameManager.GM().CloseMenu();
        }
        else
        {
            gameManager.SetCell(gameObject);
        }
        //this.GetComponentInParent<GraphBuilder>().gameObject.SetActive(false);
    }

    public void SetActive()
    {
        enabled = true;
    }
    
    public void SetInactive()
    {
        enabled = false;
    }
}
