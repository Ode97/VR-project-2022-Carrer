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

    public int food = 0;

    public bool[,] street = new bool[100, 4];

    public int[] layers = new int[100];

    public int[] buildings = new int[100];

    public int[] rotation = new int[100];

    public float time = 0;

    public int[] workerPos = new int[5];

    public bool[,] endDay;
    public bool[,] start;
    public int[,] type;
    public int[,] toX;
    public int[,] toY;
    public int[,] happiness;
    public bool[,] jobFound;
    public bool[,] eat;
    public bool[,] work;
    public bool[,] stillWork;
    public bool[,] justEat;
    public int[,,] hj;


    public Data()
    {
        endDay = new bool[100, 30];
        start = new bool[100, 30];
        type = new int[100, 30];
        toX = new int[100, 30];
        toY = new int[100, 30];
        happiness = new int [100, 30];
        jobFound = new bool[100, 30];
        eat = new bool[100, 30];
        work = new bool[100, 30];
        stillWork = new bool[100, 30];
        justEat = new bool[100, 30];
        hj = new int[100, 30, 2];
    }
    
    
}
