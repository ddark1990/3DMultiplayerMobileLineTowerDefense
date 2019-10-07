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

    public bool DisableMobileControls = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        lastPos = transform.position;
    }

    private void Update()
    {
        if(!DisableMobileControls)
        {
            MobileControls();
        }
    }

    private void MobileControls()
    {
        IsMovingCheck();

        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            Zoom(-difference * 0.02f);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

            if (PhotonNetwork.IsMasterClient)
            {
                transform.Translate(-touchDeltaPosition.x * 0.02f, 0, -touchDeltaPosition.y * 0.02f, Space.World);
            }
            else
            {
                transform.Translate(touchDeltaPosition.x * 0.02f, 0, touchDeltaPosition.y * 0.02f, Space.World);
            }
        }
    }

    private void IsMovingCheck()
    {
        if (transform.position != lastPos)
        {
            isMoving = true;
            lastPos = transform.position;
        }
        else isMoving = false;
    }

    private void Zoom(float increment)
    {
        //var camPos = Cam.transform.position;
        //camYPos = camPos.y;
        //Cam.transform.position = new Vector3(camPos.x, Mathf.Clamp(camYPos - increment, zoomOutMin, zoomOutMax), camPos.z); 
        Cam.fieldOfView += increment * 2;
    }
}
