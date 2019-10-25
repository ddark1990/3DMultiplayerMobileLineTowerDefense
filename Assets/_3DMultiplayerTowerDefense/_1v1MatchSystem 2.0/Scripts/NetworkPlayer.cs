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
        public bool PlayerReady, PoolsLoaded;
        public Camera PlayerCam;

        [Header("Cache")]
        public GameObject PlayerControllers;
        public GameObject GridGenerator;

        private GameObject grid;
        private GameObject playerControllers;

        private void Awake()
        {
            StartCoroutine(Init());
        }

        private IEnumerator Init()
        {
            StartCoroutine(CheckIfPoolsLoaded());
            StartCoroutine(SetPlayerReady());

            if (photonView.IsMine)
            {
                PlayerCam = FindObjectOfType<Camera>();

                yield return new WaitForEndOfFrame();
                photonView.RPC("RPC_SpawnControllers", RpcTarget.All); //send own player data across network for others to see
            }

            yield return new WaitForEndOfFrame();
            photonView.RPC("RPC_SendPlayerData", RpcTarget.AllViaServer); //send own player data across network for others to see
        }

        [PunRPC]
        public void RPC_SpawnControllers()
        {
            playerControllers = Instantiate(PlayerControllers, new Vector3(0, 0, 0), Quaternion.identity, transform);
            grid = Instantiate(GridGenerator, new Vector3(0, 0, 0), Quaternion.identity, transform);
        }

        [PunRPC]
        public void RPC_SendPlayerData() 
        {
            var matchManager = MatchManager.Instance;
            matchManager.PlayersInGame.Add(this);

            PlayerNumber = photonView.Owner.ActorNumber;
            PlayerName = photonView.Owner.NickName;

            if (PlayerCam)
            {
                PlayerCam.transform.position = SpawnContainer.Instance.CameraSpawnPoints[PlayerNumber - 1].position;
                PlayerCam.transform.rotation = SpawnContainer.Instance.CameraSpawnPoints[PlayerNumber - 1].rotation;
            }

            if(grid)
            {
                grid.transform.position = SpawnContainer.Instance.GridSpawnPoints[PlayerNumber - 1].position;
                grid.GetComponent<GridGenerator>().GridWorldSize.Set(6, 10);
                grid.GetComponent<GridGenerator>().NetworkOwner = this;
            }

            if(playerControllers)
            {
                playerControllers.GetComponent<CreepSender>().NetworkOwner = this;
                playerControllers.GetComponent<TowerPlacer>().NetworkOwner = this;
            }

            gameObject.name += " " + GetComponent<PhotonView>().Owner.NickName;

            Debug.Log("SendingPlayerData for: " + PlayerName);
        }

        #region SceneFullyLoadedCheck
        private IEnumerator CheckIfPoolsLoaded()
        {
            yield return new WaitUntil(() => PoolManager.Instance.PoolsLoaded);

            PoolsLoaded = true;
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