using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SaveCity : MonoBehaviour
{
    public TextMeshProUGUI saveText;
    public void SaveState()
    {
        var cells = FindObjectsOfType<Build>();

        var orderedCells = cells.OrderBy(a => a.y).ThenBy(a => a.x);

        int[] l = new int[100];
        int[] building = new int[100];
        int[] rotations = new int[100];
        int i = 0;
        
        //--------------------------------------------------------
        
        int[,] type = new int[100, 30];
        int[,] toX = new int[100, 30];
        int[,] toY = new int[100, 30];
        int[,] path_i = new int[100, 30];
        int[,] happiness = new int [100, 30];
        bool[,] busy = new bool[100, 30];
        bool[,] jobFound = new bool[100, 30];
        bool[,] eat = new bool[100, 30];
        bool[,] work = new bool[100, 30];
        bool[,] stillWork = new bool[100, 30];
        bool[,] justEat = new bool[100, 30];
        bool[,] endDay = new bool[100, 30];
        bool[,] start = new bool[100, 30];
        int[, , ] hj = new int[100, 30, 2];

        int[] workerPos = new int[5];

        int z = 0;
        foreach (var w in GameManager.GM().GetWorkers())
        {
            workerPos[z] = w.x + w.y * 10;
            z++;
        }

        var people = GameManager.GM().GetPeopleManager().GetPeople();
        
        foreach (var c in orderedCells)
        {
            l[i] = c.gameObject.layer;
            if (c.gameObject.transform.childCount > 0 && l[i] != Constant.treeLayer)
            {
                if (l[i] != Constant.streetLayer)
                {
                    var b = c.GetComponentInChildren<Building>();
                    building[i] = b.GetI();
                    rotations[i] = b.GetRotation();
                }
                
                
            }
            else
            {
                building[i] = 99;
                rotations[i] = 99;
                //for(int r = 0; r < 30; r++)
                //    type[i, r] = 99;
            }
            
            int x = 0;
            foreach (var p in people)
            {
                var pos = p.x + p.y * 10;
                if (pos == i)
                {
                    endDay[i, x] = p.endDay;
                    start[i, x] = p.start;
                    type[i, x] = p.type;
                    busy[i, x] = p.busy;
                    jobFound[i, x] = p.jobFound;
                    eat[i, x] = p.eat;
                    work[i, x] = p.work;
                    stillWork[i, x] = p.stillWorking;
                    justEat[i, x] = p.justEat;
                    happiness[i, x] = p.happiness;
                    path_i[i, x] = p.i;
                    if (p.GetCurrentTarget())
                    {
                        toX[i, x] = p.GetCurrentTarget().GetComponent<Building>().x;
                        toY[i, x] = p.GetCurrentTarget().GetComponent<Building>().y;
                    }
                    else
                    {
                        toX[i, x] = p.GetHouse().x;
                        toY[i, x] = p.GetHouse().y;
                    }

                    var t = p.GetHouse().x + p.GetHouse().y * 10;
                    int r;
                    if (p.GetJob())
                    {
                        r = p.GetJob().x + p.GetJob().y * 10;
                    }
                    else
                        r = 99;

                    hj[i, x, 0] = t;
                    hj[i, x, 1] = r;
                        
                    x++;
                }
                    
            }

            for (int g = x; g < 30; g++)
                type[i, g] = 99;
            
            i++;
        }

        GameManager.GM().data.endDay = endDay;
        GameManager.GM().data.start = start;
        GameManager.GM().data.layers = l;
        GameManager.GM().data.buildings = building;
        GameManager.GM().data.rotation = rotations;
        GameManager.GM().data.workerPos = workerPos;
        GameManager.GM().data.type = type;
        GameManager.GM().data.jobFound = jobFound;
        GameManager.GM().data.eat = eat;
        GameManager.GM().data.work = work;
        GameManager.GM().data.stillWork = stillWork;
        GameManager.GM().data.justEat = justEat;
        GameManager.GM().data.happiness = happiness;
        GameManager.GM().data.toX = toX;
        GameManager.GM().data.toY = toY;
        GameManager.GM().data.hj = hj;
        GameManager.GM().data.time = FindObjectOfType<DayManager>().time;
        Save.saveData(GameManager.GM().data);
        StartCoroutine(SaveText());
    }

    public IEnumerator SaveText()
    {
        saveText.text = "Game Saved";
        saveText.enabled = true;
        yield return new WaitForSeconds(2);
        saveText.enabled = false;
    }
}
