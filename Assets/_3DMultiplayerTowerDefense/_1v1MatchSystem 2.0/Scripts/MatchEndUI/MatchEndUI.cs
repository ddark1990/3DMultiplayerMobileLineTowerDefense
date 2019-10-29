using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

namespace MatchSystem
{
    public class MatchEndUI : MonoBehaviour
    {
        [SerializeField] private Canvas matchEndCanvas;

        public GameObject HolderPanel;
        public GameObject ScoreBackground;
        public GameObject PlayerPanelHolder;
        public TextMeshProUGUI VictoryText;

        [Header("InstantiatedObject")]
        public GameObject PlayerEndScorePanel;


        public bool ExitButtonPressed;

        private void Start()
        {
            StartCoroutine(ShowMatchEndingText());
        }

        private void CreatePlayerEndScorePanels()
        {
            foreach (var player in MatchManager.Instance.PlayersInGame)
            {
                var playerEndScorePanel = Instantiate(PlayerEndScorePanel, PlayerPanelHolder.transform);
                playerEndScorePanel.GetComponent<PlayerEndScorePanel>().NetworkOwner = player;
                playerEndScorePanel.GetComponent<PlayerEndScorePanel>().PlayerMatchData = player.GetComponent<PlayerMatchData>();
            }
        }

        private IEnumerator ShowMatchEndingText()
        {
            yield return new WaitUntil(() => MatchManager.Instance.MatchEnd);

            matchEndCanvas.gameObject.SetActive(true);

            if (PhotonNetwork.LocalPlayer.ActorNumber == MatchManager.Instance.VictorId)
            {
                VictoryText.text = "Victory!";
            }
            else
            {
                VictoryText.text = "Defeat!";
            }

            iTween.ScaleTo(HolderPanel.gameObject, new Vector3(1f, 1f, 1f), 1f);

            yield return new WaitForSeconds(5);
            iTween.ScaleTo(HolderPanel.gameObject, new Vector3(0f, 0f, 0f), 1f);
            yield return new WaitForSeconds(1);
            VictoryText.enabled = false;

            StartCoroutine(ShowMatchEndingScore());
        }
        private IEnumerator ShowMatchEndingScore()
        {
            CreatePlayerEndScorePanels();

            iTween.ScaleTo(ScoreBackground.gameObject, new Vector3(1f, 1f, 1f), 1f);
            iTween.ScaleTo(HolderPanel.gameObject, new Vector3(1f, 1f, 1f), 1f);

            yield return new WaitUntil(() => ExitButtonPressed);

            iTween.ScaleTo(ScoreBackground.gameObject, new Vector3(0, 0, 0), .5f);
            yield return new WaitForSeconds(.5f);

            matchEndCanvas.gameObject.SetActive(false); //could do a reset of match flags
        }

    }
}