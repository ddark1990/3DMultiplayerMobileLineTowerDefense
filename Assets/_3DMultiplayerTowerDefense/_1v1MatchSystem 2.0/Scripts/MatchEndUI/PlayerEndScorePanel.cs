using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MatchSystem
{
    public class PlayerEndScorePanel : MonoBehaviour
    {
        public TextMeshProUGUI PlayerNameText;
        public TextMeshProUGUI LivesRemainingText;
        public TextMeshProUGUI IncomeEarnedText;
        public TextMeshProUGUI CreepsKilledText;
        public TextMeshProUGUI CreepsSentText;
        public TextMeshProUGUI TowersBuiltText;

        [HideInInspector] public NetworkPlayer NetworkOwner;
        [HideInInspector] public PlayerMatchData PlayerMatchData;

        private void Start()
        {
            StartCoroutine(RefreshInfo());
        }

        private IEnumerator RefreshInfo()
        {
            yield return new WaitUntil(() => NetworkOwner && PlayerMatchData);

            while(true)
            {
                PlayerNameText.text = NetworkOwner.PlayerName;
                LivesRemainingText.text = PlayerMatchData.PlayerLives.ToString();
                IncomeEarnedText.text = PlayerMatchData.PlayerIncome.ToString();
                CreepsKilledText.text = PlayerMatchData.CreepsKilled.ToString();
                CreepsSentText.text = PlayerMatchData.CreepsSent.ToString();
                TowersBuiltText.text = PlayerMatchData.TowersBuilt.ToString();
                Debug.Log("Populated " + NetworkOwner.PlayerName + " end score panel.");

                yield return new WaitForSeconds(5);
            }
        }
    }
}