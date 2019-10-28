using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatchSystem
{
    public class Node : MonoBehaviour, ISelected
    {
        public NetworkPlayer NetworkOwner;

        public bool IsSelected, IsOccupied, Interactable;

        public void DeSelected()
        {
            if (!Interactable) return;

            MatchBuyMenu.Instance.towerMenuAnim.SetBool("TowerMenuOpen", false);
            IsSelected = false;
            NodeController.Instance.SelectedNode = null;
            NodeController.Instance.DehighlightNode();

            Debug.Log("DeSelected " + name);
        }

        public void Selected()
        {
            MatchBuyMenu.Instance.ResetMenus();
            MatchBuyMenu.Instance.BuyCreepMenuOpen = false;

            if (!Interactable) return;

            IsSelected = true;
            NodeController.Instance.SelectedNode = this;
            NodeController.Instance.HighlightNode();
            MatchBuyMenu.Instance.towerMenuAnim.SetBool("TowerMenuOpen" , true);

            Debug.Log("Selected " + name);
        }
    }
}