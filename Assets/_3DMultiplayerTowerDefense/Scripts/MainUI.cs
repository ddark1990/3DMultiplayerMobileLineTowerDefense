using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainUI : MonoBehaviourPunCallbacks
{
    public static MainUI instance;

    [Header("Canvas")]
    public GameObject mainMenuCanvas;
    public GameObject tapToLoginCanvas;
    public GameObject pvpCanvas;
    public GameObject pveCanvas;
    public GameObject queuedPvpCanvas;
    public GameObject backgroundCanvas;
    public GameObject gameCanvas;

    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject pvpPanel;
    public GameObject pvePanel;
    public GameObject queuedPvpPanel;
    public GameObject backgroundPanel;

    [Header("Buttons")]
    public Button loginButton;
    public Button logoutButton;
    public Button pvpButton;
    public Button pveButton;
    public Button quickmatchButton;
    public Button pvpBackButton;
    public Button pveBackButton;
    public Button queuedPvpCancelButton;

    [Header("Text")]
    public TextMeshProUGUI queuedPvpPlayerAmmountText;
    public TextMeshProUGUI queuedPvpTimerText;
    public TextMeshProUGUI playerAmmount1v1QueueText;
    public TextMeshProUGUI timer1v1QueueText;
    public TextMeshProUGUI queueTitleText;
    public Texture2D playerSprite;
    public int playerId;

    [Header("Toggles")]
    public Toggle profilerToggle;

    [Header("Animation Values")]
    public float buttonAnimSpeed = 1f;

    public int roomCount;

    List<RoomInfo> rooms = new List<RoomInfo>();
    public float queueTimer = 0;
    public float countdownTimer = 5;
    private bool matchStarting;
    private bool gameStarting;
    public GameObject queuedAnimation;


    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            //DontDestroyOnLoad(Instance);
        }

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Update()
    {
        if(gameStarting == true && SceneManager.GetActiveScene() == SceneManager.GetSceneByName("GameScene"))
        {
            gameStarting = false;
        }

        roomCount = rooms.Count;

        if(PhotonNetwork.InRoom)
        {
            QueueTimers();
        }
    }

    #region Photon Methods

    public override void OnConnectedToMaster()
    {
        PopUpSystem.Instance.SelfConnected_Message();

    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PopUpSystem.Instance.SelfDisconnected_Message();
        SetActivePanel(tapToLoginCanvas.name);
        iTween.ScaleTo(loginButton.gameObject, new Vector3(1f, 1f, 1f), .5f);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        rooms = roomList;
        for (int i = 0; i < rooms.Count; i++)
        {
            RoomInfo room = rooms[i];
            if(room.PlayerCount == room.MaxPlayers)
            {
                room.RemovedFromList = false;
                rooms.Remove(room);
                Debug.Log(room + " has been removed");
            }
        }
        Debug.Log("RoomList Updated, Room Count: " + rooms.Count);
    }

    public override void OnJoinedLobby()
    {
        SetActivePanel(pvpCanvas.name);
    }

    public override void OnLeftLobby()
    {
        
    }

    public override void OnJoinedRoom()
    {
        SetActivePanel(queuedPvpCanvas.name);
        Debug.Log("Room Joined: " + PhotonNetwork.CurrentRoom);
        Update1v1QueueText();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {

    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomOptions options = new RoomOptions { MaxPlayers = 2 };
        PhotonNetwork.CreateRoom(Random.Range(0, 1000000).ToString(), options);
    }

    public override void OnLeftRoom()
    {
        SetActivePanel(pvpCanvas.name);
        Debug.Log("Left Room");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Update1v1QueueText();
        PopUpSystem.Instance.PlayerJoinedRoom_Message(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Update1v1QueueText();
        PopUpSystem.Instance.PlayerLeftRoom_Message(otherPlayer);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room Created: " + PhotonNetwork.CurrentRoom);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log(returnCode + " : " + message);
    }

    #endregion

    private void Update1v1QueueText()
    {
        matchStarting = false;
        playerAmmount1v1QueueText.text = PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            queueTimer = 0f;
            matchStarting = true;
            queuedAnimation.GetComponent<Animator>().SetBool("MatchStarting", matchStarting);
            queueTitleText.text = "Match Starting";
        }
        else
        {
            countdownTimer = 5;
            //queuedPvpCancelButton.gameObject.SetActive(true);
            matchStarting = false;
            queueTitleText.text = "Searching...";
            queuedAnimation.GetComponent<Animator>().SetBool("MatchStarting", matchStarting);
        }
    }

    private void QueueTimers()
    {
        if (countdownTimer > 0 && PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            countdownTimer -= Time.deltaTime;
            timer1v1QueueText.text = countdownTimer.ToString("#");
        }
        else if(countdownTimer <= 0.1f)
        {
            timer1v1QueueText.text = "Starting Game!";
            StartMatch();
        }
        else
        {
            queueTimer += Time.deltaTime;
            timer1v1QueueText.text = queueTimer.ToString("F");
        }
    }

    private void StartMatch()
    {
        if(gameStarting == false)
        {
            gameStarting = true;
            StartCoroutine(StartingMatch());
        }
    }

    private IEnumerator StartingMatch()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        Debug.Log("Starting Match");
        StartingGameTweenAnimations();
        
        yield return new WaitForSeconds(1);

        SetActivePanel(gameCanvas.name);

        StartCoroutine(SceneFader.instance.FadeToNetworkScene("GameScene", 1));
    }
    private void StartingGameTweenAnimations()
    {
        iTween.ScaleFrom(timer1v1QueueText.gameObject, new Vector3(0f, 0f, 0f), 1f);
        iTween.ScaleTo(queuedPvpCancelButton.gameObject, new Vector3(0f, 0f, 0f), 1f);
        iTween.ScaleTo(backgroundPanel, new Vector3(0f, 0f, 0f), 1f);
        iTween.ScaleTo(queuedPvpPanel, new Vector3(0f, 0f, 0f), 1f);
    }

    #region Public Methods

    public IEnumerator LoginPress()
    {
        LoginTween();
        PhotonNetwork.ConnectUsingSettings();

        yield return new WaitUntil(() => PhotonNetwork.IsConnectedAndReady);
        SetActivePanel(mainMenuCanvas.name);
    }
    public void OnLoginPress()
    {
        StartCoroutine(LoginPress());
    }
    public void LoginTween()
    {
        iTween.ScaleTo(mainMenuPanel, new Vector3(1f, 1f, 1f), .5f);
        iTween.ScaleTo(loginButton.gameObject, new Vector3(0f, 0f, 0f), .5f);
    }

    public IEnumerator LogoutPress()
    {
        LogoutTween();
        yield return new WaitForSeconds(.5f);
        PhotonNetwork.Disconnect();
    }
    public void OnLogoutPress()
    {
        StartCoroutine(LogoutPress());
    }
    public void LogoutTween()
    {
        iTween.ScaleTo(mainMenuPanel, new Vector3(0f, 0f, 0f), .5f);
        iTween.ScaleTo(loginButton.gameObject, new Vector3(1f, 1f, 1f), .5f);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnPvPButtonPress()
    {
        StartCoroutine(PvPPress());
    }
    IEnumerator PvPPress()
    {
        iTween.ScaleTo(mainMenuPanel, new Vector3(0, 0f, 0f), 1f);

        yield return new WaitUntil(() => PhotonNetwork.IsConnectedAndReady);
        yield return new WaitForSeconds(1f);

        PhotonNetwork.JoinLobby();
        iTween.ScaleTo(pvpPanel, new Vector3(1f, 1f, 1f), .5f);
    }

    public void OnPvPBackButtonPressed()
    {
        StartCoroutine(BackPvPPress());
    }
    IEnumerator BackPvPPress()
    {
        iTween.ScaleTo(pvpPanel, new Vector3(0, 0f, 0f), .5f);
        PhotonNetwork.LeaveLobby();
        yield return new WaitForSeconds(1f);
        SetActivePanel(mainMenuCanvas.name);
        iTween.ScaleTo(mainMenuPanel, new Vector3(1f, 1f, 1f), .5f);
    }

    public void OnQuickmatchButtonPress()
    {
        StartCoroutine(QuickmatchPress());
    }
    private IEnumerator QuickmatchPress()
    {
        yield return new WaitForSeconds(.5f);
        iTween.ScaleTo(pvpPanel, new Vector3(0, 0f, 0f), .5f);
        iTween.ScaleTo(queuedPvpPanel, new Vector3(1f, 1f, 1f), .5f);

        PhotonNetwork.JoinRandomRoom();

        //if (rooms.Count == 0)
        //{
        //    RoomOptions options = new RoomOptions { MaxPlayers = 2 };
        //    PhotonNetwork.CreateRoom(Random.Range(0, 1000000).ToString(), options);
        //}
        //else
        //{
        //    for (int i = 0; i < rooms.Count; i++)
        //    {
        //        RoomInfo room = rooms[i];
        //        if (room.PlayerCount != room.MaxPlayers)
        //        {
        //            PhotonNetwork.JoinRoom(room.Name);
        //        }
        //    }
        //}
    }

    public void On1v1QuickmatchCancelButtonPress()
    {
        StartCoroutine(Quickmatch1v1CancelPress());
    }
    IEnumerator Quickmatch1v1CancelPress()
    {
        iTween.ScaleTo(queuedPvpPanel, new Vector3(0, 0f, 0f), .5f);
        PhotonNetwork.LeaveRoom();
        yield return new WaitForSeconds(2f);
        queueTimer = 0;
        countdownTimer = 5;
        PhotonNetwork.JoinLobby();
        iTween.ScaleTo(pvpPanel, new Vector3(1f, 1f, 1f), .5f);
    }

    public void OnPvEButtonPress()
    {
        StartCoroutine(PvEPress());
    }
    private IEnumerator PvEPress()
    {
        iTween.ScaleTo(mainMenuPanel, new Vector3(0, 0f, 0f), .5f);
        iTween.ScaleTo(pvePanel, new Vector3(1f, 1f, 1f), .5f);
        yield return new WaitForSeconds(.5f);
        SetActivePanel(pveCanvas.name);
    }

    public void SetActivePanel(string activePanel)
    {
        mainMenuCanvas.SetActive(activePanel.Equals(mainMenuCanvas.name));
        tapToLoginCanvas.SetActive(activePanel.Equals(tapToLoginCanvas.name));
        pvpCanvas.SetActive(activePanel.Equals(pvpCanvas.name));
        pveCanvas.SetActive(activePanel.Equals(pveCanvas.name));
        queuedPvpCanvas.SetActive(activePanel.Equals(queuedPvpCanvas.name));
        //backgroundCanvas.SetActive(activePanel.Equals(backgroundCanvas.Name));
        gameCanvas.SetActive(activePanel.Equals(gameCanvas.name));
    }

    #endregion
}
