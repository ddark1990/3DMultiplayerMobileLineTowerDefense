using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPropTest : MonoBehaviour
{
    private ExitGames.Client.Photon.Hashtable playerCustomProperties = new ExitGames.Client.Photon.Hashtable();

    void Update()
    {
        SetPing();
        ShowPing();
    }

    private void SetPing()
    {
        //playerCustomProperties["Timer"] = GameManager.instance.IncomeTimer;
        //PhotonNetwork.LocalPlayer.CustomProperties = playerCustomProperties;
    }

    private void ShowPing()
    {
        float timer = (float)PhotonNetwork.LocalPlayer.CustomProperties["Timer"];
        Debug.Log(timer);
    }

}
