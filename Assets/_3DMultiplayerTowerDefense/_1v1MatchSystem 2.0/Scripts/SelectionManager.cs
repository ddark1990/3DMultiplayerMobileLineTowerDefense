using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace SelectionManager2
{
    public class SelectionManager : MonoBehaviour
    {
        public static SelectionManager Instance;

        public GameObject CurrentlySelectedObject;
        public Camera Cam;
        public LayerMask _LayerMask;

        private Scene menuScene;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            menuScene = SceneManager.GetSceneByName("MenuScene");
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

            if (CurrentlySelectedObject != null && hit.collider.gameObject == CurrentlySelectedObject)
            {
                CurrentlySelectedObject.GetComponent<ISelected>().DeSelected();
                CurrentlySelectedObject = null;
                return;
            }

            CurrentlySelectedObject = hit.collider.gameObject;
            CurrentlySelectedObject.GetComponent<ISelected>().Selected();

            Debug.DrawLine(Cam.transform.position, hit.point);
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