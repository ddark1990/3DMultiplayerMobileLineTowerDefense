using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMatchData : MonoBehaviourPunCallbacks
{
    public int PlayerLives = 50;
    public int PlayerGold = 20;
    public int PlayerIncome = 5;
    public int IncomeTimer = 10;

    private const string TIMER = "Timer";

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        ClampPlayerData();

        StartCoroutine(PlayerLostAnnounce());
        StartCoroutine(Timer());

        StartingPlayerData();
    }

    private void StartingPlayerData()
    {
        PlayerGold = 20;
        PlayerIncome = 5;
    }

    private void ClampPlayerData()
    {
        PlayerLives = Mathf.Clamp(PlayerLives, 0, 100);
        PlayerGold = Mathf.Clamp(PlayerGold, 0, 1000000); 
        PlayerIncome = Mathf.Clamp(PlayerIncome, 0, 1000000);
    }

    private IEnumerator PlayerLostAnnounce()
    {
        yield return new WaitUntil(() => photonView.IsMine && PlayerLives.Equals(0));

        photonView.RPC("RPC_SendPlayerLives", RpcTarget.AllViaServer);
    }

    private IEnumerator Timer()
    {
        yield return new WaitUntil(() => GameManager.instance.allPlayersLoaded);

        while (true)
        {
            int netTimer = (int)PhotonNetwork.CurrentRoom.CustomProperties[TIMER];

            IncomeTimer = netTimer;

            yield return new WaitForSeconds(1f);
        }
    }

    [PunRPC]
    public void RPC_SendPlayerLives() //initiate end of match
    {
        Debug.Log(photonView.Owner.NickName + " has " + PlayerLives + " lives. Match is over.");
    }

    [PunRPC]
    public void IncreaseGold()
    {
        if (!photonView.IsMine) return;
        PlayerGold += PlayerIncome;
    }
}
