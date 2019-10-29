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
        public Transform CreepSpawnPoint, CreepDestination;

        public PlayerMatchData PlayerMatchData;

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
        public GameObject SendCreep(string poolName) //sent from local client to network
        {
            var objToSpawn = PoolManager.Instance.SpawnFromPool(poolName, CreepSpawnPoint.position, CreepSpawnPoint.rotation );

            if (!PlayerMatchData.CanAfford(objToSpawn.GetComponent<Creep>().CreepCost))
            {
                if(PlayerMatchData.DebugLogNetworkEvents)
                {
                    Debug.Log(NetworkOwner.PlayerName + " does not have enough gold!");
                }

                PoolManager.Instance.ReturnToPool(objToSpawn);
                return null;
            }

            SetCreepInfo(objToSpawn, NetworkOwner.photonView.ViewID, NetworkOwner);
            InterfaceInfo(objToSpawn);

            PlayerMatchData.IncreasePlayerIncome_Event(objToSpawn.GetComponent<Creep>().Income);
            PlayerMatchData.DeductPlayerGold_Event(objToSpawn.GetComponent<Creep>().CreepCost);

            object[] sendCreepData = new object[] { NetworkOwner.photonView.ViewID, poolName, CreepSpawnPoint.position, CreepSpawnPoint.rotation };

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

                if ((int)data[0] == NetworkOwner.photonView.ViewID)
                {
                    var poolName = (string)data[1];
                    var pos = (Vector3)data[2];
                    var rot = (Quaternion)data[3];

                    var objToSpawn = PoolManager.Instance.SpawnFromPool(poolName, pos, rot);

                    SetCreepInfo(objToSpawn, (int)data[0], NetworkOwner);
                    InterfaceInfo(objToSpawn);

                    PlayerMatchData.IncreasePlayerIncome_Event(objToSpawn.GetComponent<Creep>().Income);
                }
            }
        }

        //Creep Network Data
        private void SetCreepInfo(GameObject obj, int id, NetworkPlayer player)
        {
            var creep = obj.GetComponent<Creep>();

            creep.SenderViewId = id;
            creep.NetworkOwner = player;

            var destination = obj.GetComponent<AIDestinationSetter>();

            destination.target = CreepDestination; //set creeps correct destination

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