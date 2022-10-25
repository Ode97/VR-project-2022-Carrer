using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Gridprova : MonoBehaviour
{
    public GameObject grid;
    
    // Update is called once per frame
    public void StartGame()
    {
        
        var a = Instantiate(grid);
        a.transform.position = new Vector3(0, 0, 0);
        grid.GetComponent<GraphBuilder>().Create(a);

        if (!GameManager.GM().load)
        {
            if (!GameManager.GM().start)
            {
                var t = FindObjectOfType<Tutorial>();
                t.enabled = true;
                GameManager.GM().start = true;
            }else
                FindObjectOfType<DayManager>().StartTime();
        }

    }
}

