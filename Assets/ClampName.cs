using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampName : MonoBehaviour
{


    void Update()
    {
        Vector3 namePos = Camera.main.WorldToScreenPoint(this.transform.position);
    }
}
