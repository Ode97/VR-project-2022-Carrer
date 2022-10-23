
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph {

	// holds all edgeds going out from a node
	private Dictionary<Node, List<Edge>> data;
	public Graph() {
		data = new Dictionary<Node, List<Edge>>();
	}

	public void AddEdge(Edge e) {
		AddNode (e.from);
		AddNode (e.to);
		if (!data[e.from].Contains(e))
			data [e.from].Add (e);
	}

	// used only by AddEdge 
	public void AddNode(Node n) {
		if (!data.ContainsKey (n))
			data.Add (n, new List<Edge> ());
	}

	public void RemoveEdge(Node n)
	{
		if (data.ContainsKey(n))
		{
			var edges = getConnections(n);
			foreach (var edge in edges)
			{

				data[edge.@from].Remove(edge);
				
				var edges2 = getConnections(edge.to);
				foreach (var e in edges2)
				{
					if(e.to == n)
						data[e.@from].Remove(e);
				}
				
			}

			data.Remove(n);
		}

	}

	// returns the list of edged exiting from a node
	public Edge[] getConnections(Node n) {
		if (!data.ContainsKey (n)) return new Edge[0];
		return data [n].ToArray ();
	}

	public Node[] getNodes() {
		return data.Keys.ToArray ();
	}

}