using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public int woodNeed;
    public int x;
    public int y;
    
    public bool CheckWood()
    {
        if (GameManager.GM().wood - woodNeed >= 0)
        {
            Debug.Log(GameManager.GM().wood + " " + woodNeed);
            return true;
        }
        else
        {

            return false;
        }
    }
}
