using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsCogUI : MonoBehaviourPunCallbacks
{
    public static SettingsCogUI instance;

    public GameObject settingsButtonCanvas;
    public GameObject settingsPanelCanvas;
    public GameObject settingsPanel;
    public Button settingsButton;
    public GameObject graphy;
    public GameObject DebugConsole;

    private void Awake()
    {
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

    private void Start()
    {
        //graphy.SetActive(false);
    }

    public void OnSettingsCogPressed()
    {
        StartCoroutine(SettingsCogPress());
    }
    IEnumerator SettingsCogPress()
    {
        iTween.ScaleTo(settingsButton.gameObject, new Vector3(0f, 0f, 0f), .5f);
        iTween.ScaleTo(settingsPanel, new Vector3(1f, 1f, 1f), .5f);
        yield return new WaitForSeconds(.5f);
        settingsButtonCanvas.SetActive(false);
        settingsPanelCanvas.SetActive(true);
    }
    public void OnSettingsBackButtonPressed()
    {
        StartCoroutine(SettingsBackPress());
    }
    IEnumerator SettingsBackPress()
    {
        iTween.ScaleTo(settingsButton.gameObject, new Vector3(1f, 1f, 1f), .5f);
        iTween.ScaleTo(settingsPanel, new Vector3(0f, 0f, 0f), .5f);
        yield return new WaitForSeconds(.5f);
        settingsPanelCanvas.SetActive(false);
        settingsButtonCanvas.SetActive(true);
    }

    public void OnBackToLogInPress()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.LeaveRoom();

        if(MainUI.instance != null)
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
        graphy.SetActive(!graphy.activeSelf);
    }

    public void ToggleDebugConsole()
    {
        DebugConsole.SetActive(!DebugConsole.activeSelf);
    }

}
