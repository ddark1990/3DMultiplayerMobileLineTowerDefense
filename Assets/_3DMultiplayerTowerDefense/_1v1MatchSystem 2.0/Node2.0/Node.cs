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
            Debug.Log("DeSelected " + name);
            IsSelected = false;
            NodeController.Instance.SelectedNode = null;
            NodeController.Instance.DehighlightNode();
        }

        public void Selected()
        {
            IsSelected = true;
            NodeController.Instance.SelectedNode = this;
            NodeController.Instance.HighlightNode();

            MatchBuyMenu.Instance.BuyTowerMenuOpen = true;
            Debug.Log("Selected " + name);
        }
    }
}