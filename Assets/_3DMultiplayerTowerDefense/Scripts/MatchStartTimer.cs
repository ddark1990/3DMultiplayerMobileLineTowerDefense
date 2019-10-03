using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using TMPro;

public class MatchStartTimer : MonoBehaviourPunCallbacks
{
    public GameObject TimerCanvas;
    public TextMeshProUGUI TimerText;
    private GameManager GM;

    private void Start()
    {
        GM = GameManager.instance;

        StartCoroutine(OpenMatchTimer());
        StartCoroutine(CloseMatchTimer());
    }

    private IEnumerator OpenMatchTimer()
    {
        yield return new WaitUntil(() => GM.MatchStarting);

        TimerCanvas.gameObject.SetActive(true);
        iTween.ScaleTo(TimerText.gameObject, new Vector3(1f, 1f, 1f), 1f);

        yield return new WaitForSeconds(1);
    }
    private IEnumerator CloseMatchTimer()
    {
        yield return new WaitUntil(() => GM.MatchStarted);

        iTween.ScaleTo(TimerText.gameObject, new Vector3(0f, 0f, 0f), 1f);

        yield return new WaitForSeconds(1);
        TimerCanvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!GM.MatchStarting) return;

        TimerText.text = string.Format(GM.MatchStartTimer.ToString());

        if(GM.MatchStartTimer <= 0)
        {
            GM.MatchStarting = false;
            TimerText.text = string.Format("GO!");
        }
    }
}
