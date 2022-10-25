using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PathfindingSolver : MonoBehaviour {

	public static bool immediateStop = false;
	
	// two set of nodes (1)

	public static List<Node> visited;
	public static List<Node> unvisited;

	// data structures to extend nodes (2)

	protected struct NodeExtension {
		public float distance;
		public float estimate;
		public Edge predecessor;
	}

	protected static Dictionary<Node, NodeExtension> status;

	//public static Edge[] Solve(Graph g, Node start, Node goal, HeuristicFunction heuristic) {
	public Edge[] Solve(Graph g, Node start, Node goal) {
		// setup sets (1)
		visited = new List<Node>();
		unvisited = new List<Node> (g.getNodes ());

		// set all node tentative distance (2)
		status = new Dictionary<Node, NodeExtension> ();
		foreach (Node n in unvisited) {
			NodeExtension ne = new NodeExtension();
			ne.distance = ( n == start ? 0f : float.MaxValue ); // infinite
			ne.estimate = ( n == start ? ManhattanEstimator(start, goal) : float.MaxValue );
			status [n] = ne;
		}

		// iterate until goal is reached with an optimal path (6)
		while (!CheckSearchComplete(goal, unvisited)) {
			// select net current node (3)
			Node current = GetNextNode();

			if (status [current].distance == float.MaxValue) break; // graph is partitioned

			// assign weight and predecessor to all neighbors (4)
			foreach (Edge e in g.getConnections(current)) {
				if (status[current].distance + e.weight < status[e.to].distance) {
					NodeExtension ne = new NodeExtension();
					ne.distance = status[current].distance + e.weight;
					ne.estimate = ne.distance + ManhattanEstimator(e.to, goal);
					ne.predecessor = e;
					status[e.to] = ne;
					// unlike Dijkstra's, we can now discover better paths
					if (visited.Contains(e.to)) {
						unvisited.Add(e.to);
						visited.Remove(e.to);
					}
				}
			}
			// mark current node as visited (5)
			visited.Add(current);
			unvisited.Remove(current);
		}

		if (status [goal].distance == float.MaxValue) return new Edge[0]; // goal is unreachable

		// walk back and build the shortest path (7)
		List<Edge> result = new List<Edge> ();
		Node walker = goal;

		while (walker != start) {
			result.Add (status [walker].predecessor);
			walker = status [walker].predecessor.from;
		}
		
		result.Reverse ();
		return result.ToArray ();
	}

	// iterate on the unvisited set and get the lowest weight
	protected static Node GetNextNode() {
		Node candidate = null;
		float cDistance = float.MaxValue;
		foreach (Node n in unvisited) {
			if (candidate == null || cDistance > status[n].estimate) {
				candidate = n;
				cDistance = status[n].estimate;
			}
		}
		return candidate;
	}

	// chek if the goal has been reached in a satisfactory way
	protected static bool CheckSearchComplete(Node goal, List<Node> nodeList)
	{
		/*if (goal != null && status.ContainsKey(goal))
		{
			// check if we reached the goal
			if (status[goal].distance == float.MaxValue) return false;
			// check if the first hit is ok 
			if (immediateStop) return true;
			// check if all nodes in list have loger or same paths 
			foreach (Node n in nodeList)
			{
				if (status[n].distance < status[goal].distance) return false;
			}
		}
		else
		{
			Debug.Log("distrutto durante pathfinding");
			return false;
		}*/
		
		
		if (status[goal].distance == float.MaxValue) return false;
		// check if the first hit is ok 
		if (immediateStop) return true;
		// check if all nodes in list have loger or same paths 
		foreach (Node n in nodeList)
		{
			if (status[n].distance < status[goal].distance) return false;
		}

		return true;
		
	}
	
	protected static float EuclideanEstimator(Node from, Node to) {
		return (from.sceneObject.transform.position - to.sceneObject.transform.position).magnitude;
	}

	protected static float ManhattanEstimator(Node from, Node to) {
		return (
			Mathf.Abs(from.sceneObject.transform.position.x - to.sceneObject.transform.position.x) +
			Mathf.Abs(from.sceneObject.transform.position.z - to.sceneObject.transform.position.z)
		);
	}

	protected static float BisectorEstimator(Node from, Node to) {
		Ray r = new Ray (Vector3.zero, to.sceneObject.transform.position);
		return Vector3.Cross(r.direction, from.sceneObject.transform.position - r.origin).magnitude;
	}

	protected static float FullBisectorEstimator(Node from, Node to) {
		Ray r = new Ray (Vector3.zero, to.sceneObject.transform.position);
		Vector3 toBisector = Vector3.Cross (r.direction, from.sceneObject.transform.position - r.origin);
		return toBisector.magnitude + (to.sceneObject.transform.position - ( from.sceneObject.transform.position + toBisector ) ).magnitude ;
	}

	protected static float ZeroEstimator (Node from, Node to) { return 0f; }

}


	

