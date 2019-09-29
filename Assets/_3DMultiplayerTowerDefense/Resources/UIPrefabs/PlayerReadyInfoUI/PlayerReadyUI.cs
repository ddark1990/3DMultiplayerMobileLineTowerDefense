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

    public void PopulateInfo()
    {
        foreach (var player in GameManager.instance.playersInGame)
        {
            if (GameManager.instance.playersInGame.Count != PhotonNetwork.CurrentRoom.MaxPlayers) continue;

            var playerPanel = Instantiate(PlayerReadyPanel, PlayerInfoHolder.transform);

            InfoPlanels.Add(playerPanel.GetComponent<PlayerReadyInfoPanel>());

            for (int i = 0; i < InfoPlanels.Count; i++)
            {
                var panel = InfoPlanels[i].GetComponent<PlayerReadyInfoPanel>();
                
                panel.PlayerNameText.text = GameManager.instance.playersInGame[i].PlayerName;
                //panel.PlayerReadyImage.color = player.PlayerReady ? Color.green : Color.red;
            }
            //make player rdy work correctly through this buffer window
        }
    }
}