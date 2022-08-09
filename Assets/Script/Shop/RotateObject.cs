using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class RotateObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    float rotSpeed = 100;
    private bool ok = false;

    void Update()
    {
        if (ok)
        {
            //float rotX = Input.GetAxis("Mouse X") * rotSpeed * Mathf.Deg2Rad;
            //float rotY = Input.GetAxis("Mouse Y") * rotSpeed * Mathf.Deg2Rad;

            //transform.Rotate(transform.up, -rotX);
            //transform.Rotate(Vector3.right, -rotY);
            transform.Rotate(0, 0, rotSpeed * Mathf.Deg2Rad, Space.World);
        }
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        ok = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ok = false;
    }
}
