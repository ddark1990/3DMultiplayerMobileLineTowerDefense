using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerMatchData : MonoBehaviourPunCallbacks
{
    public int PlayerLives, IncomeTimer, PlayerGold, PlayerIncome;
    public int startIncomeTimer;

    private bool IncomeTimerStarted;
    
    public const string START_INCOME_TIMER = "StartIncomeTimer";

    private PhotonPlayer _player;

    private new void OnEnable()
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
        PlayerLives = 50;
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
        yield return new WaitUntil(() => GameManager.instance.MatchStarted);

        startIncomeTimer = (int)PhotonNetwork.CurrentRoom.CustomProperties[START_INCOME_TIMER];

        while (true)
        {
            IncomeTimer--;

            if ((IncomeTimer <= -1) && photonView.IsMine)
            {
                photonView.RPC("RPC_IncreaseGold", RpcTarget.AllViaServer);
                IncomeTimer = startIncomeTimer;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    [PunRPC]
    public void RPC_SendPlayerLives() //initiate end of match, from game manager
    {
        Debug.Log(photonView.Owner.NickName + " has " + PlayerLives + " lives. Match is over.");
    }

    [PunRPC]
    public void RPC_IncreaseGold() 
    {
        PlayerGold += PlayerIncome;
    }
}
