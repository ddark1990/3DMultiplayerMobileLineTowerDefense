using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MatchStartTimer : MonoBehaviourPunCallbacks
{
    public static MatchStartTimer Instance;

    public GameObject TimerCanvas;
    public TextMeshProUGUI TimerText;
    private GameManager GM;

    private float startTime;

    public float time;

    public bool TimerStarted;

    public const string MATCH_START_TIMER = "MatchStartTimer";

    private Hashtable startIncomeTimerProperties = new Hashtable();

    private void Start()
    {
        Instance = this;

        GM = GameManager.instance;

        StartCoroutine(OpenMatchTimer());
        StartCoroutine(TimerStart());
    }

    private void Update()
    {
        if (!TimerStarted) return;

        time = startTime -= Time.deltaTime;

        TimerText.text = string.Format(time.ToString("#"));

        if (time <= 0)
        {
            GM.StartMatch();
            TimerText.text = string.Format("GO!");
            TimerStarted = false;
        }
    }

    private IEnumerator TimerStart()
    {
        yield return new WaitUntil(() => GM.MatchStarting);

        startTime = (float)PhotonNetwork.CurrentRoom.CustomProperties[MATCH_START_TIMER];
        TimerStarted = true;

    }

    private IEnumerator OpenMatchTimer() //move to UI/Animation layer
    {
        yield return new WaitUntil(() => TimerStarted);

        TimerCanvas.gameObject.SetActive(true);
        iTween.ScaleTo(TimerText.gameObject, new Vector3(1f, 1f, 1f), 1f);

        yield return new WaitForSeconds(1);
        StartCoroutine(CloseMatchTimer());

    }
    private IEnumerator CloseMatchTimer()
    {
        yield return new WaitUntil(() => !TimerStarted);
        yield return new WaitForSeconds(1);

        iTween.ScaleTo(TimerText.gameObject, new Vector3(0f, 0f, 0f), 1f);

        yield return new WaitForSeconds(1);
        TimerCanvas.gameObject.SetActive(false);
    }
}
