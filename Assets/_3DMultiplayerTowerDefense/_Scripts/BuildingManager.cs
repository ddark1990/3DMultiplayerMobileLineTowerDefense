using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager instance;

    public List<GameObject> buildings;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            //DontDestroyOnLoad(Instance);
        }
    }

    public void ApplyOwnershipToBuildings()
    {
        for (int i = 0; i < buildings.Count; i++)
        {
            CreepSender creepSender = buildings[i].GetComponent<CreepSender>();

            switch (creepSender.creepSenderOwner)
            {
                case CreepSender.CreepSenderOwner.Player1:
                    creepSender.Owner = GameManager.instance.playersInGame[0];
                    break;
                case CreepSender.CreepSenderOwner.Player2:
                    creepSender.Owner = GameManager.instance.playersInGame[1];
                    break;
                case CreepSender.CreepSenderOwner.Player3:
                    creepSender.Owner = GameManager.instance.playersInGame[2];
                    break;
                case CreepSender.CreepSenderOwner.Player4:
                    creepSender.Owner = GameManager.instance.playersInGame[3];
                    break;
                case CreepSender.CreepSenderOwner.Player5:
                    creepSender.Owner = GameManager.instance.playersInGame[4];
                    break;
            }

            Debug.Log("AppliedBuildingOwnership");
        }

        //for (int i = 0; i < GameManager.instance.playersInGame.Count; i++)
        //{
        //    var player = GameManager.instance.playersInGame[i];

        //    foreach (var building in buildings)
        //    {
        //        var creepSender = building.GetComponent<CreepSender>();

        //        switch (creepSender.creepSenderOwner)
        //        {
        //            case CreepSender.CreepSenderOwner.Player1:
        //                creepSender.Owner = GameManager.instance.playersInGame[player.PlayerNumber - 1];
        //                break;
        //            case CreepSender.CreepSenderOwner.Player2:
        //                creepSender.Owner = GameManager.instance.playersInGame[player.PlayerNumber - 1];
        //                break;
        //            case CreepSender.CreepSenderOwner.Player3:
        //                creepSender.Owner = GameManager.instance.playersInGame[player.PlayerNumber - 1];
        //                break;
        //            case CreepSender.CreepSenderOwner.Player4:
        //                creepSender.Owner = GameManager.instance.playersInGame[player.PlayerNumber - 1];
        //                break;
        //            case CreepSender.CreepSenderOwner.Player5:
        //                creepSender.Owner = GameManager.instance.playersInGame[player.PlayerNumber - 1];
        //                break;
        //        }

        //        Debug.Log("AppliedBuildingOwnership");
        //    }
        //}
    }
}
