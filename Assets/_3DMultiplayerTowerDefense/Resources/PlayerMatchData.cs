using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerMatchData : MonoBehaviourPunCallbacks
{
    public int PlayerLives, IncomeTime, PlayerGold, PlayerIncome;
    private int startIncomeTimer;

    private bool IncomeTimerStarted, PlayerLost;
    
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
        StartCoroutine(IncomeTimer());
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

    private IEnumerator IncomeTimer() //create a sync method to check if the income timers are off between players and fix them based on the master client as a reference, must check at start of match and at certain intervals to limit bandwith use
    {
        yield return new WaitUntil(() => GameManager.instance.MatchStarted);

        startIncomeTimer = (int)PhotonNetwork.CurrentRoom.CustomProperties[START_INCOME_TIMER];

        while (!GameManager.instance.MatchEnd || !PlayerLost)
        {
            IncomeTime--;

            if ((IncomeTime <= -1))
            {
                IncomeTime = startIncomeTimer;

                if(photonView.IsMine)
                    photonView.RPC("RPC_IncreaseGold", RpcTarget.AllViaServer);
            }

            yield return new WaitForSeconds(1f);
        }
    }

    /// <summary> Deduct player's lives with clamp. </summary>
    public void DeductPlayerLives() {
        if (PlayerLost || GameManager.instance.MatchEnd) return;

        PlayerLives--;
        if(PlayerLives <= 0)
            PlayerLives = 0;
    }

    //player lost logic
    #region PlayerLostAnnounce

    private IEnumerator PlayerLostAnnounce()
    {
        yield return new WaitUntil(() => photonView.IsMine && PlayerLives.Equals(0));

        var matchDataInterface = GetMatchDataInterface();
        matchDataInterface.IfPlayerHasZeroLives(_player);
    }

    [PunRPC]
    public void RPC_SendPlayerLost() //Sends over network that this player has lost when has 0 lives
    {
        Debug.Log(photonView.Owner.NickName + " has " + PlayerLives + " lives.");
        PlayerLost = true;
    }

    #endregion

    [PunRPC]
    public void RPC_IncreaseGold() 
    {
        PlayerGold += PlayerIncome;
    }

    private IPlayerMatchData GetMatchDataInterface()
    {
        return GetComponent<IPlayerMatchData>();
    }
}
