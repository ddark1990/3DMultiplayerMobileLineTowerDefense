using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MatchSystem
{
    public class PlayerReadyUI : MonoBehaviour
    {
        public static PlayerReadyUI Instance;

        public GameObject PlayerReadyPanel;
        public GameObject BackPanel;
        public GameObject PlayerInfoHolder;
        public Canvas PlayerReadyCanvas;

        public List<PlayerReadyInfoPanel> InfoPlanels;

        private void OnEnable()
        {
            if (Instance == null) Instance = this;

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            StartCoroutine(OpenWaitWindow());
            StartCoroutine(CloseWaitWindow());
        }

        private IEnumerator OpenWaitWindow()
        {
            PlayerReadyCanvas.gameObject.SetActive(true);
            iTween.ScaleTo(BackPanel, new Vector3(1f, 1f, 1f), 1f);

            yield return new WaitForSeconds(1);
        }
        private IEnumerator CloseWaitWindow()
        {
            yield return new WaitUntil(() => MatchManager.Instance.AllPlayersReady);

            iTween.ScaleTo(BackPanel, new Vector3(0f, 0f, 0f), 1f);

            yield return new WaitForSeconds(1);
            PlayerReadyCanvas.gameObject.SetActive(false);
        }

        public void PopulateInfo(NetworkPlayer player) //add a check for when player is ready
        {
            var playerPanel = Instantiate(PlayerReadyPanel, PlayerInfoHolder.transform);

            InfoPlanels.Add(playerPanel.GetComponent<PlayerReadyInfoPanel>());

            playerPanel.GetComponent<PlayerReadyInfoPanel>().PlayerNameText.text = player.PlayerName;
        }

    }
}