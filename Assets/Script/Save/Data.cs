using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class Data
{
    public int wood = 0;

    public int people = 0;

    public int jobs = 0;

    public int entertainment = 0;

    public int workerNum = 0;

    public bool[,] street = new bool[100, 4];

    public int[] layers = new int[100];

    public int[] buildings = new int[100];

    public int[] rotation = new int[100];

    public Data()
    {
        
    }
    
    public Data(int wood, int people, int jobs, int entertainment, int workerNum, bool[,] streets, int[] layers, int[] buildings, int[] rot)
    {
        this.wood = wood;
        this.people = people;
        this.jobs = jobs;
        this.entertainment = entertainment;
        this.workerNum = workerNum;
        this.street = streets;
        this.layers = layers;
        this.buildings = buildings;
        this.rotation = rot;
    }
}
