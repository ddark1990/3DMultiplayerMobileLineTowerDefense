using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Pathfinding;

namespace MatchSystem
{
    public class CreepSender : MonoBehaviourPunCallbacks
    {
        public NetworkPlayer NetworkOwner;
        public List<SpawnableNPC> Creeps;

        public new void OnEnable()
        {
            Creeps.Sort();
            PhotonNetwork.NetworkingClient.EventReceived += SendCreep_EventReceived;
        }
        public new void OnDisable()
        {
            PhotonNetwork.NetworkingClient.EventReceived -= SendCreep_EventReceived;
        }

        //Network Events
        public GameObject SendCreep(string poolName, Vector3 pos, Quaternion rot) //sent from local client to network
        {
            var objToSpawn = PoolManager.Instance.SpawnFromPool(poolName, pos, rot);

            SetCreepInfo(objToSpawn, photonView.ViewID, NetworkOwner);
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

                    SetCreepInfo(objToSpawn, (int)data[0], NetworkOwner);
                    InterfaceInfo(objToSpawn);
                }
            }
        }

        //Creep Network Data
        private void SetCreepInfo(GameObject obj, int id, NetworkPlayer player)
        {
            var destination = obj.GetComponent<AIDestinationSetter>();

            foreach (var goal in ListManager.instance.goals)
            {
                if (!goal.Owner.Equals(NetworkOwner))
                {
                    destination.target = goal.transform; //set creeps correct destination
                }
            }

            var creep = obj.GetComponent<Creep>();

            creep.SenderViewId = id;
            creep.NetworkOwner = player;
        }
        private void InterfaceInfo(GameObject obj)
        {
            var creep = obj.GetComponent<Creep>();

            GetSentCreep(creep).CreepSent(obj);
        }
        private static ICreepSender GetSentCreep(Creep creep)
        {
            return creep.GetComponent<ICreepSender>();
        }

        //[PunRPC]
        //public void RPC_BuyCreep(int creepCost, int creepIncome)
        //{
        //    if (_ownerMatchData.PlayerGold < creepCost)
        //    {
        //        Debug.Log(Owner.PlayerName + " can't afford creep.");
        //        return;
        //    }

        //    _ownerMatchData.PlayerGold -= creepCost;
        //    _ownerMatchData.CreepsSent++;
        //    UpdatePlayerIncome(creepIncome);
        //}

        //public void UpdatePlayerIncome(int income) //owner defines who to send correctly
        //{
        //    _ownerMatchData.PlayerIncome += income;
        //}

    }
}