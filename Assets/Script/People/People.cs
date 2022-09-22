using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class People : MonoBehaviour
{
    private int x;

    private int y;

    private House house;

    private bool busy = false;

    private int i = 0;

    private Edge[] path;

    private Vector3 target;

    public List<Building> _buildings = new List<Building>();
    
    public bool jobFound = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
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

                Debug.Log(_buildings.Count);
                if (_buildings.Count > 1)
                {
                    var b = _buildings[Random.Range(0, _buildings.Count)];
                    toX = b.x;
                    toY = b.y;
                }
                StartCoroutine(Move(toX, toY));
            }
        }
    }

    public void SetHouse(House h)
    {
        house = h;
        _buildings.Add(h);
    }

    public IEnumerator Move(int toX, int toY)
    {
        path = GameManager.GM().PathSolver(x, y, toX, toY);
        yield return new WaitForSeconds(5);
        i = 0;
        busy = true;
    }

    public void StartMove(int toX, int toY)
    {
        path = GameManager.GM().PathSolver(x, y, toX, toY);
        busy = true;
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
