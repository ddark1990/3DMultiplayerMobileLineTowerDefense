using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using GoomerScripts;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class TowerPlacer : MonoBehaviourPunCallbacks
{
    public enum TowerPlacerOwner { Player1, Player2, Player3, Player4, Player5, Player6, Player7, Player8 };
    public TowerPlacerOwner towerPlacerOwner;

    public PhotonPlayer Owner;

    private void Start()
    {
        StartCoroutine(SetPhotonOwnerShip());
    }

    public new void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += PlaceTower_EventReceived;
    }

    public new void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= PlaceTower_EventReceived;
    }

    IEnumerator SetPhotonOwnerShip()
    {
        yield return new WaitUntil(() => GameManager.instance.AllPlayersReady);

        if (Equals(PhotonNetwork.LocalPlayer, Owner.photonView.Owner))
        {
            photonView.TransferOwnership(Owner.photonView.Owner);
        }
    }

    private void PlaceTower_EventReceived(EventData obj)
    {
        if (obj.Code == (byte)EventIdHandler.EVENT_IDs.PLACE_TOWER_EVENT)
        {
            object[] data = (object[])obj.CustomData;

            if ((int)data[0] == photonView.ViewID)
            {
                string tag = (string)data[1];
                Vector3 pos = (Vector3)data[2];
                Quaternion rot = (Quaternion)data[3];

                var objToSpawn = PoolManager.Instance.SpawnFromPool(tag, pos, rot); //plays over the network for others with the data from the object array
            }
        }
    }

    public GameObject PlaceTower(string tag, Vector3 pos, Quaternion rot)
    {
        var node = SelectionManager.Instance.currentlySelectedObject.GetComponent<Node>(); 

        if(tag == null)Debug.LogWarning(tag + " is not found!");

        if (node.isOccupied)
        {
            Debug.Log(node + " is occupied.");
            return null;
        }

        var objToSpawn = PoolManager.Instance.SpawnFromPool(tag, pos, rot);

        object[] towerPlaceData = new object[] { photonView.ViewID, tag, pos, rot };

        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.Others
        };

        PhotonNetwork.RaiseEvent((byte)EventIdHandler.EVENT_IDs.PLACE_TOWER_EVENT, towerPlaceData, options, SendOptions.SendReliable);

        node.isOccupied = true;

        return objToSpawn;
    }

    [PunRPC]
    public void BuyTower(int towerCost)
    {
        Owner.GetComponent<PlayerMatchData>().PlayerGold -= towerCost;
        Owner.GetComponent<PlayerMatchData>().TowersBuilt++;
    }
}
