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
    private Transform actual = null;
    private int layer;
    private Canvas canvas;
    private ARSessionOrigin _arSessionOrigin;
    private int i = 0;


    private void Start()
    {
        canvas = GetComponent<Canvas>();
        _arSessionOrigin = FindObjectOfType<ARSessionOrigin>();
    }

    public void LoadHouses()
    {
        if (actual)
        {
            Destroy(actual.gameObject);
        }

        i = 0;
        item = itemHouses;
        layer = Constant.houseLayer;
        ShowObject();
    }
    
    public void LoadEntertainments()
    {
        if (actual)
        {
            Destroy(actual.gameObject);
        }

        i = 0;
        item = itemEntertainments;
        layer = Constant.entertainmentLayer;
        ShowObject();
    }
    
    public void LoadJobs()
    {
        if (actual)
        {
            Destroy(actual.gameObject);
        }

        i = 0;
        item = itemJobs;
        layer = Constant.jobLayer;
        ShowObject();
    }

    public void Next()
    {
        Destroy(actual.gameObject);
        i++;
        if (i == item.childCount)
            i = 0;
        ShowObject();
    }

    public void Select()
    {
        if (actual.GetComponent<Building>().CheckWood())
        {
            Destroy(actual.gameObject);
            GameManager.GM().SetBuilding(item.GetChild(i).gameObject, layer);
            GameManager.GM().CloseShop();
        }
        else
        {
            Debug.Log("ti serve più legna");
            GameManager.GM().CloseShop();
        }
    }

    private void ShowObject()
    {
        actual = item.GetChild(i);
        if (SystemInfo.deviceType == DeviceType.Handheld){
            actual = Instantiate(item.GetChild(i), _arSessionOrigin.camera.transform);
        }else{
            actual = Instantiate(item.GetChild(i));
        }
        //actual.SetParent(_arSessionOrigin.camera.transform);
        //actual.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        //actual.transform.localScale.Set(0.1f, 0.1f, 0.1f);
        //actual.transform.rotation = Quaternion.identity;
        actual.localPosition = new Vector3(0, 0, 20);
        actual.gameObject.SetActive(true);
    }
}