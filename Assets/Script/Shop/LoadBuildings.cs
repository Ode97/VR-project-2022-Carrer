using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private TextMeshProUGUI text;
    private string building;
    private Transform actual = null;
    private int layer;
    private ARSessionOrigin _arSessionOrigin;
    private int i = 0;


    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        
        for (int i = 0; i < itemHouses.childCount; i++)
        {
            itemHouses.GetChild(i).GetComponent<Building>().SetI(i);
        }
        for (int i = 0; i < itemEntertainments.childCount; i++)
        {
            itemEntertainments.GetChild(i).GetComponent<Building>().SetI(i);
        }
        
        for (int i = 0; i < itemJobs.childCount; i++)
        {
            itemJobs.GetChild(i).GetComponent<Building>().SetI(i);
        }
        
    }

    public void SetARSession(ARSessionOrigin a)
    {
        _arSessionOrigin = a;
    }

    public void LoadHouses()
    {
        
        if (actual)
        {
            Destroy(actual.gameObject);
        }

        i = 0;
        item = itemHouses;
        building = "Houses ";
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
        building = "Entertainments ";
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
        building = "Jobs ";
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
        text.text = building + (i + 1) + "/" + item.childCount;
        actual = item.GetChild(i);
        if (SystemInfo.deviceType == DeviceType.Handheld){
            actual = Instantiate(item.GetChild(i), _arSessionOrigin.camera.transform);
        }else{
            actual = Instantiate(item.GetChild(i), Camera.main.transform);
        }
        //actual.SetParent(_arSessionOrigin.camera.transform);
        //actual.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        //actual.transform.localScale.Set(0.1f, 0.1f, 0.1f);
        //actual.transform.rotation = Quaternion.identity;
        actual.localPosition = new Vector3(0, 0, 20);
        actual.gameObject.SetActive(true);
    }
}