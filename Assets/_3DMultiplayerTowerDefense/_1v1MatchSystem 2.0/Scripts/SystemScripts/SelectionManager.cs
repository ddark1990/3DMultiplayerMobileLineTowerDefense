using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace MatchSystem
{
    public class SelectionManager : MonoBehaviour
    {
        public static SelectionManager Instance;

        public GameObject CurrentlySelectedObject;
        public GameObject LastSelectedObject;
        [Header("Cache")]
        public Camera Cam;
        public LayerMask _LayerMask;

        private Scene menuScene;
        private const string SELECTABLE = "Selectable";
        private const string MENU_SCENE = "MenuScene";

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            else
            {
                Instance = this;
            }

            menuScene = SceneManager.GetSceneByName(MENU_SCENE);
        }

        void Update()
        {
            if (Cam == null || menuScene.isLoaded || MobileCameraControls.Instance.DisableMobileControls) return;

            SelectObject();
        }

        private void SelectObject()
        {
            var ray = Cam.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out var hit, Mathf.Infinity, _LayerMask) || !Input.GetKeyDown(KeyCode.Mouse0)) return;
            Debug.DrawLine(Cam.transform.position, hit.point);

            if (CurrentlySelectedObject != null)
            {
                if(CurrentlySelectedObject != hit.collider.gameObject)
                {
                    LastSelectedObject = CurrentlySelectedObject;

                    if (CurrentlySelectedObject.GetComponent<ISelected>() != null)
                        CurrentlySelectedObject.GetComponent<ISelected>().DeSelected();
                }

                if (hit.collider.gameObject == CurrentlySelectedObject || !hit.collider.CompareTag(SELECTABLE))
                {
                    CurrentlySelectedObject = null;
                    return;
                }
            }

            if (hit.collider.CompareTag(SELECTABLE))
            {
                CurrentlySelectedObject = hit.collider.gameObject;

                if (CurrentlySelectedObject.GetComponent<ISelected>() != null)
                    CurrentlySelectedObject.GetComponent<ISelected>().Selected();
            }
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