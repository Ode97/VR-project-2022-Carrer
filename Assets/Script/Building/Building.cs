using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    
    public int woodNeed;
    public int x;
    public int y;
    private int i;
    private int rotation;
    public List<People> _peoples = new List<People>();
    
    public bool CheckWood()
    {
        if (GameManager.GM().wood - woodNeed >= 0)
        {
            return true;
        }
        else
        {

            return false;
        }
    }

    public void Dismantle()
    {
        foreach (var p in _peoples)
        {
            p.RemoveBuilding(this);   
        }
    }

    public void SetI(int n)
    {
        i = n;
    }

    public void SetRotation(int r)
    {
        rotation = r;
    }

    public int GetRotation()
    {
        return rotation;
    }

    public int GetI()
    {
        return i;
    }
}
