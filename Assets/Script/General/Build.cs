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

    public void OnPointerDown(PointerEventData eventData)
    {
        gameManager.SetCell(gameObject);
        
        if(gameObject.layer == Constant.treeLayer)
            gameManager.CutTree();
        else if(gameObject.layer != 0)
            gameManager.OpenDismantle();
        else
        {
            gameManager.OpenMenu();
        }
        
        //this.GetComponentInParent<GraphBuilder>().gameObject.SetActive(false);
    }
}
