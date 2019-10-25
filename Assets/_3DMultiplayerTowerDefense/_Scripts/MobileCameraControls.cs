using UnityEngine;
using GoomerScripts;
using Photon.Pun;

public class MobileCameraControls : MonoBehaviourPunCallbacks
{
    public static MobileCameraControls Instance;

    [SerializeField] private float minZoomHeight = 0f;
    [SerializeField] private float maxZoomHeight = 10f;
    private Vector3 minZoom;
    private Vector3 maxZoom;
    private Camera Cam;
    Vector3 lastPos;
    public bool isMoving;
    public bool DisableMobileControls = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        Cam = GetComponent<Camera>();

        lastPos = transform.position;
        RefreshCamera();
    }

    private void Update()
    {
        if (!DisableMobileControls)
        {
            MobileControls();
        }
    }

    private void MobileControls()
    {
        var tranZ = 0f;
        var tranX = 0f;

        if (Input.touchCount == 0)
            return;

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
        else
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

            if (PhotonNetwork.IsMasterClient)
            {
                transform.Translate(-touchDeltaPosition.x * 0.01f, 0, -touchDeltaPosition.y * 0.01f, Space.World);
                tranZ = Mathf.Clamp(transform.position.z, -35, 5);
            }
            else
            {
                transform.Translate(touchDeltaPosition.x * 0.01f, 0, touchDeltaPosition.y * 0.01f, Space.World);
                tranZ = Mathf.Clamp(transform.position.z, -7, 16);
            }

            tranX = Mathf.Clamp(transform.position.x, -4, 4);

            transform.position = new Vector3(tranX, transform.position.y, tranZ);
        }
    }

    private void IsMovingCheck()
    {
        if (Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            isMoving = true;
        }
        else isMoving = false;
    }

    private void Zoom(float increment)
    {
        Cam.fieldOfView = Mathf.Clamp(Cam.fieldOfView, minZoomHeight, maxZoomHeight);
        Cam.fieldOfView += increment * 2;
    }

    /// <summary> Call this to refresh initial camera position. </summary>
    public void RefreshCamera() {
        minZoom = transform.position;
        maxZoom = transform.position;
        minZoom.y = minZoomHeight;
        maxZoom.y = maxZoomHeight;
    }
}
