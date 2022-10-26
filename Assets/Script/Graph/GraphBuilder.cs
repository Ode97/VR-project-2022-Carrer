using System;
using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GraphBuilder : MonoBehaviour {

	public int x = 10;
	public int y = 10;
	public float gap = 0.01f;
    public Material firstMaterial = null;
    public Material streetMaterial;
    public GameObject[] trees = new GameObject[3];

    // what to put on the scene, not really meaningful
	public GameObject sceneObject;
	
	public Node[,] matrix;
	protected Dictionary<Node, float[]> map;
	public Graph g;
	public GameObject parent;
	
	void Start ()
	{
		
	}

	public void Create(GameObject sGameObject)
	{
		parent = sGameObject;
		if (sceneObject != null)
		{
			GameManager.GM().SetGraphBuilder(this);
			// create a x * y matrix of nodes (and scene objects)
			g = new Graph();
			if(!GameManager.GM().load)
				matrix = CreateGrid(sceneObject, x, y, gap, firstMaterial, streetMaterial);
			else
			{
				matrix = CreateLoadGrid(x, y, firstMaterial, gap);
				LoadBuilding(streetMaterial);
			}
		}
	}

	protected Node[,] CreateGrid(GameObject o, int x, int y, float gap, Material sm, Material st) {
		Node[,] matrix = new Node[x,y];

		map = new Dictionary<Node, float[]> ();
		for (int i = 0; i < matrix.GetLength(0); i += 1) {
			for (int j = 0; j < matrix.GetLength(1); j += 1)
			{
				GameObject p = Instantiate(o, parent.transform);
				
				p.GetComponent<Build>().x = i;
				p.GetComponent<Build>().y = j;
				matrix[i, j] = new Node("" + i + "," + j, p);
				matrix[i, j].sceneObject.name = o.name + " " + i + " " + j;
				matrix[i, j].sceneObject.transform.position =
					transform.position +
					transform.right * gap * (i - ((x - 1) / 2f)) +
					transform.forward * gap * (j - ((y - 1) / 2f));
				matrix[i, j].sceneObject.transform.rotation = transform.rotation;

				
					
				p.GetComponent<MeshRenderer>().material = sm;
				if ((i == 0) && (j == 0))
				{
					p.GetComponent<MeshRenderer>().material = st;
					p.layer = Constant.streetLayer;
					GameManager.GM().SpawnWorker(p.transform);
					g.AddNode(matrix[i, j]);
				}else if ((i == 1) && j == 0)
				{
					var houses = GameManager.GM().GetHouses();
					
					var fh = Instantiate(houses[0], matrix[i, j].sceneObject.transform);
					
					var building = fh.GetComponent<Building>();
        
					building.SetI(0);

					matrix[i, j].sceneObject.layer = Constant.houseLayer;
					
					var a = new Edge(matrix[i, j], matrix[0, 0]);
					var b = new Edge(matrix[0, 0], matrix[i, j]);
					
					g.AddEdge(a);
					g.AddEdge(b);
					
					var h = fh.GetComponent<House>();
					var newP = h.people;
					h.x = i;
					h.y = j;
					GameManager.GM().people += newP;
					GameManager.GM().jobs += newP;
					GameManager.GM().entertainment += newP;
					GameManager.GM().GetPeopleManager().SpawnPeople(newP, h, matrix[i, j].sceneObject.transform.position);
					
					fh.transform.localPosition = new Vector3(0f, 0f, 0f);
					fh.transform.localScale *= 18;
					building.SetRotation(0);

					var pos = matrix[0, 0].sceneObject.transform.position;
					var pos2 = matrix[1, 0].sceneObject.transform.position;
					fh.transform.rotation = Quaternion.LookRotation(pos - pos2, Vector3.up);
					fh.GetComponent<RotateObject>().enabled = false;
					fh.SetActive(true);
					GameManager.GM().data.people = GameManager.GM().people;
					GameManager.GM().SetText();
				}

				else
				{
					RandomTree(p);
				}
			}
		}
		
		return matrix;
	
	}

	protected Node[,] CreateLoadGrid(int x, int y, Material fm, float gap)
	{

		Node[,] matrix = new Node[x, y];

		for (int i = 0; i < matrix.GetLength(0); i += 1)
		{
			for (int j = 0; j < matrix.GetLength(1); j += 1)
			{
				GameObject p = Instantiate(sceneObject, parent.transform);

				p.GetComponent<MeshRenderer>().material = fm;

				p.GetComponent<Build>().x = i;
				p.GetComponent<Build>().y = j;
				matrix[i, j] = new Node("" + i + "," + j, p);
				matrix[i, j].sceneObject.name = sceneObject.name + " " + i + " " + j;

				matrix[i, j].sceneObject.transform.position =
					transform.position +
					transform.right * gap * (i - ((x - 1) / 2f)) +
					transform.forward * gap * (j - ((y - 1) / 2f));
				matrix[i, j].sceneObject.transform.rotation = transform.rotation;
			}
		}

		return matrix;
	}

	protected void LoadBuilding(Material sm){
		var houses = GameManager.GM().GetHouses();
		var jobs = GameManager.GM().GetJobs();
		var ents = GameManager.GM().GetEntertainments();

		var data = GameManager.GM().data;
		
		FindObjectOfType<DayManager>().time = data.time;
		
		for (int k = 0; k < 100; k++)
		{
			var d = (int)Mathf.Floor(k / 10);
			var c = k % 10;
			
			matrix[c, d].sceneObject.layer = GameManager.GM().data.layers[k];
			if (matrix[c, d].sceneObject.layer == Constant.houseLayer)
			{ 
				GameObject h = Instantiate(houses[data.buildings[k]], matrix[c, d].sceneObject.transform);
				h.SetActive(true);
				h.GetComponent<Building>().SetI(data.buildings[k]);
				h.GetComponent<RotateObject>().enabled = false;
				Rotation(h, GameManager.GM().data.rotation[k], c, d, matrix);
				h.GetComponent<Building>().SetRotation(data.rotation[k]);
				h.GetComponent<House>().x = c;
				h.GetComponent<House>().y = d;
				h.transform.localScale *= 18;
				//GameManager.GM().SpawnPeople(h.GetComponent<House>().people, h.GetComponent<House>(), matrix[c, d].sceneObject.transform.position);
			}else if (matrix[c, d].sceneObject.layer == Constant.entertainmentLayer)
			{
				GameObject h = Instantiate(ents[data.buildings[k]], matrix[c, d].sceneObject.transform);
				h.GetComponent<RotateObject>().enabled = false;
				h.SetActive(true);
				h.GetComponent<Building>().SetI(data.buildings[k]);
				Rotation(h, data.rotation[k], c, d, matrix);
				h.GetComponent<Building>().SetRotation(data.rotation[k]);
				h.GetComponent<Entertainment>().x = c;
				h.GetComponent<Entertainment>().y = d;
				h.transform.localScale *= 18;
				GameManager.GM().GetPeopleManager().AddEntertainment(h.GetComponent<Entertainment>());
			}else if (matrix[c, d].sceneObject.layer == Constant.jobLayer)
			{
				GameObject h = Instantiate(jobs[data.buildings[k]], matrix[c, d].sceneObject.transform);
				h.GetComponent<RotateObject>().enabled = false;
				h.SetActive(true);
				h.GetComponent<Building>().SetI(data.buildings[k]);
				Rotation(h, GameManager.GM().data.rotation[k], c, d, matrix);
				h.GetComponent<Building>().SetRotation(data.rotation[k]);
				h.GetComponent<Job>().x = c;
				h.GetComponent<Job>().y = d;
				h.transform.localScale *= 18;
				GameManager.GM().GetPeopleManager().AddJobs(h.GetComponent<Job>());
			}else if (matrix[c, d].sceneObject.layer == Constant.streetLayer)
			{
				matrix[c, d].sceneObject.GetComponent<MeshRenderer>().material = sm;
				if (data.street[k, 0])
				{
					var e = new Edge(matrix[c, d], matrix[c, d+1]);
					var r = new Edge(matrix[c, d+1], matrix[c, d]);
					g.AddEdge(e);
					g.AddEdge(r);
				}
				if (data.street[k, 1])
				{
					var e = new Edge(matrix[c+1, d], matrix[c, d]);
					var r = new Edge(matrix[c, d], matrix[c+1, d]);
					g.AddEdge(e);
					g.AddEdge(r);
				}
				if (data.street[k, 2])
				{
					var e = new Edge(matrix[c, d-1], matrix[c, d]);
					var r = new Edge(matrix[c, d], matrix[c, d-1]);
					g.AddEdge(e);
					g.AddEdge(r);
				}
				if (data.street[k, 3])
				{
					var e = new Edge(matrix[c-1, d], matrix[c, d]);
					var r = new Edge(matrix[c, d], matrix[c-1, d]);
					g.AddEdge(e);
					g.AddEdge(r);
				}
				
			}else if (matrix[c, d].sceneObject.layer == Constant.treeLayer)
			{
				var t = Random.Range(0, 3);
				if (t < 1)
				{
					Instantiate(trees[0], matrix[c,d].sceneObject.transform);
				}
				else if (t < 2)
				{
					Instantiate(trees[1], matrix[c,d].sceneObject.transform);
				}
				else
				{
					Instantiate(trees[2], matrix[c,d].sceneObject.transform);
				}
			}
			
		}

		for (int k = 0; k < 100; k++)
		{
			for (int f = 0; f < 30; f++)
				if (data.type[k, f] != 99)
				{

					GameManager.GM().GetPeopleManager().SpawnPeople(data.type[k, f], k, data.toX[k, f], data.toY[k, f],
						data.happiness[k, f], data.jobFound[k, f], data.eat[k, f], data.work[k, f], data.stillWork[k, f], 
						data.justEat[k, f], data.endDay[k,f], data.start[k,f], data.hj[k, f, 0], data.hj[k, f, 1]);
				}

		}

		var totalHappiness = 0;
		var people = GameManager.GM().GetPeopleManager().GetPeople();
		foreach (var p in people)
		{
			totalHappiness += p.happiness;
		}

		int tot = Mathf.RoundToInt(totalHappiness / people.Count);
        
		GameManager.GM().SetHappiness(tot);

		var w = data.workerNum;
		
		for (int i = 0; i < w; i++)
		{
			var x = data.workerPos[i] % 10;
			var y = (int)Mathf.Floor(data.workerPos[i] / 10);
			GameManager.GM().SpawnWorker(matrix[x, y].sceneObject.transform);
		}

		GameManager.GM().GetPeopleManager().FoodBuildings();
		//GameManager.GM().load = false;
	}


	private void Rotation(GameObject go, int r, int x, int y, Node[,] matrix)
	{
		if (r == 0)
		{
			go.transform.rotation = Quaternion.LookRotation(matrix[x - 1, y].sceneObject.transform.position -
			                                                matrix[x, y].sceneObject.transform.position);
			
			var e = new Edge(matrix[x - 1, y], matrix[x, y]);
			var t = new Edge(matrix[x, y], matrix[x - 1, y]);
			g.AddEdge(e);
			g.AddEdge(t);
		}
		if (r == 1)
		{
			go.transform.rotation = Quaternion.LookRotation(matrix[x, y + 1].sceneObject.transform.position -
			                                                matrix[x, y].sceneObject.transform.position);
			
			var e = new Edge(matrix[x, y], matrix[x, y + 1]);
			var t = new Edge(matrix[x, y + 1], matrix[x, y]);
			g.AddEdge(e);
			g.AddEdge(t);
		}
		if (r == 2)
		{
			go.transform.rotation = Quaternion.LookRotation(matrix[x + 1, y].sceneObject.transform.position -
			                                                matrix[x, y].sceneObject.transform.position);
			
			var e = new Edge(matrix[x + 1, y], matrix[x, y]);
			var t = new Edge(matrix[x, y], matrix[x + 1, y]);
			g.AddEdge(e);
			g.AddEdge(t);
		}
		if (r == 3)
		{
			go.transform.rotation = Quaternion.LookRotation(matrix[x, y - 1].sceneObject.transform.position -
			                                                matrix[x, y].sceneObject.transform.position);
			
			var e = new Edge(matrix[x, y], matrix[x, y - 1]);
			var t = new Edge(matrix[x, y - 1], matrix[x, y]);
			g.AddEdge(e);
			g.AddEdge(t);
		}
	}

	private void RandomTree(GameObject g)
	{
		var r = Random.Range(0f, 1f);
		if (r <= 0.5f)
		{
			g.layer = Constant.treeLayer;
			var t = Random.Range(0, 3);
			if (t < 1)
			{
				Instantiate(trees[0], g.transform);
			}
			else if (t < 2)
			{
				Instantiate(trees[1], g.transform);
			}
			else
			{
				Instantiate(trees[2], g.transform);
			}
		}
	}

	// quantization
	public Node this[float x, float y] =>
		
		matrix[
			(int) Math.Floor (x / 1), 
			(int) Math.Floor (y / 1) 
		];
	

	// localization
	public float[] this [Node n] => !map.ContainsKey (n) ? null : map [n];

	public Node[] Nodes => map.Keys.ToArray();

	protected virtual float Distance(Node from, Node to) {
        return 1f;
    }

}
