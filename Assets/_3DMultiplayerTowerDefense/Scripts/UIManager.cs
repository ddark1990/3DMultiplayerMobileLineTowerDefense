using System;
using System.Collections;
using System.Collections.Generic;
using GoomerScripts;
using Photon.Pun;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public PiUIManager piMan;
    public PiUI buildTowerPi;

    public Camera cam;

    private void Awake()
    {
        cam = FindObjectOfType<Camera>();
        buildTowerPi.gameObject.SetActive(false);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        piMan.gameObject.SetActive(true);
    }

    public void ToggleBuildUI()
    {
        //if (SelectionManager.Instance.currentlySelectedObject == null)
        //{
        //    buildTowerPi.gameObject.SetActive(false);
        //    return;
        //}

        //Vector3 worldPos = cam.WorldToScreenPoint(transform.position);
        //Vector3 selectedWorldPos = cam.WorldToScreenPoint(SelectionManager.Instance.currentlySelectedObject.transform.position);

        //if (worldPos != selectedWorldPos)
        //    worldPos = selectedWorldPos;

        //buildTowerPi.gameObject.SetActive(true);

        //foreach (var towerPlacer in ConstructionManager.Instance.towerPlacers)
        //{
        //    foreach (PiUI.PiData data in piMan.GetPiUIOf("BuildTowerMenu").piData)
        //    {
        //        if(PhotonNetwork.LocalPlayer == towerPlacer.photonView.Owner)
        //        {
        //            var placer = towerPlacer;
        //            data.onSlicePressed.AddListener(() => placer.photonView.RPC("BuildArrowTower", RpcTarget.All));
        //        }
        //    }
        //}


        //piMan.RegeneratePiMenu("BuildTowerMenu");
        //buildTowerPi.OpenMenu(worldPos);
    }
}
