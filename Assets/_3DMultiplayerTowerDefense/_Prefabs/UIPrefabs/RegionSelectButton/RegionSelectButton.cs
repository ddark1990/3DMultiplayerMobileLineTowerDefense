using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegionSelectButton : MonoBehaviour
{
    public static RegionSelectButton Instance;

    public GameObject RegionSelectPanel;
    public SettingsCogUI CogUi;
    public TextMeshProUGUI RegionSelectionText;
    public Scrollbar ScrollBar;

    public Button[] RegionButtons;

    public string SelectedRegion;

    private void Start()
    {
        Instance = this;

        GiveButtonsFunction();
    }

    #region UILogic

    public void OnRegionPanelSelectPress()
    {
        if (!PhotonNetwork.IsConnected)
            return;

        if (GameManager.instance != null)
            if (GameManager.instance.MatchStarting || GameManager.instance.MatchStarted) return;

        StartCoroutine(OpenRegionPanel());
    }

    private void SetRegionToConnect(string regionName)
    {
        SelectedRegion = regionName;
        RegionSelectionText.text = "Select Region: " + regionName;

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
            StartCoroutine(ConnectToSetRegion());
        }
    }
    private IEnumerator ConnectToSetRegion()
    {
        yield return new WaitUntil(() => !PhotonNetwork.IsConnected);

        PhotonNetwork.ConnectToRegion(SelectedRegion);
    }
    private void GiveButtonsFunction()
    {
        foreach (var regionButton in RegionButtons)
        {
            regionButton.onClick.AddListener(() => SetRegionToConnect(GetButtonRegionName(regionButton)));
            regionButton.onClick.AddListener(() => StartCoroutine(CloseRegionPanel()));
        }
    }

    private string GetButtonRegionName(Button button)
    {
        var regionName = "";

        regionName = button.GetComponentInChildren<TextMeshProUGUI>().text;

        return regionName;
    }

    #endregion

    #region UIAnimation

    private IEnumerator OpenRegionPanel()
    {
        RegionSelectPanel.SetActive(true);
        ScrollBar.value = 0;
        iTween.ScaleTo(RegionSelectPanel, new Vector3(1f, 1f, 1f), .5f);
        yield return new WaitForSeconds(.5f);
    }

    public IEnumerator CloseRegionPanel()
    {
        iTween.ScaleTo(RegionSelectPanel, new Vector3(0f, 0f, 0f), .5f);
        yield return new WaitForSeconds(.5f);
        RegionSelectPanel.SetActive(false);
    }

    #endregion
}
