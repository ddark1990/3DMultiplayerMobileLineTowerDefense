﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PhotonConnectionInfo : MonoBehaviourPunCallbacks
{
    public static PhotonConnectionInfo instance;

    public GameObject topInfoCanvas;
    public GameObject topInfoPanel;
    public TextMeshProUGUI conectionStatusText;
    public TextMeshProUGUI pingText;
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI regionText;
    public TextMeshProUGUI AppVersionText;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
    }

    private void Start()
    {
        topInfoCanvas.SetActive(false);
        InvokeRepeating("UpdateConnectionInfo", 0.5f, 0.25f);
    }

    void UpdateConnectionInfo()
    {
        conectionStatusText.text = PhotonNetwork.NetworkClientState.ToString();
        pingText.text = PhotonNetwork.GetPing().ToString();
    }

    public override void OnConnected()
    {
        topInfoCanvas.SetActive(true);
        iTween.ScaleTo(topInfoPanel.gameObject, new Vector3(1f, 1f, 1f), .5f);
        regionText.text = PhotonNetwork.CloudRegion;
        playerNameText.text = PhotonNetwork.NickName;
        AppVersionText.text = PhotonNetwork.AppVersion;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        iTween.ScaleTo(topInfoPanel.gameObject, new Vector3(0f, 0f, 0f), .5f);
        //topInfoCanvas.SetActive(false);
    }
}