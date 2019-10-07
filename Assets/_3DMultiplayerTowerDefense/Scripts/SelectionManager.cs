using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Photon.Pun;

namespace GoomerScripts
{
    public class SelectionManager : MonoBehaviourPunCallbacks 
    {
        public static SelectionManager Instance;

        public List<GameObject> currentlySelectedList;
        public GameObject currentlySelectedObject;
        public GameObject lastSelected;

        public float firstClickTime, timeBetweenClicks;
        public bool coroutineAllowed;
        public int clickCounter;
        public Camera cam;

        Scene menuScene;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            menuScene = SceneManager.GetSceneByName("MenuScene");
        }

        private void Start()
        {
            firstClickTime = 0f;
            timeBetweenClicks = 0.2f;
            clickCounter = 0;
            coroutineAllowed = true;
        }

        void Update()
        {
            if (cam == null || menuScene.isLoaded || MobileCameraControls.Instance.DisableMobileControls) return;

            DoubleClickSelect();
            SelectObject();

            if(currentlySelectedObject != null && currentlySelectedObject.GetComponent<Node>()) //enables node when selecting for updates
            {
                currentlySelectedObject.GetComponent<Node>().enabled = true;
            }

            if (currentlySelectedObject != null && currentlySelectedObject.GetComponentInChildren<CreepSenderUI>())
            {
                currentlySelectedObject.GetComponentInChildren<CreepSenderUI>().enabled = true;
            }
        }

        void DoubleClickSelect()
        {
            if (Input.GetMouseButtonUp(0))
                clickCounter += 1;

            if (clickCounter == 1 && coroutineAllowed)
            {
                firstClickTime = Time.time;
                StartCoroutine(DoubleClickDetect());
            }
        }

        private IEnumerator DoubleClickDetect()
        {
            coroutineAllowed = false;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            while (Time.time < firstClickTime + timeBetweenClicks)
            {
                if (clickCounter == 2)
                {
                    //Debug.Log("Double Click");
                    break;
                }
                yield return new WaitForEndOfFrame();
            }
            clickCounter = 0;
            firstClickTime = 0f;
            coroutineAllowed = true;
        }

        private void CurrentObjectSet()
        {
            if (currentlySelectedList.Count == 1)
            {
                if (currentlySelectedList[0] != currentlySelectedObject)
                {
                    lastSelected = currentlySelectedObject;
                    currentlySelectedObject = currentlySelectedList[0];
                }
            }
            else if (currentlySelectedList.Count <= 0)
            {
                currentlySelectedObject = null;
            }

            for (int i = 0; i < currentlySelectedList.Count; i++) //removes anything null
            {
                if (currentlySelectedList[i].gameObject == null)
                {
                    currentlySelectedList.Remove(currentlySelectedList[i]);
                }
            }
        }

        public void SelectObject()
        {
            if (IsPointerOverUiObject()) return;

            if (MobileCameraControls.Instance.isMoving == true)
            {
                currentlySelectedObject = null;
                currentlySelectedList.Clear();
                return;
            }

            if (!Input.GetKeyDown(KeyCode.Mouse0)) return;

            var ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit)/* && !Input.GetKey(KeyCode.LeftControl)*/)
            {
                if (!hit.collider.GetComponent<Selectable>())
                {
                    currentlySelectedList.Clear();
                    currentlySelectedObject = null;
                    return;
                }

                if (hit.collider.GetComponent<Selectable>())
                {
                    if (currentlySelectedList.Contains(hit.collider.gameObject))
                    {
                        currentlySelectedList.Clear();
                        currentlySelectedObject = null;
                        return;
                    }
                    else
                    {
                        currentlySelectedList.Clear();
                        currentlySelectedList.Add(hit.collider.gameObject);
                        //Debug.Log("Selected " + hit.collider.Name);
                    }
                }
                else if(hit.collider == null)
                {
                    Debug.Log("null collider hit");
                }
            }
            //else if (hit.collider.GetComponent<Selectable>() && !currentlySelectedList.Contains(hit.collider.gameObject))
            //{
            //    currentlySelectedList.Add(hit.collider.gameObject);
            //}
            //else if (hit.collider.GetComponent<Selectable>() && currentlySelectedList.Contains(hit.collider.gameObject))
            //{
            //    currentlySelectedList.Remove(hit.collider.gameObject);
            //}
            CurrentObjectSet();
        }

        public static bool IsPointerOverUiObject()
        {
            var eventDataCurrentPosition = new PointerEventData(EventSystem.current)
            {
                position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
            };
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
    }

}