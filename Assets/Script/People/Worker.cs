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
    public int x = 0;
    public int y = 0;
    private int i = 0;
    public GameObject constructionCell;
    private Vector2 gridTarget;
    private Vector3 target;
    private float orientation;
    private int time;
    public bool tree;
    private bool stop = true;
    private SliderController _sliderController;
    private GameObject sliderPrefab;
    private bool work = false;
    void Start()
    {
        sliderPrefab = Resources.Load<GameObject>("ScenePrefab/SliderCanvas");
    }

    // Update is called once per frame
    void Update()
    {
        if (!stop)
        {
            //var stop = false;
            if (i == path.Length)
            {
                if (path.Length != 0)
                {
                    var actualPos = path[i - 1].to.sceneObject.GetComponent<Build>();
                    x = actualPos.x;
                    y = actualPos.y;
                }

                Do();
                
                //busy = false;
                stop = true;
            }

            if (i < path.Length)
            {
                var g = path[i].to.sceneObject.transform.position;
                target = new Vector3(g.x, g.y + 0.02f, g.z);
                GetSteering();
            }
            
            if (!stop && Vector3.Distance(path[i].to.sceneObject.transform.position, transform.position) < 0.025f)
            {
                if(i < path.Length)
                    i++;
        
            }
                
        }

        if (work && _sliderController.gameObject.activeSelf)
        {
            if(SystemInfo.deviceType != DeviceType.Handheld)
                _sliderController.transform.rotation = Quaternion.LookRotation(_sliderController.transform.position - Camera.main.transform.position, Vector3.up);
            else
                _sliderController.transform.rotation = Quaternion.LookRotation(_sliderController.transform.position - GameManager.GM()._arSessionOrigin.camera.transform.position, Vector3.up);
            
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
        stop = false;
    }

    private void GetSteering()
    {
        var transform1 = transform;
        var pos = transform1.position;
        var rot = transform1.rotation;

        /*RaycastHit hit;
        if(Physics.Raycast(transform1.position, Vector3.forward, out hit, 0.025f, 1 << 11)){
       
            var t = hit.point + hit.normal * 0.1f;
            target = new Vector3(t.x, transform1.position.y, t.z);
        }*/
        
        transform.position = Vector3.MoveTowards(pos, target, Time.deltaTime * 0.1f);
        var r = target - pos;
        if(r.magnitude != 0)
            transform.rotation = Quaternion.Lerp(rot, Quaternion.LookRotation(r, Vector3.up), 0.2f);
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
    
    public void Do()
    {
        _sliderController = Instantiate(sliderPrefab.GetComponent<SliderController>());
        
        _sliderController.Reset();
        _sliderController.gameObject.SetActive(true);
        var pos = constructionCell.transform.position;
        _sliderController.transform.position = new Vector3(pos.x, pos.y + 0.1f, pos.z);
        _sliderController.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
        _sliderController.slider.maxValue = time;

        var rot = constructionCell.transform.position - transform.position; 
        transform.rotation = Quaternion.LookRotation(rot, Vector3.up);

        
        StartCoroutine(Working());
    }
    
    private IEnumerator Working()
    {
        work = true;
        for(var i = 0; i < time; i++)
        {
            yield return new WaitForSeconds(DayManager.D.gameHourInSeconds);
            _sliderController.UpdateProgress();
        }

        work = false;

        if(layer == Constant.streetLayer)
            GameManager.GM().CreateStreet(this);
        else if (layer == Constant.treeLayer)
        {
            GameManager.GM().DestroyTree(this);
            //tree = false;
        }else if (layer == 0)
        {
            GameManager.GM().Dismantle(this);
        }
        else
        {
            GameManager.GM().CreateBuilding(this, layer);
        }
        
        yield return new WaitForSeconds(0.2f);
        
        Destroy(_sliderController.gameObject);
        GameManager.GM().EndWork(this);
        
        
    }
}
