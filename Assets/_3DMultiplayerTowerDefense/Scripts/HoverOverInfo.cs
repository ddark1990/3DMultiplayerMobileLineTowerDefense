using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HoverOverInfo : MonoBehaviour
{
    public static HoverOverInfo HOI;

    [SerializeField] LayerMask layerMask;
    [HideInInspector] public bool overObject = false;

    public GameObject hoveredObject;

    public Camera cam;

    private void Awake()
    {
        if(HOI == null)
        {
            HOI = this;
        }
    }

    public void Update()
    {
        if (SceneManager.GetActiveScene().name == "MenuScene") return;
        RaycastFromMouse();
    }

    public void RaycastFromMouse()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, layerMask))
        {
            hoveredObject = hit.transform.gameObject;
            overObject = true;
        }
        else
        {
            overObject = false;
            hoveredObject = null;
        }
    }
}
