using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using System;
using UnityEngine.SceneManagement;

namespace MatchSystem
{
    public class NetworkPlayer : MonoBehaviourPunCallbacks
    {
        [Header("PlayerInfo")]
        public string PlayerName;
        public int PlayerNumber;
        public bool PlayerReady, PoolsLoaded, MenusLoaded;

        [Header("Cache")]
        public GameObject PlayerControllers;
        public GameObject GridGenerator;

        private GameObject grid;
        [HideInInspector] public GameObject playerControllers;
        private MobileCameraControls mobileControls;

        private void Awake()
        {
            if (!photonView.IsMine) return;

            StartCoroutine(Init());
        }

        private IEnumerator Init()
        {
            StartCoroutine(CheckIfPoolsLoaded());
            StartCoroutine(CheckIfMenusLoaded());
            StartCoroutine(SetPlayerReady());

            mobileControls = FindObjectOfType<MobileCameraControls>();

            yield return new WaitForSeconds(1);
            SpawnControllers();

            yield return new WaitForSeconds(1);
            photonView.RPC("RPC_SendPlayerData", RpcTarget.AllViaServer); //send own player data across network for others to see

            yield return new WaitForSeconds(1);
            MatchBuyMenu.Instance.BuildMenus(this); //build all player menus
        }

        public void SpawnControllers()
        {
            playerControllers = Instantiate(PlayerControllers, new Vector3(0, 0, 0), Quaternion.identity, transform);
            grid = Instantiate(GridGenerator, new Vector3(0, 0, 0), Quaternion.identity, transform);
        }

        [PunRPC]
        public void RPC_SendPlayerData() 
        {
            var matchManager = MatchManager.Instance;
            matchManager.PlayersInGame.Add(this); //allows the manager know when the player has started the of sending information

            PlayerNumber = photonView.Owner.ActorNumber; //player photon network specific data
            PlayerName = photonView.Owner.NickName;

            if (mobileControls) //set player cam
            {
                mobileControls.transform.position = SpawnContainer.Instance.CameraSpawnPoints[PlayerNumber - 1].position;
                mobileControls.transform.rotation = SpawnContainer.Instance.CameraSpawnPoints[PlayerNumber - 1].rotation;
                mobileControls.DisableMobileControls = true;
            }

            if(grid) //set player gridgen
            {
                grid.transform.position = SpawnContainer.Instance.GridSpawnPoints[PlayerNumber - 1].position;
                grid.GetComponent<GridGenerator>().GridWorldSize.Set(6, 10);
                grid.GetComponent<GridGenerator>().NetworkOwner = this;
            }

            if(playerControllers) //create playercontrollers such as creepsender & towerplacer
            {
                playerControllers.GetComponent<CreepSender>().NetworkOwner = this;
                playerControllers.GetComponent<TowerPlacer>().NetworkOwner = this;
            }

            PlayerReadyUI.Instance.PopulateInfo(this); //join the ready queue

            gameObject.name += " " + GetComponent<PhotonView>().Owner.NickName; //set object with network nickname

            Debug.Log("SendingPlayerData for: " + PlayerName);
        }

        #region SceneFullyLoadedCheck
        private IEnumerator CheckIfPoolsLoaded()
        {
            yield return new WaitUntil(() => PoolManager.Instance.PoolsLoaded);

            PoolsLoaded = true;
        }
        private IEnumerator CheckIfMenusLoaded()
        {
            yield return new WaitUntil(() => MatchBuyMenu.Instance.MenusLoaded);

            MenusLoaded = true;
        }
        private IEnumerator SetPlayerReady()
        {
            yield return new WaitUntil(() => PoolsLoaded);
            yield return new WaitForSeconds(6); //buffer

            photonView.RPC("RPC_ReadyPlayer", RpcTarget.AllViaServer);
        }
        [PunRPC]
        private void RPC_ReadyPlayer()
        {
            PlayerReady = true;
            MatchManager.Instance.PlayersReady.Add(this);
            Debug.Log(PlayerName + " is ready!");
        }
        #endregion
    }
}