using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Catcher : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
    }
}
