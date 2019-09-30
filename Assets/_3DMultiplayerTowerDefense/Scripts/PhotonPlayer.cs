using UnityEngine;
using Photon.Pun;
using System;
using GoomerScripts;

public class PhotonPlayer : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback, IComparable<PhotonPlayer>
{
    public Camera PlayerCam;
    public int PlayerNumber;
    public bool PlayerReady;
    public string PlayerName;

    public PlayerMatchData PlayerData;

    public int currentScene, _gameScene;

    private ExitGames.Client.Photon.Hashtable playerCustomProperties = new ExitGames.Client.Photon.Hashtable();


    private new void OnEnable()
    {
        if (!photonView.IsMine) return;

        PlayerCam = FindObjectOfType<Camera>();
        SelectionManager.Instance.cam = PlayerCam;

        photonView.RPC("RPC_SendPlayerData", RpcTarget.All); //send own player data across network for others to see
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

        Debug.Log("SendingPlayerData for: " + PlayerName);
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
