using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using GoomerScripts;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using System;
using System.IO;
using ExitGames.Client.Photon;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;

    public List<PhotonPlayer> playersInGame;
    public Transform[] playerSpawns;
    public int playerCount;

    public bool allPlayersLoaded;

    public bool ManagerCheck;

    public int IncomeTimer = 10;

    private int _startIncomeTimer;

    private ExitGames.Client.Photon.Hashtable roomCustomProperties = new ExitGames.Client.Photon.Hashtable();
    public const string TIMER = "Timer";

    public int currentScene;
    public int _gameScene;

    private void Awake()
    {
        _gameScene = SceneManager.GetSceneByName("GameScene").buildIndex;

        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            //DontDestroyOnLoad(Instance);
        }

        if (ManagerCheck)
        {
            ManagersCheck(); //checks scene if managers are existant, if not, sends us back to the preload scene
        }
    }

    private new void OnEnable()
    {
        Init();
        SceneManager.sceneLoaded += OnSceneFinishedLoading;

    }

    private new void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;

        if (scene.isLoaded)
        {
            CreatePlayer();
            Debug.Log(PhotonNetwork.LocalPlayer.NickName + " has loaded. " + mode);
        }
    }

    private void Init()
    {
        _startIncomeTimer = IncomeTimer;
        StartCoroutine(AllPlayersLoadedInCheck());
        StartCoroutine(IncomeCoRoutine());
    }

    IEnumerator AllPlayersLoadedInCheck() //convert to interface
    {
        var checking = true;

        while (checking)
        {
            yield return new WaitForSeconds(.1f);

            if (playersInGame.Count == PhotonNetwork.CurrentRoom.MaxPlayers)
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

                for (int i = 0; i < playersInGame.Count; i++)
                {
                    var player = playersInGame[i];

                    player.PlayerReady = true;
                    playerCount++;
                }

                checking = false;
                allPlayersLoaded = true;

                Debug.Log("AllPlayerLoaded");
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

    private IEnumerator IncomeCoRoutine()
    {
        while (true)
        {
            IncomeTime();
            roomCustomProperties[TIMER] = IncomeTimer;
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomCustomProperties);

            yield return new WaitForSeconds(1);
        }
    }

    private void IncomeTime() 
    {
        if (!allPlayersLoaded)
        {
            IncomeTimer = 10;
            return;
        }

        IncomeTimer--;

        if (!(IncomeTimer <= 0)) return;

        foreach (var player in playersInGame)
        {
            player.photonView.RPC("IncreaseGold", RpcTarget.AllViaServer);
            //player.GetComponent<PlayerMatchData>().IncreaseGold();
        }

        IncomeTimer = _startIncomeTimer;
    }
}
