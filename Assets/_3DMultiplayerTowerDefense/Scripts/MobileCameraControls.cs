using UnityEngine;
using GoomerScripts;
using Photon.Pun;

public class MobileCameraControls : MonoBehaviourPunCallbacks
{
    public static MobileCameraControls Instance;

    [SerializeField] private float minZoomHeight = 0f;
    [SerializeField] private float maxZoomHeight = 10f;
    [SerializeField, Range(0f, 1f)] private float zoom = 0.5f;
    private Vector3 minZoom;
    private Vector3 maxZoom;
    Vector3 lastPos;
    public bool isMoving;
    public bool DisableMobileControls = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

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
        zoom += increment;
        zoom = Mathf.Clamp(zoom, 0f, 1f);
        transform.position = Vector3.Lerp(minZoom, maxZoom, zoom);
    }

    /// <summary> Call this to refresh initial camera position. </summary>
    public void RefreshCamera() {
        minZoom = transform.position;
        maxZoom = transform.position;
        minZoom.y = minZoomHeight;
        maxZoom.y = maxZoomHeight;
    }
}
