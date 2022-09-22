using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> peoples;

    private List<People> citizens = new List<People>();

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
        }
    }

    public void AddEntertainment(Entertainment cell)
    {
        foreach (var c in citizens)
        {
            c._buildings.Add(cell);
        }
    }

    public void AddJobs(Job cell)
    {
        foreach (var c in citizens)
        {
            if (!c.jobFound)
            {
                c._buildings.Add(cell);
                c.jobFound = true;
            }
        }
    }
    
}
