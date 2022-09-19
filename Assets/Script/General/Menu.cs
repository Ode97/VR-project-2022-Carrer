using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public void Street()
    {
        GameManager.GM().SetStreet();
    }
    
    public void Dismantle()
    {
        GameManager.GM().Dismantle();
    }

    public void OpenShop()
    {
        GameManager.GM().OpenShop();
    }
}
