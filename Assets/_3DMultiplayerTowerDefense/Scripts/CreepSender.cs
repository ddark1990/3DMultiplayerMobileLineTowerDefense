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

    public PhotonPlayer Owner;
    public bool IsSelectable;

    private PlayerMatchData _ownerMatchData;

    public new void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += SendCreep_EventReceived;
        StartCoroutine(GetOwnerMatchData());
    }

    public new void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= SendCreep_EventReceived;
    }

    private IEnumerator GetOwnerMatchData()
    {
        yield return new WaitUntil(() => Owner != null);

        _ownerMatchData = Owner.GetComponent<PlayerMatchData>();
    }

    public void SetBuildingSelectability()
    {
        if (PhotonNetwork.LocalPlayer != Owner.photonView.Owner) return;

        IsSelectable = true;
        photonView.TransferOwnership(Owner.photonView.Owner);
    }

    private void SendCreep_EventReceived(EventData obj) //network clients receive the data and execute it on their end
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

                SetCreepInfo(objToSpawn, (int)data[0], Owner);
                InterfaceInfo(objToSpawn);
            }
        }
    }

    public GameObject SendCreep(string poolName, Vector3 pos, Quaternion rot) //sent from local client to network
    {
        var objToSpawn = PoolManager.Instance.SpawnFromPool(poolName, pos, rot);

        SetCreepInfo(objToSpawn, photonView.ViewID, Owner);
        InterfaceInfo(objToSpawn);

        object[] sendCreepData = new object[] { photonView.ViewID, poolName, pos, rot };

        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.Others
        };

        PhotonNetwork.RaiseEvent((byte)EventIdHandler.EVENT_IDs.SEND_CREEP_EVENT, sendCreepData, options, SendOptions.SendUnreliable);

        return objToSpawn;
    }

    private void SetCreepInfo(GameObject obj, int id, PhotonPlayer player)
    {
        var destination = obj.GetComponent<AIDestinationSetter>();

        foreach (var goal in ListManager.instance.goals)
        {
            if (!goal.Owner.Equals(Owner))
            {
                destination.target = goal.transform; //set creeps correct destination
            }
        }

        var creep = obj.GetComponent<Creep>();

        creep.SenderViewId = id;
        creep.Owner = player;

    }

    private static void InterfaceInfo(GameObject obj) 
    {
        var creep = obj.GetComponent<Creep>();

        GetSentCreep(creep).CreepSent(obj);
    }

    private static ICreepSender GetSentCreep(Creep creep)
    {
        return creep.GetComponent<ICreepSender>();
    }

    [PunRPC]
    public void RPC_BuyCreep(int creepCost, int creepIncome) 
    {
        if (_ownerMatchData.PlayerGold < creepCost)
        {
            Debug.Log(Owner.PlayerName + " can't afford creep.");
            return;
        }

        _ownerMatchData.PlayerGold -= creepCost;

        photonView.RPC("RPC_UpdatePlayerIncome", RpcTarget.AllViaServer, creepIncome);
    }

    [PunRPC]
    public void RPC_UpdatePlayerIncome(int income) //owner defines who to send correctly
    {
        _ownerMatchData.PlayerIncome += income;
    }

}
