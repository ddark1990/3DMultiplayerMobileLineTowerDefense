using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerReadyInfoPanel : MonoBehaviour
{
    public TextMeshProUGUI PlayerNameText;
    public Image PlayerReadyImage;


    private void Update()
    {
        for (int i = 0; i < GameManager.instance.playersInGame.Count; i++)
        {
            var player = GameManager.instance.playersInGame[i];

            if(player.PlayerName == PlayerNameText.text)
            {
                PlayerReadyImage.color = player.PlayerReady ? Color.green : Color.red;
            }
        }
    }
}
