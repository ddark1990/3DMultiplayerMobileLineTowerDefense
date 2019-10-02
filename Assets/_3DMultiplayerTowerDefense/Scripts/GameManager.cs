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
    public int playerCount;

    public bool PlayerOwnershipApplied, AllPlayersReady, MatchStarted;

    public bool ManagerCheck;

    public float StartIncomeTimer = 10; //defaul 10 seconds

    private ExitGames.Client.Photon.Hashtable roomCustomProperties = new ExitGames.Client.Photon.Hashtable();
    public const string START_TIMER = "Start_Timer";

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

        roomCustomProperties[START_TIMER] = StartIncomeTimer; //send the start time of the games income timer to network so all players know where to start from
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomCustomProperties);

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

        StartCoroutine(MatchStartingTimer());
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
        }
    }

    private IEnumerator MatchStartingTimer()
    {
        var timer = 5;

        yield return new WaitUntil(() => AllPlayersReady);
        //countdown should start
        while(timer > -1)
        {
            Debug.Log("MatchTimer - " + timer);
            timer--;
            yield return new WaitForSeconds(1f);

            if(timer == -1)
            {
                MatchStarted = true;
                Debug.Log("MatchStarted!");
            }
        }
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
