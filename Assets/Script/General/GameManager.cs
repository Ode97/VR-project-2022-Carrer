using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject buildingMenu;
    [SerializeField] private GameObject buttonsMenu;
    [SerializeField] private GameObject dismantle;
    [SerializeField] private SliderController _sliderController;
    private static GameManager gm;

    private GameObject selectedCell;
    private GameObject selectedBuilding;
    public GraphBuilder _graphBuilder;
    [SerializeField]
    private Material streetMat;
    public GameObject workerPrefab;
    [SerializeField] private ARSessionOrigin _arSessionOrigin;

    private Worker worker;

    private PathfindingSolver _pathfindingSolver;

    [SerializeField]
    private PeopleManager _peopleManager;

    public int wood = 0;
    [SerializeField]
    private TextMeshProUGUI woodText;
    
    private int people = 0;
    [SerializeField]
    private TextMeshProUGUI peopleText;

    private int jobs = 0;
    [SerializeField]
    private TextMeshProUGUI jobText;

    private int entertainment = 0;
    [SerializeField]
    private TextMeshProUGUI entertainmentText;

    private int workerNum = 0;
    [SerializeField]
    private TextMeshProUGUI workerText;
    
    // Start is called before the first frame update
    public static GameManager GM()
    {
        return gm;
    }
    void Start()
    {
        gm = this;
        _pathfindingSolver = GetComponent<PathfindingSolver>();
        buildingMenu.gameObject.SetActive(false);
        peopleText.text = "0";
        jobText.text = "0";
        entertainmentText.text = "0";
        woodText.text = "0";
        workerText.text = "0";

    }

    // Update is called once per frame
    void Update()
    {
        if (_sliderController.gameObject.activeSelf)
        {
            //Debug.Log("aaaa");
            if(SystemInfo.deviceType != DeviceType.Handheld)
                _sliderController.transform.rotation = Quaternion.LookRotation(_sliderController.transform.position - Camera.main.transform.position, Vector3.up);
            else
                _sliderController.transform.rotation = Quaternion.LookRotation(_sliderController.transform.position - _arSessionOrigin.camera.transform.position, Vector3.up);

        }
    }

    public Edge[] PathSolver(int fromX, int fromY, int toX, int toY)
    {
        return _pathfindingSolver.Solve(_graphBuilder.g,
            _graphBuilder[fromX, fromY],
            _graphBuilder[toX, toY]);
    }

    public void SetGraphBuilder(GraphBuilder graphBuilder)
    {
        _graphBuilder = graphBuilder;
    }
    
    public void OpenMenu()
    {
        buttonsMenu.gameObject.SetActive(true);

        //mainCamera.transform.SetPositionAndRotation(menuTransform.position, menuTransform.rotation);
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
    

    public void SetCell(GameObject cell)
    {
        selectedCell = cell;
    }

    private bool CheckNearStreet()
    {
        var gridPos = selectedCell.GetComponent<Build>();
        var w = worker.GetComponent<Worker>();
        if (gridPos.x != 9 && _graphBuilder.matrix[gridPos.x + 1, gridPos.y].sceneObject.layer == Constant.streetLayer){
            w.SetTarget(new Vector2(gridPos.x + 1, gridPos.y));
            return true;
        }else if (gridPos.x != 0 && _graphBuilder.matrix[gridPos.x - 1, gridPos.y].sceneObject.layer == Constant.streetLayer){
            w.SetTarget(new Vector2(gridPos.x - 1, gridPos.y));
            return true;
        }else if (gridPos.y != 9 && _graphBuilder.matrix[gridPos.x, gridPos.y + 1].sceneObject.layer == Constant.streetLayer){
            w.SetTarget(new Vector2(gridPos.x, gridPos.y + 1));
            return true;
        }else if (gridPos.y != 0 && _graphBuilder.matrix[gridPos.x, gridPos.y - 1].sceneObject.layer == Constant.streetLayer){
            w.SetTarget(new Vector2(gridPos.x, gridPos.y - 1));
            return true;
        }

        return false;
    }

    public void SetBuilding(GameObject building, int layer)
    {
        worker.SetInfo(layer, 1, building);
        MoveWorker();
    }

    public void SetStreet()
    {
        worker.SetInfo(Constant.streetLayer, 1);
        MoveWorker();
    }

    private void MoveWorker()
    {
        if (CheckNearStreet())
        {
            //Debug.Log(worker.GetComponent<Worker>().x + " " + worker.GetComponent<Worker>().y + "   " + goal.x + " " + goal.y);

            worker.Walk(_pathfindingSolver.Solve(_graphBuilder.g,
                    _graphBuilder[worker.x, worker.y],
                    _graphBuilder[worker.GetTarget().x, worker.GetTarget().y]));
        }
        else
        {
            Debug.Log("Devi costruire vicino ad una strada");
            buttonsMenu.SetActive(false);
        }
    }

    public void CutTree()
    {
        worker.SetInfo(0, 1);
        worker.tree = true;
        MoveWorker();
    }

    private void DestroyTree()
    {
        wood += 3;
        woodText.text = wood.ToString();
        selectedCell.layer = 0;
        Destroy(selectedCell.transform.GetChild(0).gameObject);
        worker.tree = false;
    }

    public void Do(int layer, int time)
    {
        _sliderController.gameObject.SetActive(true);
        var pos = selectedCell.transform.position;
        _sliderController.transform.position = new Vector3(pos.x, pos.y + 0.1f, pos.z);
        _sliderController.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
        _sliderController.slider.maxValue = time;

        var rot = selectedCell.transform.position - worker.transform.position; 
        worker.transform.rotation = Quaternion.LookRotation(rot, Vector3.up);

        
        StartCoroutine(Working(time, layer));
    }

    private IEnumerator Working(int t, int layer)
    {
        for(var i = 0; i <= t; i++)
        {
            yield return new WaitForSeconds(1);
            _sliderController.UpdateProgress();
        }
        
        if(layer == Constant.streetLayer)
            CreateStreet();
        else if (worker.tree)
        {
            DestroyTree();
        }else if (layer == 0)
        {
            Dismantle();
        }
        else
        {
            CreateBuilding(worker.GetBuilding(), layer);
        }
        EndWork();
    }

    private void EndWork()
    {
        _sliderController.gameObject.SetActive(false);
        _sliderController.Reset();
    }

    private void CreateBuilding(GameObject building, int layer)
    {
        
        GameObject g = Instantiate(building, selectedCell.transform);
        selectedCell.layer = layer;
        
        var a = new Edge(_graphBuilder.matrix[worker.x, worker.y],
            _graphBuilder.matrix[selectedCell.GetComponent<Build>().x, selectedCell.GetComponent<Build>().y]);
        var b = new Edge(_graphBuilder.matrix[selectedCell.GetComponent<Build>().x, selectedCell.GetComponent<Build>().y],
            _graphBuilder.matrix[worker.x, worker.y]);
        _graphBuilder.g.AddEdge(a);
        _graphBuilder.g.AddEdge(b);
        
        
        if (layer == Constant.houseLayer)
        {
            var h = g.GetComponent<House>();
            var newP = h.people;
            h.x = selectedCell.GetComponent<Build>().x;
            h.y = selectedCell.GetComponent<Build>().y;
            people += newP;
            wood -= h.woodNeed;
            jobs = people - jobs;
            entertainment = people - entertainment;
            _peopleManager.SpawnPeople(newP, h);
        }
        else if(layer == Constant.jobLayer)
        {
            var j = g.GetComponent<Job>();
            
            j.x = selectedCell.GetComponent<Build>().x;
            j.y = selectedCell.GetComponent<Build>().y;
            
            jobs = people - building.GetComponent<Job>().jobsNum;
            wood -= building.GetComponent<Job>().woodNeed;
            _peopleManager.AddJobs(j);


        }else if (layer == Constant.entertainmentLayer)
        {
            var e = g.GetComponent<Entertainment>();
            
            e.x = selectedCell.GetComponent<Build>().x;
            e.y = selectedCell.GetComponent<Build>().y;

            entertainment = people - building.GetComponent<Entertainment>().peopleEntertained;
            wood -= building.GetComponent<Entertainment>().woodNeed;
            _peopleManager.AddEntertainment(e);
        }

        peopleText.text = people.ToString();
        jobText.text = jobs.ToString();
        entertainmentText.text = entertainment.ToString();
        woodText.text = wood.ToString();
        g.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
        g.transform.localPosition = new Vector3(0f, 0f, 0f);
        var pos = _graphBuilder[worker.GetTarget().x, worker.GetTarget().y].sceneObject.transform.position;
        g.transform.localRotation = Quaternion.LookRotation(pos - g.transform.position, Vector3.up);
        g.GetComponent<RotateObject>().enabled = false;
        g.SetActive(true);
    }

    public void CreateStreet()
    {
        if (selectedCell.layer == 0)
        {
            selectedCell.GetComponent<MeshRenderer>().material = streetMat;
            selectedCell.layer = Constant.streetLayer;
            buttonsMenu.gameObject.SetActive(false);
            var gridPos = selectedCell.GetComponent<Build>();

            if (gridPos.x != 9 && _graphBuilder.matrix[gridPos.x + 1, gridPos.y].sceneObject.layer ==
                Constant.streetLayer)
            {
                var a = new Edge(_graphBuilder.matrix[gridPos.x + 1, gridPos.y],
                    _graphBuilder.matrix[gridPos.x, gridPos.y]);
                var b = new Edge(_graphBuilder.matrix[gridPos.x, gridPos.y],
                    _graphBuilder.matrix[gridPos.x + 1, gridPos.y]);
                _graphBuilder.g.AddEdge(a);
                _graphBuilder.g.AddEdge(b);

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

            }
        }
    }

    public void SpawnWorker(Transform cell)
    {
        var pos = cell.position;
        var w = Instantiate(workerPrefab);
        w.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
        w.transform.position = new Vector3(pos.x, pos.y + 0.02f, pos.z);
        worker = w.GetComponent<Worker>();
        workerNum++;
        workerText.text = workerNum.ToString();
    }

    public void StartDismantle()
    {
        buttonsMenu.gameObject.SetActive(false);
        worker.SetInfo(0, 1);
        MoveWorker();
        dismantle.SetActive(false);
    }

    public void Dismantle()
    {
        Destroy(selectedCell.transform.GetChild(0).gameObject);
        selectedCell.layer = 0;
    }

}
