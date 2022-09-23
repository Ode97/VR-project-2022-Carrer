using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PeopleManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> peoples;

    private List<People> citizens = new List<People>();

    private List<Entertainment> entertainment = new List<Entertainment>();
    private List<Job> jobsRemain = new List<Job>();

    public void SpawnPeople(int num, House cell)
    {
        var max = peoples.Count;
        var pos = GameManager.GM()._graphBuilder[0, 0].sceneObject.transform.position;
        for (var i = 0; i < num; i++)
        {
            var p = Random.Range(0, max);
            var citizen = Instantiate(peoples[p]);
            citizen.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            citizen.transform.localPosition = new Vector3(pos.x, pos.y + 0.01f, pos.z);
            citizen.GetComponent<People>().SetHouse(cell);
            citizen.GetComponent<People>().StartMove(cell.x, cell.y);
            citizens.Add(citizen.GetComponent<People>());
            citizen.GetComponent<People>()._buildings.AddRange(entertainment);
            if (jobsRemain.Count > 0)
            {
                if (jobsRemain[0].CheckJob())
                {
                    citizen.GetComponent<People>()._buildings.Add(jobsRemain[0]);
                }else
                    jobsRemain.Remove(jobsRemain[0]);
            }

        }
    }

    public void AddEntertainment(Entertainment cell)
    {
        entertainment.Add(cell);
        foreach (var c in citizens)
        {
            c._buildings.Add(cell);
        }
    }

    public void AddJobs(Job cell)
    {
        jobsRemain.Add(cell);
        foreach (var c in citizens.Where(c => !c.jobFound))
        {
            if (cell.CheckJob())
            {
                c._buildings.Add(cell);
                c.jobFound = true;
            }
            else
            {
                jobsRemain.Remove(cell);
            }
        }
    }
    
}
