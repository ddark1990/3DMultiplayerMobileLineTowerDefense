using System;
using System.Collections;
using System.Collections.Generic;
using GoomerScripts;
using UnityEngine;
using Pathfinding;
using Photon.Pun;
using System.IO;

public class ConstructionManager : MonoBehaviourPunCallbacks
{
    public static ConstructionManager instance;
    public TowerPlacer[] towerPlacers;

    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void ApplyOwnershipToTowerPlacers()
    {
        for (int i = 0; i < towerPlacers.Length; i++)
        {
            TowerPlacer towerPlacer = towerPlacers[i].GetComponent<TowerPlacer>();

            switch (towerPlacer.towerPlacerOwner)
            {
                case TowerPlacer.TowerPlacerOwner.Player1:
                    towerPlacer.Owner = GameManager.instance.playersInGame[0];
                    break;
                case TowerPlacer.TowerPlacerOwner.Player2:
                    towerPlacer.Owner = GameManager.instance.playersInGame[1];
                    break;
                case TowerPlacer.TowerPlacerOwner.Player3:
                    towerPlacer.Owner = GameManager.instance.playersInGame[2];
                    break;
                case TowerPlacer.TowerPlacerOwner.Player4:
                    towerPlacer.Owner = GameManager.instance.playersInGame[3];
                    break;
                case TowerPlacer.TowerPlacerOwner.Player5:
                    towerPlacer.Owner = GameManager.instance.playersInGame[4];
                    break;
            }

            Debug.Log("AppliedTowerPlacersOwnership");
        }

    }
}
