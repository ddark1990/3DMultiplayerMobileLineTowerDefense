using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatchSystem
{
    public class Node : MonoBehaviour, ISelected
    {
        public bool IsSelected, IsOccupied;

        public void DeSelected()
        {
            IsSelected = false;
            NodeController.Instance.SelectedNode = null;
            NodeController.Instance.DehighlightNode();

            //Debug.Log("DeSelected " + name);
        }

        public void Selected()
        {
            IsSelected = true;
            NodeController.Instance.SelectedNode = this;
            NodeController.Instance.HighlightNode();

            //Debug.Log("Selected " + name);
        }
    }
}