using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject buildingMenu;
    [SerializeField] private GameObject buttonsMenu;
    [SerializeField] private GameObject dismantle;
    private static GameManager gm;

    private GameObject selectedCell;
    private GameObject selectedBuilding;
    public GraphBuilder _graphBuilder;
    [SerializeField]
    private Material streetMat;
    [SerializeField]
    public GameObject workerPrefab;

    private GameObject worker;

    private PathfindingSolver _pathfindingSolver;
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
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void SetBuilding(GameObject building, int layer)
    {
        bool ok = false;
        var gridPos = selectedCell.GetComponent<Build>();
        Vector2 goal = new Vector2(0,0);
        if (gridPos.x != 9 && _graphBuilder.matrix[gridPos.x + 1, gridPos.y].sceneObject.layer == Constant.streetLayer){
            goal = new Vector2(gridPos.x + 1, gridPos.y);
            ok = true;
        }else if (gridPos.x != 0 && _graphBuilder.matrix[gridPos.x - 1, gridPos.y].sceneObject.layer == Constant.streetLayer){
            goal = new Vector2(gridPos.x - 1, gridPos.y);
            ok = true;
        }else if (gridPos.y != 9 && _graphBuilder.matrix[gridPos.x, gridPos.y + 1].sceneObject.layer == Constant.streetLayer){
            goal = new Vector2(gridPos.x, gridPos.y + 1);
            ok = true;
        }else if (gridPos.y != 0 && _graphBuilder.matrix[gridPos.x, gridPos.y - 1].sceneObject.layer == Constant.streetLayer){
            goal = new Vector2(gridPos.x, gridPos.y - 1);
            ok = true;
        }

        if (ok)
        {
            //Debug.Log(worker.GetComponent<Worker>().x + " " + worker.GetComponent<Worker>().y + "   " + goal.x + " " + goal.y);

            worker.GetComponent<Worker>().Walk(building, layer,
                _pathfindingSolver.Solve(_graphBuilder.g,
                    _graphBuilder[worker.GetComponent<Worker>().x, worker.GetComponent<Worker>().y],
                    _graphBuilder[goal.x, goal.y]));
        }
        else
        {
            Debug.Log("Devi costruire vicino ad una strada");
        }

    }

    public void Build(GameObject building, int layer)
    {
        GameObject g = Instantiate(building, selectedCell.transform);
        selectedCell.layer = layer;
        g.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
        g.transform.localPosition = new Vector3(0f, 0f, 0f);
        g.transform.localRotation = Quaternion.identity;
        g.GetComponent<RotateObject>().enabled = false;
        g.SetActive(true);
    }

    public void SetStreet()
    {
        selectedCell.GetComponent<MeshRenderer>().material = streetMat;
        selectedCell.layer = Constant.streetLayer;
        buttonsMenu.gameObject.SetActive(false);
        var gridPos = selectedCell.GetComponent<Build>();
        
        if (gridPos.x != 9 && _graphBuilder.matrix[gridPos.x + 1, gridPos.y].sceneObject.layer == Constant.streetLayer)
        {
            var a = new Edge(_graphBuilder.matrix[gridPos.x + 1, gridPos.y],
                _graphBuilder.matrix[gridPos.x, gridPos.y]);
            var b = new Edge(_graphBuilder.matrix[gridPos.x, gridPos.y],
                _graphBuilder.matrix[gridPos.x + 1, gridPos.y]);
            _graphBuilder.g.AddEdge(a);
            _graphBuilder.g.AddEdge(b);
            
        }
        
        if (gridPos.x != 0 && _graphBuilder.matrix[gridPos.x - 1, gridPos.y].sceneObject.layer == Constant.streetLayer)
        {
            var c = new Edge(_graphBuilder.matrix[gridPos.x - 1, gridPos.y],
                _graphBuilder.matrix[gridPos.x, gridPos.y]);
            var d = new Edge(_graphBuilder.matrix[gridPos.x, gridPos.y],
                _graphBuilder.matrix[gridPos.x - 1, gridPos.y]);
            _graphBuilder.g.AddEdge(c);
            _graphBuilder.g.AddEdge(d);
            
        }
        
        if (gridPos.y != 9 && _graphBuilder.matrix[gridPos.x, gridPos.y + 1].sceneObject.layer == Constant.streetLayer)
        {
            var e = new Edge(_graphBuilder.matrix[gridPos.x, gridPos.y + 1],
                _graphBuilder.matrix[gridPos.x, gridPos.y]);
            var f = new Edge(_graphBuilder.matrix[gridPos.x, gridPos.y],
                _graphBuilder.matrix[gridPos.x, gridPos.y + 1]);
            _graphBuilder.g.AddEdge(e);
            _graphBuilder.g.AddEdge(f);
            
        }
        
        if (gridPos.y != 0 && _graphBuilder.matrix[gridPos.x, gridPos.y - 1].sceneObject.layer == Constant.streetLayer)
        {
            var g = new Edge(_graphBuilder.matrix[gridPos.x, gridPos.y - 1],
                _graphBuilder.matrix[gridPos.x, gridPos.y]);
            var h = new Edge(_graphBuilder.matrix[gridPos.x, gridPos.y],
                _graphBuilder.matrix[gridPos.x, gridPos.y - 1]);
            _graphBuilder.g.AddEdge(g);
            _graphBuilder.g.AddEdge(h);
            
        }
    }

    public void SpawnWorker(Transform cell)
    {
        var pos = cell.position;
        worker = Instantiate(workerPrefab);
        worker.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
        worker.transform.position = new Vector3(pos.x, pos.y + 0.02f, pos.z);
    }

    public void Dismantle()
    {
        
        /*for (int i = 0; i < selectedCell.transform.childCount; i++)
        {
            Destroy(selectedCell.transform.GetChild(i));
        }*/
        if(selectedCell.layer != Constant.treeLayer)
            Destroy(selectedCell.transform.GetChild(0).gameObject);

        selectedCell.layer = 0;
        buttonsMenu.gameObject.SetActive(false);
    }

}
