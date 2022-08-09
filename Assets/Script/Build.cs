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

    private void Start()
    { 
        gameManager = GameManager.GM();
    }
    

    public void OnPointerDown(PointerEventData eventData)
    {
        gameManager.OpenMenu();
        //this.GetComponentInParent<GraphBuilder>().gameObject.SetActive(false);
        gameManager.SetCell(gameObject);
    }
}
