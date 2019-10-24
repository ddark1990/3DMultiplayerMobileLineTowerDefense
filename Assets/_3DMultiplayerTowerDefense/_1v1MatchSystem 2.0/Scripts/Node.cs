using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Node2
{
    public class Node : MonoBehaviour, ISelected
    {
        public void DeSelected()
        {
            Debug.Log("Deselected " + name);
        }

        public void Selected()
        {
            Debug.Log("Selected " + name);
        }
    }
}