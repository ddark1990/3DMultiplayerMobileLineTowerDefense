using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using GoomerScripts;
using System.IO;

public class PhotonPlayer : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback, IComparable<PhotonPlayer>
{
    public Camera PlayerCam;
    public int PlayerNumber;
    public bool PlayerReady;

    public PlayerMatchData PlayerData;

    private void Start()
    {
        photonView.RPC("RPC_SendPlayerData", RpcTarget.Others); //send own player data across network for others to see

        if (photonView.IsMine)
        {
            PlayerCam = FindObjectOfType<Camera>();
            SelectionManager.Instance.cam = PlayerCam.GetComponent<Camera>();
        }
    }

    [PunRPC]
    private void RPC_SendPlayerData()  // fix all dis shit
    {
        var gameMan = GameManager.instance;

        Debug.Log("SendingPlayerData");

        PlayerNumber = photonView.Owner.ActorNumber;

        gameMan.playerCount++;
        gameMan.playersInGame.Add(this);

        gameObject.name += " " + GetComponent<PhotonView>().Owner.NickName;
        
        PlayerReady = true;

        if (PlayerCam != null)
        {
            if (PhotonNetwork.IsMasterClient) //temp spawn data
            {
                //GameManager.instance.playerSpawns[0].GetComponent<ScrollAndPinch>().Camera = PlayerCam;
                PlayerCam.transform.position = GameManager.instance.playerSpawns[0].position;
                PlayerCam.transform.rotation = GameManager.instance.playerSpawns[0].rotation;
            }
            else
            {
                //GameManager.instance.playerSpawns[1].GetComponent<ScrollAndPinch>().Camera = PlayerCam;
                PlayerCam.transform.position = GameManager.instance.playerSpawns[1].position;
                PlayerCam.transform.rotation = GameManager.instance.playerSpawns[1].rotation;
            }
        }
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        Debug.Log(info);
    }

    public int CompareTo(PhotonPlayer other)
    {
        if (this.PlayerNumber > other.PlayerNumber)
            return 1;
        else if (this.PlayerNumber < other.PlayerNumber)
            return -1;
        else
            return 0;
    }
}
