using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GoomerScripts
{
    public class Selectable : MonoBehaviour
    {
        Material[] cachedMaterials;
        Renderer myRend;

        void Awake()
        {
            //GrabRenderer();
        }
        void GrabRenderer()
        {
            if (GetComponent<Renderer>() == null)
            {
                myRend = GetComponentInChildren<Renderer>();
            }
            else myRend = GetComponent<Renderer>();

            cachedMaterials = myRend.materials;
        }
    }
}
