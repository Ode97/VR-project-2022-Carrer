using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class GraphBuilder : MonoBehaviour {

	public int x = 10;
	public int y = 10;
	public Color edgeColor = Color.red;
	public float gap = 0.01f;
    public Material firstMaterial = null;
    public Material secondMaterial = null;

	// what to put on the scene, not really meaningful
	public GameObject sceneObject;

	protected Node[,] matrix;
	protected Graph g;
	
	void Start () {
		if (sceneObject != null)
		{

			// create a x * y matrix of nodes (and scene objects)
			matrix = CreateGrid(sceneObject, x, y, gap, firstMaterial, secondMaterial);

			// create a graph and put random edges inside
			g = new Graph();
		}
	}

	protected virtual Node[,] CreateGrid(GameObject o, int x, int y, float gap, Material sm, Material em) {
		Node[,] matrix = new Node[x,y];
		for (int i = 0; i < x; i += 1) {
			for (int j = 0; j < y; j += 1)
			{
				GameObject g = Instantiate(o);
				matrix[i, j] = new Node("" + i + "," + j, g);
				matrix[i, j].sceneObject.name = o.name + " " + i + " " + j;
				matrix[i, j].sceneObject.transform.position =
					transform.position +
					transform.right * gap * (i - ((x - 1) / 2f)) +
					transform.forward * gap * (j - ((y - 1) / 2f));
				matrix[i, j].sceneObject.transform.rotation = transform.rotation;

				if ((i % 2 == 0 && j % 2 != 0) || (j % 2 == 0 && i % 2 != 0))
				{
					Debug.Log(i + " " + j);
					g.GetComponent<MeshRenderer>().material = sm;
				}
				else
				{
					Debug.Log(i + " " + j + " a");
					g.GetComponent<MeshRenderer>().material = em;
				}
			}
		}
		return matrix;
	}

	protected virtual float Distance(Node from, Node to) {
        return 1f;
    }
    
}
