
using System.Collections.Generic;
using UnityEngine;

public class Node {

	
	public GameObject sceneObject;

	public Node(string description, GameObject o = null) {
		this.sceneObject = o;
	}
	
}