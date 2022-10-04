using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gridprova : MonoBehaviour
{
    public GameObject grid;
    
    // Update is called once per frame
    public void StartGame()
    {
        
        var a = Instantiate(grid);
        a.transform.position = new Vector3(0, 0, 0);
        grid.GetComponent<GraphBuilder>().Create();
        
    }
}
