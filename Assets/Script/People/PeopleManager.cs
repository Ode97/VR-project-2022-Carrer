using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PeopleManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> peoples;

    private List<People> citizens = new List<People>();

    private List<Entertainment> entertainment = new List<Entertainment>();
    private List<Job> jobsRemain = new List<Job>();
    private List<Food> foodBuildings = new List<Food>();
    private bool updateHappiness = false;
    private bool startGame = false;

    private void Start()
    {
        
    }

    void Update()
    {
        if(!updateHappiness)
            StartCoroutine(Happiness());
        
    }

    private IEnumerator Happiness()
    {
        updateHappiness = true;
        var totalHappiness = 0;
        yield return new WaitForSeconds(1);
        foreach (var p in citizens)
        {
            totalHappiness += p.happiness;
        }

        updateHappiness = false;
        int tot = 0;
        if (citizens.Count > 0)
        {
            startGame = true;
            tot = Mathf.RoundToInt(totalHappiness / citizens.Count);
        }
        else if(startGame)
        {
            GameManager.GM().EndGame();
        }
        
        

        GameManager.GM().SetHappiness(tot);
        
        if (tot < 30 && startGame)
        {
            
            GameManager.GM().EndGame();
        }
    }

    public void SpawnPeople(int num, House cell, Vector3 pos)
    {
        var max = peoples.Count;
        for (var i = 0; i < num; i++)
        {
            var p = Random.Range(0, max);
            var r = Random.insideUnitCircle * 0.025f;
            var citizen = Instantiate(peoples[p], GameManager.GM()._graphBuilder.parent.transform);
            var c = citizen.GetComponent<People>();
            c.type = p;
            cell.AddPeople(c);
            citizen.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            citizen.transform.position = new Vector3(pos.x + r.x, pos.y + 0.01f, pos.z + r.y);
            c.SetHouse(cell);
            citizens.Add(citizen.GetComponent<People>());
            c._buildings.AddRange(entertainment);
            /*var food = FoodBuildingChoice(cell);
            if (food)
                c._buildings.Add(food);*/
            
            if (jobsRemain.Count > 0)
            {
                if (jobsRemain[0].CheckJob(c))
                {
                    c._buildings.Add(jobsRemain[0]);
                }else
                    jobsRemain.Remove(jobsRemain[0]);
            }
            FoodBuildings();
        }
    }
    
    //------------------------------------------------
    
    public void SpawnPeople(int type, int pos, int toX, int toY, int happiness, bool jobFound, bool eat,
                            bool work, bool stillWork, bool justEat, bool endDay, bool start, int h, int j)
    {


        var p = type;
        var r = Random.insideUnitCircle * 0.025f;
        var citizen = Instantiate(peoples[p], GameManager.GM()._graphBuilder.parent.transform);
        var c = citizen.GetComponent<People>();
        c.loadCitizen = true;
        c.type = p;
        c.x = pos % 10;
        c.y = (int)Mathf.Floor(pos / 10);

        c.SetCurrentTarget(GameManager.GM()._graphBuilder.matrix[toX, toY].sceneObject.GetComponentInChildren<Building>().gameObject);
        c.happiness = happiness;
        c.jobFound = jobFound;
        c.eat = eat;
        c.work = work;
        c.stillWorking = stillWork;
        c.justEat = justEat;
        c.start = start;
        c.endDay = endDay;
        c.SetHouse(GameManager.GM()._graphBuilder.matrix[h%10, (int)Mathf.Floor(h / 10)].sceneObject.GetComponentInChildren<House>());
        c.GetHouse().AddPeople(c);
        
        if (j != 99)
        {
            c.SetJob(GameManager.GM()._graphBuilder.matrix[j % 10, (int) Mathf.Floor(j / 10)].sceneObject
                .GetComponentInChildren<Job>());
            c.GetJob().AddPeople(c);
        }


        var position = GameManager.GM()._graphBuilder.matrix[c.x, c.y].sceneObject.transform.position;
        
        citizen.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        citizen.transform.position = new Vector3(position.x + r.x, position.y + 0.01f, position.z + r.y);

        citizens.Add(c);
        c._buildings.AddRange(entertainment);
        
        /*if (jobsRemain.Count > 0)
        {
            if (jobsRemain[0].CheckJob(c))
            {
                c._buildings.Add(jobsRemain[0]);
            }else
                jobsRemain.Remove(jobsRemain[0]);
        }*/

        
    }

    public List<People> GetPeople()
    {
        return citizens;
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

    public void RemoveEnt(Entertainment cell)
    {
        entertainment.Remove(cell);
        foreach (var c in citizens)
        {
            c._buildings.Remove(cell);
            MoveFromDismantleCell(c, cell);
        }
    }

    public void RemoveJob(Job cell)
    {
        jobsRemain.Remove(cell);
        foreach (var c in citizens)
        {
            c._buildings.Remove(cell);
            MoveFromDismantleCell(c, cell);
        }
    }

    private void MoveFromDismantleCell(People c, Building cell)
    {
        var t = c.GetCurrentTarget().GetComponent<Building>();
        var m = GameManager.GM()._graphBuilder.matrix;
        c.runAway = true;
        if (c.x == cell.x && c.y == cell.y)
        {

            if (c.x != 9 && m[c.x + 1, c.y].sceneObject.layer ==
                Constant.streetLayer)
            {
                var x = m[c.x + 1, c.y].sceneObject.GetComponent<Build>().x;
                var y = m[c.x + 1, c.y].sceneObject.GetComponent<Build>().y;

                c.x = x;
                c.y = y;
            }else if (c.y != 9 && m[c.x, c.y + 1].sceneObject.layer == Constant.streetLayer)
            {
                var x = m[c.x, c.y + 1].sceneObject.GetComponent<Build>().x;
                var y = m[c.x, c.y + 1].sceneObject.GetComponent<Build>().y;
                
                c.x = x;
                c.y = y;
            }else if (c.x != 0 && m[c.x - 1, c.y].sceneObject.layer == Constant.streetLayer)
            {
                var x = m[c.x - 1, c.y].sceneObject.GetComponent<Build>().x;
                var y = m[c.x - 1, c.y].sceneObject.GetComponent<Build>().y;
                
                c.x = x;
                c.y = y;
            }else if (c.y != 0 && m[c.x, c.y - 1].sceneObject.layer == Constant.streetLayer)
            {
                var x = m[c.x, c.y - 1].sceneObject.GetComponent<Build>().x;
                var y = m[c.x, c.y - 1].sceneObject.GetComponent<Build>().y;
                
                c.x = x;
                c.y = y;
            }
            
            c.StartMove(c.x, c.y);
        }else if (t.x == cell.x && t.y == cell.y)
        {
            c.SetCurrentTarget(c.GetHouse().gameObject);
            c.StartMove(c.x, c.y);
        }
    }

    public void AddJobs(Job cell)
    {
        Building b = cell;
        Food f;
        if (cell.gameObject.TryGetComponent<Food>(out f))
        {
            foodBuildings.Add(f);
            
        }
        jobsRemain.Add(cell);
        foreach (var c in citizens.Where(c => !c.jobFound))
        {
            if (cell.CheckJob(c))
            {
                c.SetJob(cell);
                b.AddPeople(c);
                c.jobFound = true;
            }
            else
            {
                jobsRemain.Remove(cell);
            }
        }
    }

    public void FoodBuildings()
    {
        foreach (var c in citizens)
        {
            foreach (var b in foodBuildings)
            {
                if(!c._buildings.Contains(b))
                    c._buildings.Add(b);
            }
            
        }
    }
    
}
