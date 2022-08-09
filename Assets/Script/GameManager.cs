using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Canvas buildingMenu;
    [SerializeField] public Camera mainCamera;
    private static GameManager gm;

    private GameObject selectedCell;
    private GameObject selectedBuilding;
    // Start is called before the first frame update

    public static GameManager GM()
    {
        return gm;
    }
    void Start()
    {
        gm = this;
        buildingMenu.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OpenMenu()
    {
        buildingMenu.gameObject.SetActive(true);
        
        //mainCamera.transform.SetPositionAndRotation(menuTransform.position, menuTransform.rotation);
    }
    
    public void CloseMenu()
    {
        buildingMenu.gameObject.SetActive(false);
        //mainCamera.transform.SetPositionAndRotation(initalTransform.position, initalTransform.rotation);
    }

    public void SetCell(GameObject cell)
    {
        selectedCell = cell;
    }

    public void SetBuilding(GameObject building)
    {
        GameObject g = Instantiate(building, selectedCell.transform);

        g.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
        g.transform.localPosition = new Vector3(0f, 0f, 0f);
        g.transform.localRotation = Quaternion.identity;
        g.GetComponent<RotateObject>().enabled = false;
        g.SetActive(true);
    }

}
