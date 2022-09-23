using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Job : Building
{
    public int jobsNum;
    private int peopleWorking = 0;

    public bool CheckJob()
    {
        if (jobsNum - peopleWorking > 0)
        {
            peopleWorking++;
            return true;
        }
        else
            return false;
    }
}
