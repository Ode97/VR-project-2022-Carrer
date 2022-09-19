using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation.Samples;

public class Build : MonoBehaviour, IPointerDownHandler
{
    private GameManager gameManager;
    public int x, y;

    private void Start()
    { 
        gameManager = GameManager.GM();
    }

    private int GetX()
    {
        return x;
    }
    
    private int GetY()
    {
        return y;
    }
    

    public void OnPointerDown(PointerEventData eventData)
    {
        if(gameObject.layer != 0)
            gameManager.OpenDismantle();
        
        gameManager.OpenMenu();
        //this.GetComponentInParent<GraphBuilder>().gameObject.SetActive(false);
        gameManager.SetCell(gameObject);
    }
}
