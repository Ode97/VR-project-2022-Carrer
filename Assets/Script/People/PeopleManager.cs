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
    private List<Food> foodBuildings = new List<Food>();

    public void SpawnPeople(int num, House cell, Vector3 pos)
    {
        var max = peoples.Count;
        for (var i = 0; i < num; i++)
        {
            var p = Random.Range(0, max);
            var citizen = Instantiate(peoples[p]);
            var c = citizen.GetComponent<People>();
            cell.AddPeople(c);
            citizen.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            citizen.transform.localPosition = new Vector3(pos.x, pos.y + 0.01f, pos.z);
            c.SetHouse(cell);
            
            //citizen.GetComponent<People>().StartMove(cell.x, cell.y,cell.x, cell.y);
            
            citizens.Add(citizen.GetComponent<People>());
            c._buildings.AddRange(entertainment);
            var food = FoodBuildingChoice(cell);
            if (food)
                c._buildings.Add(food);
            
            if (jobsRemain.Count > 0)
            {
                if (jobsRemain[0].CheckJob(c))
                {
                    c._buildings.Add(jobsRemain[0]);
                }else
                    jobsRemain.Remove(jobsRemain[0]);
            }

        }
    }

    private Food FoodBuildingChoice(House h)
    {
        var l = int.MaxValue;
        Food foodBuilding = null;
        
        foreach (var f in foodBuildings)
        {
            var d = GameManager.GM().PathSolver(h.x, h.y, f.x, f.y);
            if (d.Length < l)
                foodBuilding = f;
        }
        return foodBuilding;
    }

    public void RemovePeople(House h)
    {
        foreach (var p in h.GetPeople())
        {
            Destroy(p.gameObject);
        }
        h.GetPeople().RemoveAll(x => x);
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
        Food f;
        if (cell.gameObject.TryGetComponent<Food>(out f))
        {
            foodBuildings.Add(f);
            foreach (var c in citizens)
            {
                c._buildings.Add(f);
            }
        }
        jobsRemain.Add(cell);
        foreach (var c in citizens.Where(c => !c.jobFound))
        {
            if (cell.CheckJob(c))
            {
                c.SetJob(cell);
                c.jobFound = true;
            }
            else
            {
                jobsRemain.Remove(cell);
            }
        }
    }
    
}
