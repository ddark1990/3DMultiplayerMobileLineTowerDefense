using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

namespace MatchSystem
{
    public class PlayerHudUi : MonoBehaviour
    {
        public TextMeshProUGUI TimerText;
        public TextMeshProUGUI IncomeText;
        public TextMeshProUGUI GoldText;
        public TextMeshProUGUI LivesText;

        public NetworkPlayer NetworkPlayer;
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

        private void Update() //move into a coroutine or interface to fix GC aloc
        {
            IncomeText.text = PlayerData.PlayerIncome.ToString();
            TimerText.text = PlayerData.IncomeTime.ToString("#");
            GoldText.text = PlayerData.PlayerGold.ToString();
            LivesText.text = PlayerData.PlayerLives.ToString();
        }

    }
}