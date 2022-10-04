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

    void Update()
    {
        if(Input.touchCount > 0)
        {
            float rotateSpeed = 0.09f;
            Touch touchZero = Input.GetTouch(0);
 
            //Rotate the model based on offset
            Vector3 localAngle = this.transform.localEulerAngles;
            localAngle.y -= rotateSpeed * touchZero.deltaPosition.x;
            localAngle.x += rotateSpeed * touchZero.deltaPosition.y;
            this.transform.localEulerAngles = localAngle;
        }
    }

}
