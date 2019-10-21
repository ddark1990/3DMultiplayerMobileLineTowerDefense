using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using ExitGames.Client.Photon;

//1v1 game manager, break down into a dynamic match controller that would be able to controll any type of match, 1v1 or team

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;

    public List<PhotonPlayer> playersInGame, playersReady;
    public Transform[] playerSpawns;
    public int playerCount, IncomeStartTimer = 10;

    public bool PlayerOwnershipApplied, AllPlayersReady, MatchStarting, MatchStarted, TeamMatch, MatchEnd;

    public int VictorId;

    public bool ManagerCheck;

    private ExitGames.Client.Photon.Hashtable matchStartTimerProperties = new ExitGames.Client.Photon.Hashtable();
    public const string MATCH_START_TIMER = "MatchStartTimer";
    public const string START_INCOME_TIMER = "StartIncomeTimer";

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
        }

        if (ManagerCheck)
        {
            ManagersCheck(); //checks scene if managers are existant, if not, sends us back to the preload scene
        }
    }

    private new void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
        PhotonNetwork.NetworkingClient.EventReceived += MatchEnd_EventReceived;
        PhotonNetwork.NetworkingClient.EventReceived += PlayerWonCheck_EventReceived;
    }

    private new void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
        PhotonNetwork.NetworkingClient.EventReceived -= MatchEnd_EventReceived;
        PhotonNetwork.NetworkingClient.EventReceived -= PlayerWonCheck_EventReceived;
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.isLoaded)
        {
            Init();
        }
    }

    private void Init()
    {
        CreatePlayer();

        StartCoroutine(PlayerOwnershipAppliedCheck());
        StartCoroutine(AllPlayersReadyCheck());
        StartCoroutine(PlayerWonCheck());

        TeamMatch = TeamCheck(PhotonNetwork.CurrentRoom.MaxPlayers);
    }

    private IEnumerator PlayerOwnershipAppliedCheck()
    {
        var checking = true;

        while (checking)
        {
            yield return new WaitForSeconds(.1f);

            if(playersInGame.Count == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                playersInGame.Sort();

                NodeOwnership.instance.ApplyOwnershipToNodes();
                //BuildingManager.instance.ApplyOwnershipToBuildings();
                //SpawningArea.instance.ApplyOwnershipToSpawners();
                //ConstructionManager.instance.ApplyOwnershipToTowerPlacers();

                foreach (Node node in NodeOwnership.instance.nodes)
                {
                    node.SetNodeSelectability();
                }

                foreach (GameObject building in BuildingManager.instance.buildings)
                {
                    building.GetComponent<CreepSender>().SetBuildingSelectability();
                }

                PlayerOwnershipApplied = true;

                Debug.Log("PlayerOwnershipApplied " + PlayerOwnershipApplied);
                checking = false;
            }
        }
    }

    private IEnumerator AllPlayersReadyCheck() 
    {
        yield return new WaitUntil(() => playersReady.Count == PhotonNetwork.CurrentRoom.MaxPlayers);

        var checking = true;

        while (checking)
        {
            yield return new WaitForSeconds(1f);
            AllPlayersReady = true;

            Debug.Log("AllPlayersReady " + AllPlayersReady);

            checking = false;

            StartCoroutine(MatchStartingCheck());
        }
    }

    private IEnumerator MatchStartingCheck() 
    {
        yield return new WaitUntil(() => AllPlayersReady);

        matchStartTimerProperties[MATCH_START_TIMER] = 5f;
        matchStartTimerProperties[START_INCOME_TIMER] = 10;
        PhotonNetwork.CurrentRoom.SetCustomProperties(matchStartTimerProperties);

        MatchStarting = true;
    }
    public void StartMatch()
    {
        MobileCameraControls.Instance.DisableMobileControls = false;
        MatchStarted = true;

        Debug.Log("MatchStarted!");
    }

    #region MatchNetworkEvents

    public void MatchEndCheck() //temp 1v1 solution
    {
        var playersLost = 0;

        foreach (var player in playersInGame)
        {
            if (player.GetComponent<PlayerMatchData>().PlayerLives == 0)
            {
                playersLost++;
            }

            if (playersLost == PhotonNetwork.CurrentRoom.MaxPlayers - 1)
            {
                MatchEnd = true;

                object[] sendMatchEndData = new object[] { MatchEnd };

                RaiseEventOptions options = new RaiseEventOptions()
                {
                    CachingOption = EventCaching.DoNotCache,
                    Receivers = ReceiverGroup.All
                };

                PhotonNetwork.RaiseEvent((byte)EventIdHandler.EVENT_IDs.MATCH_END, sendMatchEndData, options, SendOptions.SendUnreliable);

                return;
            }
        }
    }
    private void MatchEnd_EventReceived(EventData obj)
    {
        if (obj.Code == (byte)EventIdHandler.EVENT_IDs.MATCH_END)
        {
            object[] data = (object[])obj.CustomData;

            var matchEnd = (bool)data[0];

            var playersLost = 0;

            foreach (var player in playersInGame)
            {
                if (player.GetComponent<PlayerMatchData>().PlayerLives == 0)
                {
                    playersLost++;
                }

                if (playersLost == 1)
                {
                    MatchEnd = matchEnd;
                    Debug.Log("MatchEnd!");
                    MobileCameraControls.Instance.DisableMobileControls = true;
                    return;
                }
            }

        }
    }

    private IEnumerator PlayerWonCheck()
    {
        yield return new WaitUntil(() => MatchEnd);

        for (int i = 0; i < playersInGame.Count; i++)
        {
            var player = playersInGame[i];

            if(player.PlayerData.PlayerLives != 0)
            {
                VictorId = player.photonView.OwnerActorNr;

                object[] sendPlayerWonData = new object[] { VictorId };

                RaiseEventOptions options = new RaiseEventOptions()
                {
                    CachingOption = EventCaching.DoNotCache,
                    Receivers = ReceiverGroup.All
                };

                PhotonNetwork.RaiseEvent((byte)EventIdHandler.EVENT_IDs.PLAYER_WON, sendPlayerWonData, options, SendOptions.SendUnreliable);
            }
        }
    }
    private void PlayerWonCheck_EventReceived(EventData obj)
    {
        if (obj.Code == (byte)EventIdHandler.EVENT_IDs.PLAYER_WON)
        {
            object[] data = (object[])obj.CustomData;

            var victorId = (int)data[0];

            VictorId = victorId;

            if(PhotonNetwork.IsMasterClient)
                Debug.Log(PhotonNetwork.CurrentRoom.GetPlayer(VictorId).NickName + " is victorious!");
        }
    }

    #endregion

    private void CreatePlayer() 
    {
        var player = PhotonNetwork.Instantiate("NetworkPlayer", new Vector3(0, 0, 0), Quaternion.identity, 0);
    }

    private bool TeamCheck(int playerCount)
    {
        if (playerCount > 2)
        {
            return true;
        }
        return false;
    }

    void ManagersCheck() //temp debug
    {
        GameObject check = GameObject.Find("Managers");
        if (check == null)
        {
            SceneManager.LoadScene("_preload");
        }
    }
}
