﻿using UnityEngine;
using Photon.Pun;
using System;
using GoomerScripts;
using System.Collections;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class PhotonPlayer : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback, IComparable<PhotonPlayer>
{
    public Camera PlayerCam;
    public int PlayerNumber;
    public bool PlayerReady, PoolsLoaded, OwnershipApplied;
    public string PlayerName;

    public PlayerMatchData PlayerData;

    public int currentScene, _gameScene;

    private ExitGames.Client.Photon.Hashtable playerCustomProperties = new ExitGames.Client.Photon.Hashtable();


    private new void OnEnable()
    {
        if (!photonView.IsMine) return;

        StartCoroutine(CheckIfPoolsLoaded());
        StartCoroutine(CheckIfOwnershipLoaded());
        StartCoroutine(SetPlayerReady());

        PlayerCam = FindObjectOfType<Camera>();
        SelectionManager.Instance.cam = PlayerCam;

        photonView.RPC("RPC_SendPlayerData", RpcTarget.AllViaServer); //send own player data across network for others to see
    }

    [PunRPC]
    public void RPC_SendPlayerData()  // fix all dis shit
    {
        var _gameMan = GameManager.instance;

        PlayerNumber = photonView.Owner.ActorNumber;
        PlayerName = photonView.Owner.NickName;

        gameObject.name += " " + GetComponent<PhotonView>().Owner.NickName;

        _gameMan.playersInGame.Add(this);
        _gameMan.playerCount++;

        //playerCustomProperties["PlayerReady"] = PlayerReady;
        //PhotonNetwork.LocalPlayer.SetCustomProperties(playerCustomProperties);

        if (PlayerCam != null)
        {
            PlayerCam.transform.position = GameManager.instance.playerSpawns[PlayerNumber - 1].position;
            PlayerCam.transform.rotation = GameManager.instance.playerSpawns[PlayerNumber - 1].rotation;
        }

        PlayerReadyUI.Instance.PopulateInfo(this);

        //Debug.Log(PlayerName + " is ready!");

        Debug.Log("SendingPlayerData for: " + PlayerName);
    }

    private IEnumerator CheckIfPoolsLoaded() 
    {
        yield return new WaitUntil(() => PoolManager.Instance.PoolsLoaded);

        PoolsLoaded = true;
    }

    private IEnumerator CheckIfOwnershipLoaded() 
    {
        yield return new WaitUntil(() => GameManager.instance.PlayerOwnershipApplied);

        OwnershipApplied = true;
    }

    private IEnumerator SetPlayerReady() //setting it for each other right now, need to set themselves
    {
        yield return new WaitUntil(() => PoolsLoaded && OwnershipApplied);
        yield return new WaitForSeconds(6); //buffer

        photonView.RPC("RPC_ReadyPlayer", RpcTarget.All);
    }
    [PunRPC]
    private void RPC_ReadyPlayer()
    {
        PlayerReady = true;
        Debug.Log(PlayerName + " is ready!");
    }

    public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable changedProps)
    {
        Debug.Log(target + " | " + changedProps);
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        //Debug.Log(info);
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
