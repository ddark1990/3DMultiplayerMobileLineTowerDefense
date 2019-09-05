using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoomerScripts;
using Photon.Pun;

public class MobileCameraControls : MonoBehaviourPunCallbacks
{
    public static MobileCameraControls Instance;

    Vector3 touchStart;
    Vector3 lastPos;
    public float zoomOutMin = 1;
    public float zoomOutMax = 8;

    float camYPos;

    public bool isMoving;

    public Camera Cam;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        lastPos = transform.position;
    }

    void Update()
    {
        IsMovingCheck();

        if (Input.GetMouseButtonDown(0))
        {
            //touchStart = Cam.ScreenToWorldPoint(Input.mousePosition);
            //if(SelectionManager.SM.currentlySelectedObject != null)
            //{
            //    SelectionManager.SM.currentlySelectedObject.GetComponent<Node>().isSelected = true;
            //    Debug.Log("Selected: " + SelectionManager.SM.currentlySelectedObject);
            //}
        }

        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            Zoom(difference * 0.01f);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            transform.Translate(-touchDeltaPosition.x * 0.01f, -touchDeltaPosition.y * 0.01f, 0);
        }
    }

    void IsMovingCheck()
    {
        if (transform.position != lastPos)
        {
            isMoving = true;
            lastPos = transform.position;
        }
        else isMoving = false;
    }

    void Zoom(float increment)
    {
        Vector3 camPos = Cam.transform.position;
        camYPos = camPos.y;
        Cam.transform.position = new Vector3(camPos.x, Mathf.Clamp(camYPos - increment, zoomOutMin, zoomOutMax), camPos.z); 
    }
}
