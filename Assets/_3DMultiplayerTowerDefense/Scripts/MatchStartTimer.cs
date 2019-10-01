using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using TMPro;

public class MatchStartTimer : MonoBehaviourPunCallbacks
{
    private int StartTime = 5; 

    public float CountdownTime = 5f; //default 5 seconds

    public TextMeshProUGUI TimerText;

    public bool _countdown;

    private void Update()
    {
        if (!_countdown) return;

        float timer = (float)PhotonNetwork.Time - StartTime;
        float countdown = CountdownTime - timer;

        TimerText.text = string.Format("Game starts in {0} seconds", countdown.ToString("n2"));

        if(countdown <= 0)
        {
            _countdown = false;
            TimerText.text = string.Format("GO!");
        }
    }
}
