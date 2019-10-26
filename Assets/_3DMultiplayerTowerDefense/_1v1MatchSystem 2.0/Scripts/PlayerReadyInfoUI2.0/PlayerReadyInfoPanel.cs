using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace MatchSystem
{
    public class PlayerReadyInfoPanel : MonoBehaviour
    {
        public TextMeshProUGUI PlayerNameText;
        public Image PlayerReadyImage;


        private void Update()
        {
            for (int i = 0; i < MatchManager.Instance.PlayersInGame.Count; i++)
            {
                var player = MatchManager.Instance.PlayersInGame[i];

                if (player.PlayerName == PlayerNameText.text)
                {
                    PlayerReadyImage.color = player.PlayerReady ? Color.green : Color.red;
                }
            }
        }

    }
}
