using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPropTest : MonoBehaviourPunCallbacks
{
    private ExitGames.Client.Photon.Hashtable playerCustomProperties = new ExitGames.Client.Photon.Hashtable();

    void Update()
    {
        if (!photonView.IsMine) return;

        SetPing();
        ShowPing();
    }

    private void SetPing()
    {
        playerCustomProperties["Ping"] = PhotonNetwork.GetPing();
        PhotonNetwork.LocalPlayer.CustomProperties = playerCustomProperties;
    }

    private void ShowPing()
    {
        int timer = (int)PhotonNetwork.LocalPlayer.CustomProperties["Ping"];
        Debug.Log(timer + ", " + PhotonNetwork.LocalPlayer.NickName);
    }

}
