using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARFoundation.Samples;
using UnityEngine.XR.ARSubsystems;
using TouchPhase = UnityEngine.TouchPhase;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceOnPlane : PressInputBase
{
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject m_PlacedPrefab;
    private float previousDistance = 0;

    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }

    bool m_Pressed;

    protected override void Awake()
    {
        base.Awake();
        m_RaycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {

        if (Pointer.current == null || m_Pressed == false)
            return;

        var touchPosition = Pointer.current.position.ReadValue();

        if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            // Raycast hits are sorted by distance, so the first one
            // will be the closest hit.
            var hitPose = s_Hits[0].pose;
            if (spawnedObject == null)
            {
                spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);
                
                foreach (var plane in GetComponent<ARPlaneManager>().trackables)
                {
                    plane.gameObject.SetActive(false);
                }
                var points =  GetComponent<ARSessionOrigin>().GetComponent<ARPointCloudManager>().trackables;
                foreach(var pts in points)
                {
                    pts.gameObject.SetActive(false);
                }
                GetComponent<ARSessionOrigin>().GetComponent<ARPointCloudManager>().enabled = false;
                GetComponent<ARPlaneManager>().enabled = false;
                
                spawnedObject.GetComponent<GraphBuilder>().Create(spawnedObject);

                var t = FindObjectOfType<Tutorial>();
                t.enabled = true;
            }else if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                /*var currentDistance = Vector3.Distance(hitPose.position, s_Hits[1].pose.position);
                if (currentDistance - previousDistance > 0)
                    spawnedObject.transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
                else if(currentDistance - previousDistance < 0)
                {
                    spawnedObject.transform.localScale -= new Vector3(0.01f, 0.01f, 0.01f);
                }*/
                spawnedObject.transform.position = hitPose.position;
                //previousDistance = Vector3.Distance(hitPose.position, s_Hits[1].pose.position);
            }else if (Input.touchCount == 3 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                //var r = spawnedObject.transform.rotation;
                //spawnedObject.transform.rotation = new Quaternion(r.x, r.y + 0.05f, r.z, r.w);
                Vector3 localAngle = spawnedObject.transform.localEulerAngles;
                localAngle.y -= 0.5f * Input.GetTouch(0).deltaPosition.x;
                spawnedObject.transform.localEulerAngles = localAngle;
            }
        }
    }

    protected override void OnPress(Vector3 position) => m_Pressed = true;

    protected override void OnPressCancel() => m_Pressed = false;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    ARRaycastManager m_RaycastManager;
}

