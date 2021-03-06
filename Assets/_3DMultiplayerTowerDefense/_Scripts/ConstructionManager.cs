﻿using System;
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
                    towerPlacer.Owner = GameManager.instance.PlayersInGame[0];
                    break;
                case TowerPlacer.TowerPlacerOwner.Player2:
                    towerPlacer.Owner = GameManager.instance.PlayersInGame[1];
                    break;
                case TowerPlacer.TowerPlacerOwner.Player3:
                    towerPlacer.Owner = GameManager.instance.PlayersInGame[2];
                    break;
                case TowerPlacer.TowerPlacerOwner.Player4:
                    towerPlacer.Owner = GameManager.instance.PlayersInGame[3];
                    break;
                case TowerPlacer.TowerPlacerOwner.Player5:
                    towerPlacer.Owner = GameManager.instance.PlayersInGame[4];
                    break;
            }

            Debug.Log("AppliedTowerPlacersOwnership");
        }

        //for (int i = 0; i < GameManager.instance.playersInGame.Count; i++)
        //{
        //    var player = GameManager.instance.playersInGame[i];

        //    foreach (var towerPlacer in towerPlacers)
        //    {
        //        switch (towerPlacer.towerPlacerOwner)
        //        {
        //            case TowerPlacer.TowerPlacerOwner.Player1:
        //                towerPlacer.Owner = GameManager.instance.playersInGame[player.PlayerNumber - 1];
        //                break;
        //            case TowerPlacer.TowerPlacerOwner.Player2:
        //                towerPlacer.Owner = GameManager.instance.playersInGame[player.PlayerNumber - 1];
        //                break;
        //            case TowerPlacer.TowerPlacerOwner.Player3:
        //                towerPlacer.Owner = GameManager.instance.playersInGame[player.PlayerNumber - 1];
        //                break;
        //            case TowerPlacer.TowerPlacerOwner.Player4:
        //                towerPlacer.Owner = GameManager.instance.playersInGame[player.PlayerNumber - 1];
        //                break;
        //            case TowerPlacer.TowerPlacerOwner.Player5:
        //                towerPlacer.Owner = GameManager.instance.playersInGame[player.PlayerNumber - 1];
        //                break;
        //        }

        //        Debug.Log("AppliedTowerPlacersOwnership");
        //    }
        //}
    }
}
