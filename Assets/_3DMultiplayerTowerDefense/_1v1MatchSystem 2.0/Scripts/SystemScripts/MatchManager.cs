using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatchSystem
{
    public class MatchManager : MonoBehaviourPunCallbacks
    {
        public static MatchManager Instance;

        public List<NetworkPlayer> PlayersInGame;
        public List<NetworkPlayer> PlayersReady;

        public int VictorId;

        public bool AllPlayersReady, MatchStarting, MatchStarted, MatchEnd;

        private ExitGames.Client.Photon.Hashtable matchStartTimerProperties = new ExitGames.Client.Photon.Hashtable();

        #region Unity Methods
        private new void OnEnable()
        {
            PhotonNetwork.NetworkingClient.EventReceived += MatchEnd_EventReceived;
            PhotonNetwork.NetworkingClient.EventReceived += PlayerWonCheck_EventReceived;
        }
        private new void OnDisable()
        {
            PhotonNetwork.NetworkingClient.EventReceived -= MatchEnd_EventReceived;
            PhotonNetwork.NetworkingClient.EventReceived -= PlayerWonCheck_EventReceived;
        }
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            else
            {
                Instance = this;
            }

        }
        void Start()
        {
            Init();
        }
        #endregion

        private void Init()
        {
            CreateNetworkPlayer();
            StartCoroutine(AllPlayersReadyCheck());
            StartCoroutine(PlayerWonCheck());
        }

        public GameObject CreateNetworkPlayer()
        {
            return PhotonNetwork.Instantiate(StringConstant.NETWORK_PLAYER, new Vector3(0, 0, 0), Quaternion.identity, 0);
        }

        private IEnumerator AllPlayersReadyCheck()
        {
            yield return new WaitUntil(() => PlayersReady.Count == PhotonNetwork.CurrentRoom.MaxPlayers);

            yield return new WaitForSeconds(1f);
            AllPlayersReady = true;

            Debug.Log("AllPlayersReady " + AllPlayersReady);

            StartCoroutine(MatchStartingCheck());

        }
        private IEnumerator MatchStartingCheck()
        {
            yield return new WaitUntil(() => AllPlayersReady);

            matchStartTimerProperties[StringConstant.START_MATCH_TIMER] = 5f;
            matchStartTimerProperties[StringConstant.START_INCOME_TIMER] = 10;

            PhotonNetwork.CurrentRoom.SetCustomProperties(matchStartTimerProperties);

            MatchStarting = true;
        }
        public void StartMatch()
        {
            MobileCameraControls.Instance.DisableMobileControls = false; //move
            MatchStarted = true;

            Debug.Log("MatchStarted!");
        }

        #region MatchNetworkEvents

        public void MatchEnd_Event() 
        {
            var playersLost = 0;

            foreach (var player in PlayersInGame)
            {
                if (player.GetComponent<PlayerMatchData>().PlayerLives == 0)
                {
                    playersLost++;
                }

                if (playersLost == 1)
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

                foreach (var player in PlayersInGame)
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

            for (int i = 0; i < PlayersInGame.Count; i++)
            {
                var player = PlayersInGame[i];

                if (player.PlayerMatchData.PlayerLives != 0)
                {
                    VictorId = player.photonView.OwnerActorNr;
                    Debug.Log(PhotonNetwork.CurrentRoom.GetPlayer(VictorId).NickName + " is victorious!");

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
            }
        }

        #endregion

    }
}

