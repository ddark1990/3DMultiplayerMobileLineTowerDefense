using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class TopRightUI : MonoBehaviour
{
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI IncomeText;
    public TextMeshProUGUI GoldText;
    public TextMeshProUGUI LivesText;

    public PlayerMatchData PlayerData;
    public GameObject HudCanvas;

    private void Start()
    {
        var pv = GetComponentInParent<PhotonView>();

        if (!pv.IsMine)
        {
            HudCanvas.SetActive(false);
        }
    }

    private void Update()
    {
        IncomeText.text = PlayerData.PlayerIncome.ToString();
        TimerText.text = PlayerData.IncomeTimer.ToString("#");
        GoldText.text = PlayerData.PlayerGold.ToString();
        LivesText.text = PlayerData.PlayerLives.ToString();
    }
}
