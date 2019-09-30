using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        
    }

    public void PopulateInfo(PhotonPlayer player) //add a check for when player is ready
    {
        var playerPanel = Instantiate(PlayerReadyPanel, PlayerInfoHolder.transform);

        InfoPlanels.Add(playerPanel.GetComponent<PlayerReadyInfoPanel>());

        playerPanel.GetComponent<PlayerReadyInfoPanel>().PlayerNameText.text = player.PlayerName;
    }
}