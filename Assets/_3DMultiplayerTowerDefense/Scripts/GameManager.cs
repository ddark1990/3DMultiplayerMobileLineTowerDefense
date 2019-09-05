using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using GoomerScripts;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using System;
using System.IO;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;

    public List<PhotonPlayer> playersInGame;
    public Transform[] playerSpawns;
    public int playerCount;

    public int playerGold = 50;
    public int playerIncome = 5;

    public float incomeTimer = 10;
    float startIncomeTimer;

    public int currentScene;
    public int gameScene = 2;
    public bool allPlayersLoaded = false;

    public bool ManagerCheck;


    private void Awake()
    {
        gameScene = SceneManager.GetSceneByName("GameScene").buildIndex;

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

    void Start()
    {
        CreatePlayer();
        startIncomeTimer = incomeTimer;
        StartCoroutine(AllPlayersLoadedInCheck());
    }

    void Update()
    {
        IncomeTimer();
    }

    IEnumerator AllPlayersLoadedInCheck() //convert to interface
    {
        bool checking = true;
        while (checking)
        {
            yield return new WaitForSeconds(.1f);

            if (playersInGame.Count == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                checking = false;
                allPlayersLoaded = true;
                playersInGame.Sort();
                NodeOwnership.instance.ApplyOwnershipToNodes(); //gets the ownership for the nodes for each player
                BuildingManager.instance.ApplyOwnershipToBuildings(); //gets ownership for the buildings for the players
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

                Debug.Log("AllPlayerLoaded");
            }
        }
    }

    void IncomeTimer()
    {
        incomeTimer -= Time.deltaTime;
        if(incomeTimer <= 0)
        {
            IncreaseGold();
            incomeTimer = startIncomeTimer;
        }
    }

    private void IncreaseGold()
    {
        playerGold += playerIncome;
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
