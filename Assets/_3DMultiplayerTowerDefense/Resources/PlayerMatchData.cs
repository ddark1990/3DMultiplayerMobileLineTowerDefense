using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMatchData : MonoBehaviourPunCallbacks
{
    public int PlayerLives = 50;
    public int PlayerGold = 25;
    public int PlayerIncome = 5;

    public float IncomeTimer = 10;

    private float _startIncomeTimer;

    private void Start()
    {
        ClampPlayerData();

        _startIncomeTimer = IncomeTimer;

        StartCoroutine(PlayerLostAnnounce());
    }

    private void Update()
    {
        IncomeTime();
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

    [PunRPC]
    private void RPC_SendPlayerLives()
    {
        Debug.Log(photonView.Owner.NickName + " has " + PlayerLives + " lives. Match is over.");
    }

    private void IncomeTime()
    {
        IncomeTimer -= Time.deltaTime;

        if (!(IncomeTimer <= 0)) return;

        IncreaseGold();
        IncomeTimer = _startIncomeTimer;
    }

    private void IncreaseGold()
    {
        PlayerGold += PlayerIncome;
    }
}
