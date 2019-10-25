using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using GooglePlayGames;

public class ConnectionInfoPanel : MonoBehaviourPunCallbacks
{
    public static ConnectionInfoPanel instance;

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
        InvokeRepeating("UpdateConnectionInfo", 1f, 0.25f);
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
        //playerNameText.text = PhotonNetwork.NickName;
        AppVersionText.text = Application.version;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        iTween.ScaleTo(topInfoPanel.gameObject, new Vector3(0f, 0f, 0f), .5f);
        //topInfoCanvas.SetActive(false);
    }
}
