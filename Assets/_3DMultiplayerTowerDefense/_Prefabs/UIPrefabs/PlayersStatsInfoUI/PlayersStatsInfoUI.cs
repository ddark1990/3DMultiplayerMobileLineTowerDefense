using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayersStatsInfoUI : MonoBehaviour
{
    #region PublicPanelVariables

    public GameObject StatsCanvas;
    public GameObject StatsInfoPanel;
    public GameObject StatsHolder;
    public GameObject PlayerStatPanel;

    public Button PanelToggleButton;
    public Button ClosePanelButton;

    #endregion

    private GameManager GM;

    private void Start()
    {
        GM = GameManager.instance;
        StartCoroutine(PopulatePanelHolderData());
        StartCoroutine(OpenOnMatchStart());
    }

    #region UILogic

    private IEnumerator PopulatePanelHolderData()
    {
        yield return new WaitUntil(() => GM.MatchStarted);

        for (int i = 0; i < GM.playersInGame.Count; i++)
        {
            var player = GM.playersInGame[i];
            var panel = Instantiate(PlayerStatPanel, StatsHolder.transform);
            var temp = panel.GetComponent<PlayerStatsInfoPanel>();

            temp.OwnerId = player.PlayerNumber;
            StartCoroutine(PopulateData(temp, player));
        }

    }
    private IEnumerator PopulateData(PlayerStatsInfoPanel temp, PhotonPlayer player)
    {
        while (!GM.MatchEnd)
        {
            temp.PlayerNameText.text = player.PlayerName;
            temp.PlayerLivesText.text = player.PlayerData.PlayerLives.ToString();
            temp.PlayerKillsText.text = player.PlayerData.CreepsKilled.ToString();
            temp.PlayerGoldText.text = player.PlayerData.PlayerGold.ToString();
            temp.PlayerIncomeText.text = player.PlayerData.PlayerIncome.ToString();

            yield return new WaitForSeconds(.5f);
        }
    }

    #endregion

    #region PanelAnimations

    private IEnumerator OpenOnMatchStart()
    {
        iTween.ScaleTo(PanelToggleButton.gameObject, new Vector3(0f, 0f, 0f), .1f);

        yield return new WaitUntil(() => GM.MatchStarted);
        StartCoroutine(CloseStatsPanel());
    }

    private IEnumerator OpenStatsPanel()
    {
        StatsInfoPanel.SetActive(true);
        iTween.ScaleTo(PanelToggleButton.gameObject, new Vector3(0f, 0f, 0f), .5f);
        iTween.ScaleTo(StatsInfoPanel, new Vector3(1f, 1f, 1f), .5f);
        yield return new WaitForSeconds(.5f);
        PanelToggleButton.gameObject.SetActive(false);
    }
    public void OnStatsPanelOpenPress()
    {
        StartCoroutine(OpenStatsPanel());
    }

    private IEnumerator CloseStatsPanel()
    {
        PanelToggleButton.gameObject.SetActive(true);
        iTween.ScaleTo(StatsInfoPanel, new Vector3(0f, 0f, 0f), .5f);
        iTween.ScaleTo(PanelToggleButton.gameObject, new Vector3(1f, 1f, 1f), .5f);
        yield return new WaitForSeconds(.5f);
        StatsInfoPanel.SetActive(false);
    }
    public void OnStatsPanelBackPress()
    {
        StartCoroutine(CloseStatsPanel());
    }

    #endregion

}
