using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Pathfinding;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class CreepSender : MonoBehaviourPunCallbacks
{
    public enum CreepSenderOwner { Player1, Player2, Player3, Player4, Player5, Player6, Player7, Player8 };
    public CreepSenderOwner creepSenderOwner;

    public PhotonPlayer owner;
    public bool IsSelectable;

    public new void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += SendCreep_EventReceived;
    }

    public new void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= SendCreep_EventReceived;
    }

    public void SetBuildingSelectability()
    {
        if (PhotonNetwork.LocalPlayer != owner.photonView.Owner) return;

        IsSelectable = true;
        photonView.TransferOwnership(owner.photonView.Owner);
    }

    private void SendCreep_EventReceived(EventData obj)
    {
        if (obj.Code == (byte)EventIdHandler.EVENT_IDs.SEND_CREEP_EVENT)
        {
            object[] data = (object[])obj.CustomData;

            if ((int)data[0] == photonView.ViewID)
            {
                var poolName = (string)data[1];
                var pos = (Vector3)data[2];
                var rot = (Quaternion)data[3];

                var objToSpawn = PoolManager.Instance.SpawnFromPool(poolName, pos, rot);

                SetCreepDestination(objToSpawn);
            }
        }
    }

    public GameObject SendCreep(string poolName, Vector3 pos, Quaternion rot)
    {
        var objToSpawn = PoolManager.Instance.SpawnFromPool(poolName, pos, rot);

        SetCreepDestination(objToSpawn);

        object[] sendCreepData = new object[] { photonView.ViewID, poolName, pos, rot };

        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.Others
        };

        PhotonNetwork.RaiseEvent((byte)EventIdHandler.EVENT_IDs.SEND_CREEP_EVENT, sendCreepData, options, SendOptions.SendUnreliable);

        return objToSpawn;
    }

    private void SetCreepDestination(GameObject obj)
    {
        var destination = obj.GetComponent<AIDestinationSetter>();

        foreach (var goal in ListManager.instance.goals)
        {
            if (!goal.owner.Equals(owner))
            {
                destination.target = goal.transform;
            }
        }
    }
}
