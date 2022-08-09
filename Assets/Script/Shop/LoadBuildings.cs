using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using Object = UnityEngine.Object;

public class LoadBuildings : MonoBehaviour
{
    public Transform itemHouses;
    public Transform itemEntertainments;
    public Transform itemJobs;
    private Transform item;

    private Transform grid;
    private Transform actual = null;
    //public LoadBuildings next;
    private Canvas canvas;
    private ARSessionOrigin _arSessionOrigin;
    private Transform initPos;

    private int i = 0;


    private void Start()
    {
        canvas = GetComponent<Canvas>();
        _arSessionOrigin = FindObjectOfType<ARSessionOrigin>();
        initPos = _arSessionOrigin.transform;
    }

    public void SetGrid(Transform g)
    {
        grid = g;
    }

    public void LoadHouses()
    {
        if(actual)
            actual.gameObject.SetActive(false);
        i = 0;
        item = itemHouses;
        ShowObject();
    }
    
    public void LoadEntertainments()
    {
        if(actual)
            actual.gameObject.SetActive(false);
        i = 0;
        item = itemEntertainments;
        ShowObject();
    }
    
    public void LoadJobs()
    {
        if(actual)
            actual.gameObject.SetActive(false);
        i = 0;
        item = itemJobs;
        ShowObject();
    }

    public void Next()
    {
        actual.gameObject.SetActive(false);
        i++;
        if (i == item.childCount)
            i = 0;
        ShowObject();
    }

    public void Select()
    {
        GameManager.GM().SetBuilding(item.GetChild(i).gameObject);
        GameManager.GM().CloseMenu();
    }

    private void ShowObject()
    {
        //item.GetChild(i).gameObject.transform.position = new Vector3(GameManager.GM().mainCamera.transform.position.x, GameManager.GM().mainCamera.transform.position.y, GameManager.GM().mainCamera.transform.position.z + 150);
        
        actual = item.GetChild(i);
        actual.gameObject.SetActive(true);
        var pos = _arSessionOrigin.transform.position;
        actual.position = new Vector3(pos.x, pos.y - 0.5f, pos.z + 20);
        //_arSessionOrigin.MakeContentAppearAt(actual, new Vector3(pos.x, pos.y - 0.5f, pos.z + 20));
    }

    private void Update()
    {
        //throw new NotImplementedException();
    }
}
