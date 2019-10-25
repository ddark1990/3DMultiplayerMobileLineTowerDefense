using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Node2
{
    public class Node : MonoBehaviour, ISelected
    {
        public bool IsSelected, IsOccupied;

        public void DeSelected()
        {
            IsSelected = false;
            NodeController.Instance.CurrentlySelectedNode = null;
            NodeController.Instance.DehighlightNode();
            //Debug.Log("DeSelected " + name);
        }

        public void Selected()
        {
            IsSelected = true;
            NodeController.Instance.CurrentlySelectedNode = this;
            NodeController.Instance.HighlightNode();

            //Debug.Log("Selected " + name);
        }
    }
}