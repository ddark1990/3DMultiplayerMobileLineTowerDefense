using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatchSystem
{
    public class TowerPlacer : MonoBehaviourPunCallbacks
    {
        public NetworkPlayer NetworkOwner;
        public List<PlaceableTower> Towers;

        public new void OnEnable()
        {
            Towers.Sort();
            PhotonNetwork.NetworkingClient.EventReceived += PlaceTower_EventReceived;
        }
        public new void OnDisable()
        {
            PhotonNetwork.NetworkingClient.EventReceived -= PlaceTower_EventReceived;
        }

        //Network Events
        public GameObject PlaceTower(string tag, Vector3 pos, Quaternion rot)//sent from local client to network
        {
            var node = SelectionManager.Instance.CurrentlySelectedObject.GetComponent<Node>();

            if (tag == null) Debug.LogWarning(tag + " is not found!");

            if (node.IsOccupied)
            {
                Debug.Log(node + " is occupied.");
                return null;
            }

            var objToSpawn = PoolManager.Instance.SpawnFromPool(tag, pos, rot);

            object[] towerPlaceData = new object[] { NetworkOwner.photonView.ViewID, tag, pos, rot };

            RaiseEventOptions options = new RaiseEventOptions()
            {
                CachingOption = EventCaching.DoNotCache,
                Receivers = ReceiverGroup.Others
            };

            PhotonNetwork.RaiseEvent((byte)EventIdHandler.EVENT_IDs.PLACE_TOWER_EVENT, towerPlaceData, options, SendOptions.SendReliable);

            node.IsOccupied = true;

            return objToSpawn;
        }
        private void PlaceTower_EventReceived(EventData obj)//network clients receive the data and execute it on their end
        {
            if (obj.Code == (byte)EventIdHandler.EVENT_IDs.PLACE_TOWER_EVENT)
            {
                object[] data = (object[])obj.CustomData;

                if ((int)data[0] == NetworkOwner.photonView.ViewID)
                {
                    string tag = (string)data[1];
                    Vector3 pos = (Vector3)data[2];
                    Quaternion rot = (Quaternion)data[3];

                    var objToSpawn = PoolManager.Instance.SpawnFromPool(tag, pos, rot); //plays over the network for others with the data from the object array
                }
            }
        }

        //[PunRPC]
        //public void BuyTower(int towerCost)
        //{
        //    Owner.GetComponent<PlayerMatchData>().PlayerGold -= towerCost;
        //    Owner.GetComponent<PlayerMatchData>().TowersBuilt++;
        //}

    }
}