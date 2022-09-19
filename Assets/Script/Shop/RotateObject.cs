using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.TouchPhase;
using UnityEngine.XR.ARFoundation.Samples;

public class RotateObject : MonoBehaviour
{
    float rotSpeed = 100;
    private bool ok = false;
    private Touch touch;
    private Vector2 oldTouchPosition;
    private Vector2 newTouchPosition;

    void Update()
    {
        if(Input.touchCount == 1)
        {
            float rotateSpeed = 0.09f;
            Touch touchZero = Input.GetTouch(0);
 
            //Rotate the model based on offset
            Vector3 localAngle = this.transform.localEulerAngles;
            localAngle.y -= rotateSpeed * touchZero.deltaPosition.x;
            this.transform.localEulerAngles = localAngle;
        }
    }

}
