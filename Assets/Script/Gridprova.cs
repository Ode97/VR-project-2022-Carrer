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
            var t = FindObjectOfType<Tutorial>();
            t.enabled = true;
            t.GetComponent<Image>().enabled = true;
        }

    }
}

