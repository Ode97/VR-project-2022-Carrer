using System.Collections;
using System.Collections.Generic;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

public class Job : Building
{
    public int jobsNum;

    public bool CheckJob(People p)
    {
        if (jobsNum - _peoples.Count > 0)
        {
            _peoples.Add(p);
            return true;
        }
        else
            return false;
    }

    public virtual void Produce()
    {
        
    }
}
