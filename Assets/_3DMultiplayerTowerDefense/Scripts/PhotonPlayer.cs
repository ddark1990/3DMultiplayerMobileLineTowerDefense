using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using GoomerScripts;
using System.IO;
using UnityEngine.SceneManagement;

public class PhotonPlayer : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback, IComparable<PhotonPlayer>
{
    public Camera PlayerCam;
    public int PlayerNumber;
    public bool PlayerReady;

    public PlayerMatchData PlayerData;

    public int currentScene;
    public int _gameScene;

    public bool PlayerLoaded;

    private ExitGames.Client.Photon.Hashtable playerCustomProperties = new ExitGames.Client.Photon.Hashtable();


    private void Start()
    {
        photonView.RPC("RPC_SendPlayerData", RpcTarget.Others); //send own player data across network for others to see

        _gameScene = SceneManager.GetSceneByName("GameScene").buildIndex;

        if (photonView.IsMine)
        {
            PlayerCam = FindObjectOfType<Camera>();
            SelectionManager.Instance.cam = PlayerCam.GetComponent<Camera>();
        }
    }

    [PunRPC]
    private void RPC_SendPlayerData()  // fix all dis shit
    {
        var _gameMan = GameManager.instance;

        PlayerNumber = photonView.Owner.ActorNumber;

        gameObject.name += " " + GetComponent<PhotonView>().Owner.NickName;
        
        if (PlayerCam != null)
        {
            if (PhotonNetwork.IsMasterClient) //temp spawn data
            {
                PlayerCam.transform.position = GameManager.instance.playerSpawns[0].position;
                PlayerCam.transform.rotation = GameManager.instance.playerSpawns[0].rotation;
            }
            else
            {
                PlayerCam.transform.position = GameManager.instance.playerSpawns[1].position;
                PlayerCam.transform.rotation = GameManager.instance.playerSpawns[1].rotation;
            }
        }

        _gameMan.playersInGame.Add(this);

        Debug.Log("SendingPlayerData");
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
