using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using Debug = UnityEngine.Debug;

[System.Serializable]
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject buildingMenu;
    [SerializeField] private GameObject buttonsMenu;
    [SerializeField] private GameObject dismantle;
    private static GameManager gm;
    public AudioManager audioManager;

    private GameObject selectedCell;
    public GraphBuilder _graphBuilder;
    [SerializeField]
    private Material streetMat;
    public GameObject workerPrefab;
    public ARSessionOrigin _arSessionOrigin;

    private Queue<Worker> worker = new Queue<Worker>();

    private PathfindingSolver _pathfindingSolver;

    [SerializeField]
    private PeopleManager _peopleManager;

    [SerializeField] private TextMeshProUGUI warningText;
    
    public int wood = 0;
    [SerializeField]
    private TextMeshProUGUI woodText;
    
    public int people = 0;
    [SerializeField]
    private TextMeshProUGUI peopleText;

    public int jobs = 0;
    [SerializeField]
    private TextMeshProUGUI jobText;

    public int entertainment = 0;
    [SerializeField]
    private TextMeshProUGUI entertainmentText;

    private int workerNum = 0;
    [SerializeField]
    private TextMeshProUGUI workerText;
    
    private int food = 0;
    [SerializeField]
    private TextMeshProUGUI foodText;

    [SerializeField] private TextMeshProUGUI happinessText;

    public Data data;
    public bool load = false;
    
    private GameObject[] _houses;
    private GameObject[] _entertainments;
    private GameObject[] _jobs;
    
    // Start is called before the first frame update
    public static GameManager GM()
    {
        return gm;
    }
    
    
    void Start()
    {
        if (gm==null)
            gm=this;
        else if (gm !=this)
            Destroy (gameObject);

        DontDestroyOnLoad (gameObject);
        
        _pathfindingSolver = GetComponent<PathfindingSolver>();
        buildingMenu.gameObject.SetActive(false);
        peopleText.text = "0";
        jobText.text = "0";
        entertainmentText.text = "0";
        woodText.text = wood.ToString();
        workerText.text = "0";
        foodText.text = "0";

        var r = FindObjectOfType<LoadBuildings>();
        
        var x = r.itemHouses.childCount;
        var k = r.itemEntertainments.childCount;
        var j = r.itemJobs.childCount;

        _houses = new GameObject[x];
        _entertainments = new GameObject[k];
        _jobs = new GameObject[j];

        for (int i = 0; i < x; i++)
        {
            _houses[i] = r.itemHouses.GetChild(i).gameObject;
        }
        for (int i = 0; i < k; i++)
        {
            _entertainments[i] = r.itemEntertainments.GetChild(i).gameObject;
        }
        for (int i = 0; i < j; i++)
        {
            _jobs[i] = r.itemJobs.GetChild(i).gameObject;
        }
        
        load = false;
        
    }
    
    public IEnumerator WarningText(string t)
    {
        warningText.text = t;
        warningText.enabled = true;
        audioManager.Error();
        yield return new WaitForSeconds(2);
        warningText.enabled = false;
    }
    
    public void SetHappiness(int h)
    {
        happinessText.text = h.ToString();
    }

    public GameObject[] GetHouses()
    {
        return _houses;
    }
    
    public GameObject[] GetEntertainments()
    {
        return _entertainments;
    }
    
    public GameObject[] GetJobs()
    {
        return _jobs;
    }

    public void SetLoad(int wood, int people, int jobs, int ents, int food)
    {
        this.wood = wood;
        this.people = people;
        this.jobs = jobs;
        this.entertainment = ents;
        this.food = food;
        load = true;
        SetText();

    }
    
    public void SetNewGame()
    {
        this.wood = 0;
        this.people = 0;
        this.jobs = 0;
        this.entertainment = 0;
        this.workerNum = 0;
        this.food = 0;
        load = false;
        SetText();
    }

    public void SetText()
    {
        peopleText.text = people.ToString();
        jobText.text = jobs.ToString();
        entertainmentText.text = entertainment.ToString();
        woodText.text = wood.ToString();
        workerText.text = workerNum.ToString();
        foodText.text = food.ToString();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public Edge[] PathSolver(int fromX, int fromY, int toX, int toY)
    {
        return _pathfindingSolver.Solve(_graphBuilder.g,
            _graphBuilder[fromX, fromY],
            _graphBuilder[toX, toY]);
    }

    public void SetGraphBuilder(GraphBuilder graphBuilder)
    {
        _peopleManager = FindObjectOfType<PeopleManager>();
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            _arSessionOrigin = FindObjectOfType<ARSessionOrigin>();
            FindObjectOfType<LoadBuildings>().SetARSession(_arSessionOrigin);
        }

        _graphBuilder = graphBuilder;
        if(load)
            FindObjectOfType<DayManager>().StartTime();
        //FindObjectOfType<DayManager>().StartTime();
    }
    
    public void OpenMenu()
    {
        buttonsMenu.gameObject.SetActive(true);

    }
    
    public void CloseMenu()
    {
        buttonsMenu.gameObject.SetActive(false);

    }

    public void OpenDismantle()
    {
        dismantle.gameObject.SetActive(true);

        //mainCamera.transform.SetPositionAndRotation(menuTransform.position, menuTransform.rotation);
    }
    
    public void CloseDismantle()
    {
        dismantle.gameObject.SetActive(false);
        //mainCamera.transform.SetPositionAndRotation(initalTransform.position, initalTransform.rotation);
    }
    
    public void CloseShop()
    {
        buildingMenu.gameObject.SetActive(false);
        //mainCamera.transform.SetPositionAndRotation(initalTransform.position, initalTransform.rotation);
    }
    public void OpenShop()
    {
        buildingMenu.gameObject.SetActive(true);
        buttonsMenu.gameObject.SetActive(false);

        //mainCamera.transform.SetPositionAndRotation(menuTransform.position, menuTransform.rotation);
    }

    public PeopleManager GetPeopleManager()
    {
        return _peopleManager;
    }

    public void SetCell(GameObject cell)
    {

        if (dismantle.activeSelf || buttonsMenu.activeSelf || buildingMenu.activeSelf)
        {
            CloseMenu();
            CloseDismantle();
            return;
        }

        if (worker.Count == 0)
        {
            StartCoroutine(WarningText("All workers are busy"));
            return;
        }
        else
        {
            selectedCell = cell;
            if (cell.layer == Constant.treeLayer)
            {
                Worker w = worker.Dequeue();
                CutTree(w);
            }
            else if (cell.layer != 0)
            {
                OpenDismantle();
            }
            else
            {
                OpenMenu();
            }
        }
    }

    public int GetFood()
    {
        return food;
    }

    public void AddFood()
    {
        food++;
    }

    public void Eat()
    {
        food--;
    }

    private bool CheckNearStreet(Worker w)
    {
        bool ok = false;
        
        Edge[] path1 = new Edge[100];
        Edge[] path2 = new Edge[100];
        Edge[] path3 = new Edge[100];
        Edge[] path4 = new Edge[100];
        
        var gridPos = w.constructionCell.GetComponent<Build>();
        if (gridPos.x != 9 && _graphBuilder.matrix[gridPos.x + 1, gridPos.y].sceneObject.layer == Constant.streetLayer){
            w.SetTarget(new Vector2(gridPos.x + 1, gridPos.y));
            path1 = _pathfindingSolver.Solve(_graphBuilder.g,
                _graphBuilder[w.x, w.y],
                _graphBuilder[w.GetTarget().x, w.GetTarget().y]);

            
            
            ok = true;
        }
        if (gridPos.x != 0 && _graphBuilder.matrix[gridPos.x - 1, gridPos.y].sceneObject.layer == Constant.streetLayer){
            w.SetTarget(new Vector2(gridPos.x - 1, gridPos.y));
            path2 = _pathfindingSolver.Solve(_graphBuilder.g,
                _graphBuilder[w.x, w.y],
                _graphBuilder[w.GetTarget().x, w.GetTarget().y]);
            
            
            ok = true;
        }
        if (gridPos.y != 9 && _graphBuilder.matrix[gridPos.x, gridPos.y + 1].sceneObject.layer == Constant.streetLayer){
            w.SetTarget(new Vector2(gridPos.x, gridPos.y + 1));
            path3 = _pathfindingSolver.Solve(_graphBuilder.g,
                _graphBuilder[w.x, w.y],
                _graphBuilder[w.GetTarget().x, w.GetTarget().y]);
          
            
            ok = true;
        }
        if (gridPos.y != 0 && _graphBuilder.matrix[gridPos.x, gridPos.y - 1].sceneObject.layer == Constant.streetLayer){
            w.SetTarget(new Vector2(gridPos.x, gridPos.y - 1));
            path4 = _pathfindingSolver.Solve(_graphBuilder.g,
                _graphBuilder[w.x, w.y],
                _graphBuilder[w.GetTarget().x, w.GetTarget().y]);
            
            
            ok = true;
        }
        
        if (ok)
        {
            var path = path1;
            if (path2.Length < path.Length)
                path = path2;

            if (path3.Length < path.Length)
                path = path3;

            if (path4.Length < path.Length)
                path = path4;

            Debug.Log(path.Length);
            w.Walk(path);
            
            return true;
        }else
            return false;
    }

    public void SetBuilding(GameObject building, int layer)
    {
        Worker w = worker.Dequeue();
        w.constructionCell = selectedCell;
        w.SetInfo(layer, building.GetComponent<Building>().constructionTimeInHour, building);
        MoveWorker(w);
    }

    public void SetStreet()
    {
        Worker w = worker.Dequeue();
        w.constructionCell = selectedCell;
        w.SetInfo(Constant.streetLayer, 1);
        buttonsMenu.SetActive(false);
        MoveWorker(w);
    }

    private void MoveWorker(Worker w)
    {
        if (CheckNearStreet(w))
        {
            
            w.constructionCell.GetComponent<Build>().SetInactive();

            /*var path = _pathfindingSolver.Solve(_graphBuilder.g,
                _graphBuilder[w.x, w.y],
                _graphBuilder[w.GetTarget().x, w.GetTarget().y]);
            
            

            w.Walk(path);*/
            
        }
        else
        {
            StartCoroutine(WarningText("You must build near a street"));
            worker.Enqueue(w);
            buttonsMenu.SetActive(false);
        }
    }

    public void CutTree(Worker w)
    {
        w.SetInfo(0, 1);
        w.constructionCell = selectedCell;
        w.tree = true;
        MoveWorker(w);
    }

    public void DestroyTree(Worker w)
    {
        wood += 5;

        data.wood = wood;
        
        w.constructionCell.layer = 0;
        Destroy(w.constructionCell.transform.GetChild(0).gameObject);
    }

    public void EndWork(Worker w)
    {
        SetText();
        w.constructionCell.GetComponent<Build>().SetActive();
        worker.Enqueue(w);
    }

    public void CreateBuilding(Worker w, int layer)
    {
        
        GameObject g = Instantiate(w.GetBuilding(), w.constructionCell.transform);
        
        audioManager.Construction();
        
        var building = g.GetComponent<Building>();
        
        building.SetI(w.GetBuilding().GetComponent<Building>().GetI());
        
        
        var buildingCell = w.constructionCell.GetComponent<Build>();
        
        w.constructionCell.layer = layer;
        
        var a = new Edge(_graphBuilder.matrix[w.x, w.y],
            _graphBuilder.matrix[buildingCell.x, buildingCell.y]);
        var b = new Edge(_graphBuilder.matrix[buildingCell.x, buildingCell.y],
            _graphBuilder.matrix[w.x, w.y]);
        _graphBuilder.g.AddEdge(a);
        _graphBuilder.g.AddEdge(b);
        
        
        if (layer == Constant.houseLayer)
        {
            var h = g.GetComponent<House>();
            var newP = h.people;
            h.x = buildingCell.x;
            h.y = buildingCell.y;
            people += newP;
            wood -= h.woodNeed;
            jobs += newP;
            entertainment += newP;
            SpawnPeople(newP, h, w.constructionCell.transform.position);
        }
        else if(layer == Constant.jobLayer)
        {
            var j = g.GetComponent<Job>();
            
            j.x = buildingCell.x;
            j.y = buildingCell.y;

            var job = w.GetBuilding().GetComponent<Job>();
            
            jobs -= job.jobsNum;
            wood -= job.woodNeed;
            _peopleManager.AddJobs(j);
            
        }else if (layer == Constant.entertainmentLayer)
        {
            var e = g.GetComponent<Entertainment>();
            
            e.x = buildingCell.x;
            e.y = buildingCell.y;

            var ent = w.GetBuilding().GetComponent<Entertainment>();
            
            entertainment -= ent.peopleEntertained;
            wood -= ent.woodNeed;
            _peopleManager.AddEntertainment(e);
        }

        //g.transform.localScale.Set(0.08f, 0.08f, 0.08f);
        g.transform.localPosition = new Vector3(0f, 0f, 0f);
        var pos = _graphBuilder[w.GetTarget().x, w.GetTarget().y].sceneObject.transform.position;
        var target = w.GetTarget();
        
        
        if(target.x - buildingCell.x == -1)
            building.SetRotation(0);
        else if(target.x - buildingCell.x == 1)
            building.SetRotation(2);
        else if(target.y - buildingCell.y == 1)
            building.SetRotation(1);
        else if(target.y - buildingCell.y == -1)
            building.SetRotation(3);
        
        g.transform.rotation = Quaternion.LookRotation(pos - w.constructionCell.transform.position, Vector3.up);
        g.GetComponent<RotateObject>().enabled = false;
        g.SetActive(true);


        if((people >= 12 && workerNum == 1) || (people >= 32 && workerNum == 2) || (people >= 60 && workerNum == 3)|| (people >= 100 && workerNum == 4))
            SpawnWorker(_graphBuilder[0, 0].sceneObject.transform);

        data.jobs = jobs;
        data.entertainment = entertainment;
        data.people = people;
        data.wood = wood;
        data.workerNum = workerNum;
    }

    public void CreateStreet(Worker w)
    {
        if (w.constructionCell.layer == 0)
        {
            w.constructionCell.GetComponent<MeshRenderer>().material = streetMat;
            w.constructionCell.layer = Constant.streetLayer;
            buttonsMenu.gameObject.SetActive(false);
            var gridPos = w.constructionCell.GetComponent<Build>();
            if (gridPos.x != 9 && _graphBuilder.matrix[gridPos.x + 1, gridPos.y].sceneObject.layer ==
                Constant.streetLayer)
            {
                var a = new Edge(_graphBuilder.matrix[gridPos.x + 1, gridPos.y],
                    _graphBuilder.matrix[gridPos.x, gridPos.y]);
                var b = new Edge(_graphBuilder.matrix[gridPos.x, gridPos.y],
                    _graphBuilder.matrix[gridPos.x + 1, gridPos.y]);
                _graphBuilder.g.AddEdge(a);
                _graphBuilder.g.AddEdge(b);
                data.street[gridPos.y * 10 + gridPos.x, 1] = true;
            }

            if (gridPos.x != 0 && _graphBuilder.matrix[gridPos.x - 1, gridPos.y].sceneObject.layer ==
                Constant.streetLayer)
            {
                var c = new Edge(_graphBuilder.matrix[gridPos.x - 1, gridPos.y],
                    _graphBuilder.matrix[gridPos.x, gridPos.y]);
                var d = new Edge(_graphBuilder.matrix[gridPos.x, gridPos.y],
                    _graphBuilder.matrix[gridPos.x - 1, gridPos.y]);
                _graphBuilder.g.AddEdge(c);
                _graphBuilder.g.AddEdge(d);
                data.street[gridPos.y * 10 + gridPos.x, 3] = true;

            }

            if (gridPos.y != 9 && _graphBuilder.matrix[gridPos.x, gridPos.y + 1].sceneObject.layer ==
                Constant.streetLayer)
            {
                var e = new Edge(_graphBuilder.matrix[gridPos.x, gridPos.y + 1],
                    _graphBuilder.matrix[gridPos.x, gridPos.y]);
                var f = new Edge(_graphBuilder.matrix[gridPos.x, gridPos.y],
                    _graphBuilder.matrix[gridPos.x, gridPos.y + 1]);
                _graphBuilder.g.AddEdge(e);
                _graphBuilder.g.AddEdge(f);
                data.street[gridPos.y * 10 + gridPos.x, 0] = true;
            }

            if (gridPos.y != 0 && _graphBuilder.matrix[gridPos.x, gridPos.y - 1].sceneObject.layer ==
                Constant.streetLayer)
            {
                var g = new Edge(_graphBuilder.matrix[gridPos.x, gridPos.y - 1],
                    _graphBuilder.matrix[gridPos.x, gridPos.y]);
                var h = new Edge(_graphBuilder.matrix[gridPos.x, gridPos.y],
                    _graphBuilder.matrix[gridPos.x, gridPos.y - 1]);
                _graphBuilder.g.AddEdge(g);
                _graphBuilder.g.AddEdge(h);
                data.street[gridPos.y * 10 + gridPos.x, 2] = true;
            }
        }
    }

    public void SpawnPeople(int newP, House h, Vector3 pos)
    {
        _peopleManager.SpawnPeople(newP, h, pos);
    }

    public void SpawnWorker(Transform cell)
    {
        Debug.Log("a");
        if (workerNum < 5)
        {
            var pos = cell.position;
            var w = Instantiate(workerPrefab, _graphBuilder.parent.transform);
            w.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            w.transform.position = new Vector3(pos.x, pos.y + 0.02f, pos.z);
            worker.Enqueue(w.GetComponent<Worker>());
            workerNum++;
            SetText();
        }
    }

    public void StartDismantle()
    {
        Worker w = worker.Dequeue();
        buttonsMenu.gameObject.SetActive(false);
        w.constructionCell = selectedCell;
        //var t = w.constructionCell.transform.GetChild(0).GetComponent<Building>().constructionTime;
        w.SetInfo(0, 2);
        MoveWorker(w);
        dismantle.SetActive(false);
    }

    public void Dismantle(Worker w)
    {
        var c = w.constructionCell;
        if (c.transform.childCount > 0)
        {

            var b = c.transform.GetChild(0);
            if (c.layer == Constant.houseLayer)
            {
                var h = b.GetComponent<House>();
                people -= h.people;
                entertainment -= h.people;
                jobs -= h.people;
                _peopleManager.RemovePeople(h);
            }
            else if (c.layer == Constant.entertainmentLayer)
                entertainment += b.GetComponent<Entertainment>().peopleEntertained;
            else if (c.layer == Constant.jobLayer)
                jobs += b.GetComponent<Job>().jobsNum;

            wood += b.GetComponent<Building>().woodNeed;

            Destroy(b.gameObject);
            audioManager.Explosion();
        }
        else
        {
            
            c.GetComponent<MeshRenderer>().material = _graphBuilder.firstMaterial;
        }

        c.layer = 0;
    }

}
