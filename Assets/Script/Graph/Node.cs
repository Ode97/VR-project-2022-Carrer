
using System.Collections.Generic;
using UnityEngine;

public class Node {

	
	public GameObject sceneObject;

	public Node(string description, GameObject o = null) {
		this.sceneObject = o;
	}

	public override string ToString()
	{
		var x = sceneObject.GetComponent<Build>().x.ToString();
		var y = sceneObject.GetComponent<Build>().y.ToString();
		return "Node pos: " + x + " " + y;
	}
}