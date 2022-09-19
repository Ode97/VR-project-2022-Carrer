
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGraph {

	protected float tileSize;
	protected Node[,] matrix;
	protected Dictionary<Node, float[]> map;
	
	public TileGraph(float x, float y, float ts, GameObject o) {

		tileSize = ts;

		matrix = new Node[
          (int) Math.Floor (x / tileSize) + 1,
          (int) Math.Floor (y / tileSize) + 1
          ];

		map = new Dictionary<Node, float[]> ();
		for (var i = 0; i < matrix.GetLength(0); i += 1) {
			for (var j = 0; j < matrix.GetLength(1); j += 1) {
				var n = new Node ("" + i + "," + j);
				matrix [i, j] = n;
				map [n] = new float[] { (x + .5f) * tileSize, (y + .5f) * tileSize };
			}
		}	
	}

	// quantization
	public Node this [float x, float y] =>
		matrix[
			(int) Math.Floor (x / tileSize), 
			(int) Math.Floor (y / tileSize) 
		];

	// localization
	public float[] this [Node n] => !map.ContainsKey (n) ? null : map [n];

	public Node[] Nodes => map.Keys.ToArray();
}