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
        
        foreach (var c in orderedCells)
        {
            l[i] = c.gameObject.layer;
            if (c.gameObject.transform.childCount > 0 && l[i] != Constant.treeLayer && l[i] != Constant.streetLayer)
            {
                building[i] = c.GetComponentInChildren<Building>().GetI();
                rotations[i] = c.GetComponentInChildren<Building>().GetRotation();
            }
            else
            {
                building[i] = 99;
                rotations[i] = 99;
            }
            i++;
        }

        GameManager.GM().data.layers = l;
        GameManager.GM().data.buildings = building;
        GameManager.GM().data.rotation = rotations;
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
