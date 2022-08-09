using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuType {House, Entertainment, Job, Next, Select}
public class ButtonDown : MonoBehaviour
{
    private LoadBuildings loadBuildings;

    public MenuType menuType;
    // Start is called before the first frame update
    private void Start()
    {
        loadBuildings = GetComponentInParent<LoadBuildings>();
    }

    private void OnMouseDown()
    {
        switch (menuType)
        {
            case MenuType.House:
                loadBuildings.LoadHouses();
                break;
            case MenuType.Entertainment:
                loadBuildings.LoadEntertainments();
                break;
            case MenuType.Job:
                loadBuildings.LoadJobs();
                break;
            case MenuType.Next:
                loadBuildings.Next();
                break;
            case MenuType.Select:
                loadBuildings.Select();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
