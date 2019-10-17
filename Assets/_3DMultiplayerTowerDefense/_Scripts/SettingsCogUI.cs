using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsCogUI : MonoBehaviourPunCallbacks
{
    public static SettingsCogUI instance;

    public GameObject SettingsButtonCanvas;
    public GameObject SettingsPanelCanvas;
    public GameObject SettingsPanel;
    public Button SettingsButton;
    public Button RegionSelectButton;
    public TextMeshProUGUI RegionSelectButtonText;
    public GameObject Graphy;
    public GameObject DebugConsole;
    public RegionSelectButton RegionButton;

    public bool IsWindowOpen;

    private void Awake()
    {
        Graphy.SetActive(false);

        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
    }

    public void OnSettingsCogPressed()
    {
        StartCoroutine(SettingsCogPress());
    }
    IEnumerator SettingsCogPress()
    {
        iTween.ScaleTo(SettingsButton.gameObject, new Vector3(0f, 0f, 0f), .5f);
        iTween.ScaleTo(SettingsPanel, new Vector3(1f, 1f, 1f), .5f);
        yield return new WaitForSeconds(.5f);
        SettingsButtonCanvas.SetActive(false);
        SettingsPanelCanvas.SetActive(true);
        IsWindowOpen = true;
    }
    public void OnSettingsBackButtonPressed()
    {
        StartCoroutine(SettingsBackPress());
    }
    IEnumerator SettingsBackPress()
    {
        iTween.ScaleTo(SettingsButton.gameObject, new Vector3(1f, 1f, 1f), .5f);
        iTween.ScaleTo(SettingsPanel, new Vector3(0f, 0f, 0f), .5f);
        yield return new WaitForSeconds(.5f);
        SettingsPanelCanvas.SetActive(false);
        SettingsButtonCanvas.SetActive(true);
        IsWindowOpen = false;
        StartCoroutine(RegionButton.CloseRegionPanel());
    }

    public void OnBackToLogInPress()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }

        PhotonNetwork.Disconnect();

        if (MainUI.instance != null)
        {
            MainUI.instance.queueTimer = 0;
        }

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("GameScene"))
        {
            StartCoroutine(SceneFader.instance.FadeToScene("MenuScene", 1));
        }
    }

    public void OnAchievementsPress()
    {
        Social.ShowAchievementsUI();
    }

    public void OnLeaderboardsPress()
    {
        Social.ShowLeaderboardUI();
    }

    public void ToggleProfiler()
    {
        Graphy.SetActive(!Graphy.activeSelf);
    }

    public void ToggleDebugConsole()
    {
        DebugConsole.SetActive(!DebugConsole.activeSelf);
    }

}
