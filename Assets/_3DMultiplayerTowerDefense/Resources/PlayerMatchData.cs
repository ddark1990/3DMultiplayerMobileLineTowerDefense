using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PlayerMatchData : MonoBehaviourPunCallbacks
{
    public int PlayerLives = 50;
    public int PlayerGold = 20;
    public int PlayerIncome = 5;
    public int IncomeTimer = 10;

    private const string TIMER = "Timer";

    private PhotonPlayer _player;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _player = GetComponent<PhotonPlayer>();
        ClampPlayerData();
        StartingPlayerData();

        StartCoroutine(PlayerLostAnnounce());
        StartCoroutine(Timer());
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

    private IEnumerator Timer() //grab start time of the game managers timer and decrement based off that once all players are loaded for each client
    {
        int netTimer = (int)PhotonNetwork.CurrentRoom.CustomProperties[TIMER];
        IncomeTimer = netTimer;

        yield return new WaitUntil(() => GameManager.instance.AllPlayersReady);

        while (true)
        {
            if ((IncomeTimer <= 0) && photonView.IsMine)
                photonView.RPC("RPC_IncreaseGold", RpcTarget.AllViaServer);

            IncomeTimer--;

            if ((IncomeTimer <= -1))
                IncomeTimer = netTimer;

            yield return new WaitForSeconds(1f);
        }
    }

    [PunRPC]
    public void RPC_SendPlayerLives() //initiate end of match, from game manager
    {
        Debug.Log(photonView.Owner.NickName + " has " + PlayerLives + " lives. Match is over.");
    }

    [PunRPC]
    public void RPC_IncreaseGold() //must be controlled by game manager
    {
        PlayerGold += PlayerIncome;
    }
}
