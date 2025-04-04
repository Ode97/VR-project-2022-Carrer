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
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI woodText;
    [SerializeField] private TextMeshProUGUI peopleText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI description;
    private string building;
    private Transform actual = null;
    private int layer;
    private ARSessionOrigin _arSessionOrigin;
    private int i = 0;
    private bool house;
    private bool ent;
    private bool job;

    void Awake()
    {
        if (GameManager.GM())
            Destroy (gameObject);

        DontDestroyOnLoad (gameObject);
    }

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

        nameText.text = "";
        text.text = "";
        woodText.text = "";
        peopleText.text = "";
        timeText.text = "";
        description.text = "";

    }

    public void SetARSession(ARSessionOrigin a)
    {
        _arSessionOrigin = a;
    }

    public void LoadHouses()
    {
        house = true;
        job = false;
        ent = false;
        
        if (actual)
        {
            Destroy(actual.gameObject);
        }

        i = 0;
        item = itemHouses;
        peopleText.text = "People: " + item.GetChild(i).GetComponent<House>().people.ToString();
        building = "Houses "; 
        layer = Constant.houseLayer;
        ShowObject();
    }
    
    public void LoadEntertainments()
    {
        house = false;
        job = false;
        ent = true;
        
        if (actual)
        {
            Destroy(actual.gameObject);
        }

        i = 0;
        item = itemEntertainments;
        peopleText.text = "People\nEntertained: " + item.GetChild(i).GetComponent<Entertainment>().peopleEntertained.ToString();
        building = "Entertainments ";
        layer = Constant.entertainmentLayer;
        ShowObject();
    }
    
    public void LoadJobs()
    {
        house = false;
        job = true;
        ent = false;
        
        
        if (actual)
        {
            Destroy(actual.gameObject);
        }

        i = 0;
        item = itemJobs;
        peopleText.text = "Jobs: " + item.GetChild(i).GetComponent<Job>().jobsNum.ToString();
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

        if(house)
            peopleText.text = "People: " + item.GetChild(i).GetComponent<House>().people.ToString();
        else if(ent)
            peopleText.text = "People\nEntertained: " + item.GetChild(i).GetComponent<Entertainment>().peopleEntertained.ToString();
        else if(job)
            peopleText.text = "Jobs: " + item.GetChild(i).GetComponent<Job>().jobsNum.ToString();
        
        ShowObject();
    }

    public void Previous()
    {
        Destroy(actual.gameObject);
        i--;
        if (i == -1)
            i = item.childCount - 1;

        if(house)
            peopleText.text = "People: " + item.GetChild(i).GetComponent<House>().people.ToString();
        else if(ent)
            peopleText.text = "People\nEntertained: " + item.GetChild(i).GetComponent<Entertainment>().peopleEntertained.ToString();
        else if(job)
            peopleText.text = "Jobs: " + item.GetChild(i).GetComponent<Job>().jobsNum.ToString();
        
        ShowObject();
    }
    
    public void Select()
    {
        if(!actual)
            return;
        
        if (actual.GetComponent<Building>().CheckWood())
        {
            GameManager.GM().SetBuilding(item.GetChild(i).gameObject, layer);
            GameManager.GM().CloseShop();
        }
        else
        {
            StartCoroutine(GameManager.GM().WarningText("You need more woods"));
            GameManager.GM().CloseShop();
        }

        Destroy(actual.gameObject);
        Reset();
    }

    public void Close()
    {
        if(actual)
            Destroy(actual.gameObject);
        
        Reset();
        GameManager.GM().CloseShop();
    }

    private void Reset()
    {
        woodText.text = "";
        peopleText.text = "";
        timeText.text = "";
        text.text = "";
        nameText.text = "";
        description.text = "";
    }

    private void ShowObject()
    {
        var b = item.GetChild(i).GetComponent<Building>();
        description.text = b.description;
        nameText.text = b.name;
        woodText.text = "Wood: " + b.woodNeed.ToString();
        text.text = building + (i + 1) + "/" + item.childCount;
        timeText.text = "Time: " + b.constructionTimeInHour + "h";
        //actual = item.GetChild(i);
        if (SystemInfo.deviceType == DeviceType.Handheld){
            actual = Instantiate(b.transform, _arSessionOrigin.camera.transform);
        }else{
            actual = Instantiate(b.transform, Camera.main.transform);
        }
        
        //actual.localPosition = new Vector3(0, -3, 20);
        actual.localPosition = new Vector3(0, -0.1f, 0.7f);
        actual.localRotation = new Quaternion(0, 180, 0, 0);
        actual.gameObject.SetActive(true);
    }
}