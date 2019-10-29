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
        public TextMeshProUGUI VictoryText;

        public bool ExitButtonPressed;

        private void Start()
        {
            StartCoroutine(ShowMatchEndingText());
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

            yield return new WaitForSeconds(2);
            iTween.ScaleTo(HolderPanel.gameObject, new Vector3(0f, 0f, 0f), 1f);
            yield return new WaitForSeconds(1);

            StartCoroutine(ShowMatchEndingScore());
        }
        private IEnumerator ShowMatchEndingScore()
        {
            VictoryText.enabled = false;
            ScoreBackground.SetActive(true);

            iTween.ScaleTo(HolderPanel.gameObject, new Vector3(1f, 1f, 1f), 1f);

            yield return new WaitUntil(() => ExitButtonPressed);

            iTween.ScaleTo(HolderPanel.gameObject, new Vector3(0, 0, 0), 1f);
            yield return new WaitForSeconds(1);

            matchEndCanvas.gameObject.SetActive(false); //could do a reset of match flags
        }

    }
}