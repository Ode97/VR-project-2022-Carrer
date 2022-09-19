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
    private Vector3 target;
    private float orientation;
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
                GameManager.GM().Build(building, layer);
                busy = false;
                stop = true;
            }

            if (i < path.Length)
            {
                var g = path[i].to.sceneObject.transform.position;
                target = new Vector3(g.x, g.y + 0.005f, g.z);
                GetSteering();
            }
            //GetComponent<Rigidbody>().velocity = goal - transform.position;
            
            //Debug.Log(path.Length + " " + i + " " + Vector3.Distance(path[i].to.sceneObject.transform.position, transform.position));
            if (!stop && Vector3.Distance(path[i].to.sceneObject.transform.position, transform.position) < 0.03f)
            {
                if(i < path.Length)
                    i++;
        
            }
        }
    }

    public void Walk(GameObject b, int l, Edge[] p)
    {
        building = b;
        layer = l;
        busy = true;
        path = p;
    }

    private void GetSteering()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * 0.1f);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target - transform.position, Vector3.up), 0.2f);

    }
}
