using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;

    public List<PhotonPlayer> playersInGame, playersReady;
    public Transform[] playerSpawns;
    public int playerCount, IncomeStartTimer = 10;

    public bool PlayerOwnershipApplied, AllPlayersReady, MatchStarting, MatchStarted, MatchEnd;

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
    }

    private new void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
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
                BuildingManager.instance.ApplyOwnershipToBuildings();
                SpawningArea.instance.ApplyOwnershipToSpawners();
                ConstructionManager.instance.ApplyOwnershipToTowerPlacers();

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

    private void CreatePlayer() 
    {
        var player = PhotonNetwork.Instantiate("NetworkPlayer", new Vector3(0, 0, 0), Quaternion.identity, 0);
    }

    void ManagersCheck()
    {
        GameObject check = GameObject.Find("Managers");
        if (check == null)
        {
            SceneManager.LoadScene("_preload");
        }
    }
}
