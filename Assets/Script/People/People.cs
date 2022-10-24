using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using UnityEngine;

public class People : MonoBehaviour
{
    public int type;
    public int x;
    public int y;
    private Job job;
    private House house;
    public bool busy = false;
    public int i = 0;
    private Edge[] path;
    private Vector3 target;
    public List<Building> _buildings = new List<Building>();
    private GameObject current;
    public bool jobFound = false;
    public bool eat = false;
    public bool work = false;
    public bool stillWorking = false;
    public bool justEat = false;
    public bool runAway = false;
    public int happiness = 100;
    private List<MeshRenderer> _meshRenderer;

    private bool onLoad = false;

    public bool endDay = false;
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
                            Debug.Log("a");
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
                
                //Debug.Log(path[i].to.sceneObject.GetComponent<Build>().x + " " + path[i].to.sceneObject.GetComponent<Build>().y);
                
                target = new Vector3(g.x, g.y + 0.01f, g.z);
                
            }

            //Debug.Log(path.Length + " " + i + " " + Vector3.Distance(path[i].to.sceneObject.transform.position, transform.position));
            if (!stop && Vector3.Distance(target, transform.position) < 0.025f)
            {
                if (path.Length > 0)
                {
                    var actualPos = path[i].to.sceneObject.GetComponent<Build>();
                    x = actualPos.x;
                    y = actualPos.y;
                }

                i++;
                /*if(!obstAvoid)
                    i++;
                else
                {
                    obstAvoid = false;
                }*/
            }
            
            GetSteering();

            if (stop)
            {
                /*var toX = x;
                var toY = y;
                
                if (_buildings.Count > 1)
                {
                    Building building = null;
                    if (DayManager.D.dayTime == DayTime.Morning)
                    {
                        Debug.Log("b");
                        endDay = false;
                        if (jobFound)
                        {
                            Debug.Log("c");
                            work = true;
                            building = job;
                        }
                    }
                    else if (!eat && DayManager.D.dayTime == DayTime.Afternoon)
                    {
                        stillWorking = false;
                        work = false;
                        var buildings = _buildings.FindAll(b => b.GetType() == typeof(Food));

                        int l = 0;
                        foreach (var b in buildings)
                        {
                            int lb = GameManager.GM().PathSolver(x, y, b.x, b.y).Length;
                            if (lb > l)
                            {
                                building = b;
                                l = lb;
                            }
                        }
                        
                        if (building)
                        {
                            
                            eat = true;
                        }
                    }
                    else if (DayManager.D.dayTime == DayTime.Evening || DayManager.D.dayTime == DayTime.Afternoon)
                    {
                        eat = false;
                        var r = Random.Range(0, _buildings.Count);
                        building = _buildings[r];
                    }
                    else if (DayManager.D.dayTime == DayTime.Night && !endDay)
                    {
                        endDay = true;
                        if (!justEat)
                            happiness -= 20;
                        else
                        {
                            happiness += 10;
                        }
                        if (!jobFound)
                            happiness -= 15;

                        if (GameManager.GM().entertainment < 0)
                            happiness -= 5 * GameManager.GM().entertainment;

                        if (GameManager.GM().entertainment == 0)
                            happiness += 10;
                        if (GameManager.GM().jobs == 0)
                            happiness += 10;
                        
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
                    
                }*/

                if (happiness > 100)
                    happiness = 100;
                
                
                
                StartCoroutine(Move());
                
            }
        }
    }

    public Vector2 SetDestination()
    {
        Vector2 v = new Vector2();
        
        v.x = x;
        v.y = y;
        
        if (_buildings.Count >= 1)
        {
            Building building = null;
            if (DayManager.D.dayTime == DayTime.Morning)
            {
                endDay = false;
                if (jobFound)
                {
                    work = true;
                    building = job;
                }
            }
            else if (!eat && DayManager.D.dayTime == DayTime.Afternoon)
            {
                stillWorking = false;
                work = false;
                var buildings = _buildings.FindAll(b => b.GetType() == typeof(Food));

                int l = 0;
                foreach (var b in buildings)
                {
                    int lb = GameManager.GM().PathSolver(x, y, b.x, b.y).Length;
                    if (lb > l)
                    {
                        building = b;
                        l = lb;
                    }
                }
                
                if (building)
                {
                    
                    eat = true;
                }
            }
            else if (DayManager.D.dayTime == DayTime.Evening || DayManager.D.dayTime == DayTime.Afternoon)
            {
                eat = false;
                var r = Random.Range(0, _buildings.Count);
                building = _buildings[r];
            }
            else if (DayManager.D.dayTime == DayTime.Night && !endDay)
            {
                endDay = true;
                if (!justEat)
                    happiness -= 20;
                else
                {
                    happiness += 10;
                }
                if (!jobFound)
                    happiness -= 15;

                if (GameManager.GM().entertainment < 0)
                    happiness -= 5 * GameManager.GM().entertainment;

                if (GameManager.GM().entertainment == 0)
                    happiness += 10;
                if (GameManager.GM().jobs == 0)
                    happiness += 10;
                
                justEat = false;
                building = house;
            }

            if (building)
            {
                current = building.gameObject;
                v.x = building.x;
                v.y = building.y;
            }
            else 
            {
                v.x = x;
                v.y = y;
            }
            
        }
        
        return v;
    }

    public GameObject GetCurrentTarget()
    {
        return current;
    }

    public void SetCurrentTarget(GameObject g)
    {
        current = g;
    }
    
    private IEnumerator Produce()
    {
        stillWorking = true;
        yield return new WaitForSeconds(DayManager.D.gameHourInSeconds);
        job.Produce();
        stillWorking = false;
    }

    public void SetHouse(House h)
    {
        house = h;
        _buildings.Add(h);
    }
    
    public House GetHouse()
    {
        return house;
    }
    
    public void SetJob(Job j)
    {
        job = j;
        _buildings.Add(j);
    }
    
    public Job GetJob()
    {
        return job;
    }

    public IEnumerator Move()
    {

        foreach (var mesh in _meshRenderer)
        {
            mesh.enabled = false;
        }

        if(!justEat && !onLoad && !runAway)
            yield return new WaitForSeconds(DayManager.D.gameHourInSeconds);
        else
        {
            runAway = false;
            yield return new WaitForSeconds(0);
        }


        if (!onLoad)
        {
            Vector2 dest = SetDestination();
            
            Debug.Log(gameObject.name + " dest: " + dest.x +" " + dest.y + " pos: " + x + " " + y);

            path = GameManager.GM().PathSolver(x, y, (int) dest.x, (int) dest.y);
        }
        else
        {
            onLoad = false;
            path = GameManager.GM()
                .PathSolver(x, y, current.GetComponent<Building>().x, current.GetComponent<Building>().y);
        }

        //Debug.Log(gameObject.name + " " + current.gameObject.name + " " + path.Length + " pos: " + x + " " + y);
        var pos = current.transform.position;
        var v = new Vector3(pos.x, pos.y + 0.01f, pos.z);
                
        if (path.Length == 0 && Vector3.Distance(transform.position, v) >= 0.15f)
        {
            StartCoroutine(GameManager.GM().WarningText("Some people can't reach destination"));
        }
        else
        {

            if (path.Length > 0)
                foreach (var mesh in _meshRenderer)
                {
                    mesh.enabled = true;
                }

        }

        busy = true;
        
        i = 0;
    }
    

    public void StartMove(int fromX, int fromY)
    {
        if (!GameManager.GM().load)
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
        else
        {
            onLoad = true;
            StartCoroutine(Move());
        }

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

        /*RaycastHit hit;
        if(path.Length > 0 && Physics.Raycast(transform1.position, Vector3.forward, out hit, 0.03f, 1 << 11) && hit.transform != gameObject.transform)
        {
            Vector3 t;
            var p = path[i - 1].to.sceneObject.GetComponent<Build>();
            if(Mathf.Abs(p.x - x) > Mathf.Abs(p.y - y))
                t = hit.point + Vector3.forward * 0.01f;
            else
            {
                t = hit.point + Vector3.right * 0.01f;
            }
            
            target = new Vector3(t.x, pos.y, t.z);

            obstAvoid = true;
            
            Debug.Log(Vector3.Distance(pos, target));
        }
        else
        {
            if (target == Vector3.zero)
                target = new Vector3(pos.x, pos.y, pos.z);
            
        }*/

        if (target == Vector3.zero)
            target = new Vector3(pos.x, pos.y, pos.z);
        
        transform.position = Vector3.MoveTowards(pos, target, Time.deltaTime * 0.1f);
        var r = target - pos;
        if (r.magnitude != 0)
        {
            transform.rotation = Quaternion.Lerp(rot, Quaternion.LookRotation(r, Vector3.up), 0.2f);
        }
    }
}
