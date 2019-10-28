using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.EventSystems;

namespace MatchSystem
{
    public class TowerButton : MonoBehaviour
    {
        [Header("ManualSet")]
        public Image TowerImage;
        public TextMeshProUGUI CostText;
        public TextMeshProUGUI NameText;
        public Button Button;

        [Header("AutoSet")]
        public GameObject TowerPrefab;
        public TowerPlacer TowerPlacer;

        public void Start()
        {
            var selMan = SelectionManager.Instance;
            Button.onClick.AddListener(() => TowerPlacer.PlaceTower(TowerPrefab.name,
                selMan.CurrentlySelectedObject.transform.position + new Vector3(0, .6f, 0), selMan.CurrentlySelectedObject.transform.rotation));
        }
    }
}

