using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Worker : MonoBehaviour
{
    private int layer;
    private GameObject building;
    private Edge[] path;
    private bool busy = false;
    public int x = 0;
    public int y = 0;
    private int i = 0;
    private Vector2 gridTarget;
    private Vector3 target;
    private float orientation;
    private int time;
    public bool tree;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (busy)
        {
            var stop = false;
            
            if (i == path.Length)
            {
                if (path.Length != 0)
                {
                    var actualPos = path[i - 1].to.sceneObject.GetComponent<Build>();
                    x = actualPos.x;
                    y = actualPos.y;
                }

                GameManager.GM().Do(layer, time);
                busy = false;
                stop = true;
            }

            if (i < path.Length)
            {
                var g = path[i].to.sceneObject.transform.position;
                target = new Vector3(g.x, g.y + 0.02f, g.z);
                GetSteering();
            }
            
            //Debug.Log(path.Length + " " + i + " " + Vector3.Distance(path[i].to.sceneObject.transform.position, transform.position));
            if (!stop && Vector3.Distance(path[i].to.sceneObject.transform.position, transform.position) < 0.03f)
            {
                if(i < path.Length)
                    i++;
        
            }
        }
    }

    public void SetTarget(Vector2 g)
    {
        gridTarget = g;
    }
    
    public Vector2 GetTarget()
    {
        return gridTarget;
    }

    public void Walk(Edge[] p)
    {
        i = 0;
        path = p;
        busy = true;
    }

    private void GetSteering()
    {
        var transform1 = transform;
        var pos = transform1.position;
        var rot = transform1.rotation;
        transform.position = Vector3.MoveTowards(pos, target, Time.deltaTime * 0.1f);
        transform.rotation = Quaternion.Lerp(rot, Quaternion.LookRotation(target - pos, Vector3.up), 0.2f);
    }

    public void SetInfo(int l, int workTime, GameObject b)
    {
        time = workTime;
        building = b;
        layer = l;
        
    }
    
    public void SetInfo(int l, int workTime)
    {
        time = workTime;
        layer = l;
    }

    public GameObject GetBuilding()
    {
        return building;
    }
}
