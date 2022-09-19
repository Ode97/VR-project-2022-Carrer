using System;
using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GraphBuilder : MonoBehaviour {

	public int x = 10;
	public int y = 10;
	public Color edgeColor = Color.red;
	public float gap = 0.01f;
    public Material firstMaterial = null;
    public Material secondMaterial = null;
    public Material streetMaterial;
    public GameObject[] trees = new GameObject[3];
    private bool s = false;

    // what to put on the scene, not really meaningful
	public GameObject sceneObject;
	
	public Node[,] matrix;
	protected float tileSize;
	protected Dictionary<Node, float[]> map;
	public Graph g;
	
	void Start () {
		
		if (sceneObject != null)
		{
			
			// create a x * y matrix of nodes (and scene objects)
			g = new Graph();
			matrix = CreateGrid(sceneObject, x, y, gap, firstMaterial, secondMaterial, streetMaterial);
			// create a graph and put random edges inside
			GameManager.GM().SetGraphBuilder(this);
		}
	}

	protected Node[,] CreateGrid(GameObject o, int x, int y, float gap, Material sm, Material em, Material st) {
		Node[,] matrix = new Node[x,y];

		map = new Dictionary<Node, float[]> ();
		
		for (int i = 0; i < matrix.GetLength(0); i += 1) {
			for (int j = 0; j < matrix.GetLength(1); j += 1)
			{
				GameObject p = Instantiate(o);
				p.GetComponent<Build>().x = i;
				p.GetComponent<Build>().y = j;
				matrix[i, j] = new Node("" + i + "," + j, p);
				matrix[i, j].sceneObject.name = o.name + " " + i + " " + j;
				matrix[i, j].sceneObject.transform.position =
					transform.position +
					transform.right * gap * (i - ((x - 1) / 2f)) +
					transform.forward * gap * (j - ((y - 1) / 2f));
				matrix[i, j].sceneObject.transform.rotation = transform.rotation;

				if ((i % 2 == 0 && j % 2 != 0) || (j % 2 == 0 && i % 2 != 0) && s)
				{
					Debug.Log(i + " " + j);
					p.GetComponent<MeshRenderer>().material = sm;
				}
				else
				{
					Debug.Log(i + " " + j + " a");
					p.GetComponent<MeshRenderer>().material = em;
				}

				if (!s)
				{
					p.GetComponent<MeshRenderer>().material = st;
					p.layer = Constant.streetLayer;
					GameManager.GM().SpawnWorker(p.transform);
					g.AddNode(matrix[i,j]);
					s = true;
				}
				else
				{
					RandomTree(p);
				}
			}
		}
		
		return matrix;
	
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
	public Node this [float x, float y] =>
		
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
