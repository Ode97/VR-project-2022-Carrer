using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using UnityEngine;

public class People : MonoBehaviour
{
    private int x;

    private int y;

    private Job job;
    private House house;

    private bool busy = false;

    private int i = 0;

    private Edge[] path;

    private Vector3 target;

    public List<Building> _buildings = new List<Building>();

    private GameObject current;
    
    public bool jobFound = false;
    private bool eat = false;
    private bool work = false;
    private bool stillWorking = false;
    private bool justEat = false;

    private List<MeshRenderer> _meshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        _meshRenderer = gameObject.GetComponentsInChildren<MeshRenderer>().ToList();
        StartMove(house.x, house.y);
    }

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

                if (current)
                {
                    if (eat)
                    {
                        if (!justEat)
                        {
                            current.GetComponent<Food>().Eat();
                            justEat = true;
                        }

                        //justEat = true;
                    }
                    else if (work)
                    {
                        if (!stillWorking)
                        {
                            StartCoroutine(Produce());
                        }
                    }
                }

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
            if (!stop && Vector3.Distance(path[i].to.sceneObject.transform.position, transform.position) < 0.025f)
            {
                if(i < path.Length)
                    i++;
        
            }

            if (stop)
            {
                var toX = x;
                var toY = y;
                
                if (_buildings.Count > 1)
                {
                    Building building = _buildings[0];
                    if (DayManager.D.dayTime == DayTime.Morning)
                    {
                        if (jobFound)
                        {
                            work = true;
                            building = job;
                        }
                    }
                    else if (!eat && DayManager.D.dayTime == DayTime.Afternoon)
                    {
                        work = false;
                        building = _buildings.Find(b => b.GetType() == typeof(Food));
                        eat = true;
                    }
                    else if (DayManager.D.dayTime == DayTime.Evening || DayManager.D.dayTime == DayTime.Afternoon)
                    {
                        eat = false;
                        var r = Random.Range(0, _buildings.Count);
                        building = _buildings[r];
                    }
                    else if (DayManager.D.dayTime == DayTime.Night)
                    {
                        justEat = false;
                        building = house;
                    }

                    if (building)
                    {
                        current = building.gameObject;
                        toX = building.x;
                        toY = building.y;
                    }
                    else 
                    {
                        toX = x;
                        toY = y;
                    }
                    
                }
                StartCoroutine(Move(toX, toY));
            }
        }
    }

    private IEnumerator Produce()
    {
        stillWorking = true;
        yield return new WaitForSeconds(2);
        job.Produce();
        stillWorking = false;
    }

    public void SetHouse(House h)
    {
        house = h;
        _buildings.Add(h);
    }
    
    public void SetJob(Job j)
    {
        job = j;
        _buildings.Add(j);
    }

    public IEnumerator Move(int toX, int toY)
    {
        path = GameManager.GM().PathSolver(x, y, toX, toY);
        
        foreach (var mesh in _meshRenderer)
        {
            mesh.enabled = false;
        }
        
        yield return new WaitForSeconds(1);
        
        if(path.Length > 0)
            foreach (var mesh in _meshRenderer)
            {
                mesh.enabled = true;
            }
        
        i = 0;
        busy = true;
    }

    public void StartMove(int fromX, int fromY)
    {
        x = fromX;
        y = fromY;
        foreach (var mesh in _meshRenderer)
        {
            mesh.enabled = false;
        }
        
        path = new Edge[0];
        busy = true;
        
    }

    public void RemoveBuilding(Building b)
    {
        _buildings.Remove(b);
    }

    private void GetSteering()
    {
        var transform1 = transform;
        var pos = transform1.position;
        var rot = transform1.rotation;
        transform.position = Vector3.MoveTowards(pos, target, Time.deltaTime * 0.1f);
        var r = target - pos;
        if(r.magnitude != 0)
            transform.rotation = Quaternion.Lerp(rot, Quaternion.LookRotation(r, Vector3.up), 0.2f);
    }
}
