using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceOnPlane : MonoBehaviour
{
    [SerializeField] private GameObject gameObjectToPlace;

    private GameObject visualObject;

    private ARRaycastManager _raycastManager;

    private UnityEvent placementUpdate;

    private static List<ARRaycastHit> s_hits = new List<ARRaycastHit>();

    private GameObject spawnedObject;

    public GameObject GameObjectToPlace
    {
        get => gameObjectToPlace;
        set => gameObjectToPlace = value;
    }

    // Start is called before the first frame update
    void Awake()
    {
        _raycastManager = GetComponent<ARRaycastManager>();

        if (placementUpdate == null)
        {
            placementUpdate = new UnityEvent();
            //placementUpdate.AddListener(DiableVisual);
        }
    }

    private bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!TryGetTouchPosition(out Vector2 touchPosition))
            return;

        if (_raycastManager.Raycast(touchPosition, s_hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = s_hits[0].pose;

            if (spawnedObject == null)
            {
                spawnedObject = Instantiate(gameObjectToPlace, hitPose.position, hitPose.rotation);
            }
            else
                spawnedObject.transform.position = hitPose.position;

            placementUpdate.Invoke();
        }
    }

    public void DisableVisual()
    {
        visualObject.SetActive(false);
    }
}
