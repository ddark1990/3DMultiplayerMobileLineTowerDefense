using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

namespace MatchSystem
{
    public class PlayerMatchData : MonoBehaviourPunCallbacks
    {
        public int PlayerLives, PlayerGold, PlayerIncome, CreepsKilled, TowersBuilt, CreepsSent;
        public float IncomeTime;
        private int startIncomeTimer = 10;

        public bool DebugLogNetworkEvents;

        public const string START_INCOME_TIMER = "StartIncomeTimer";

        public NetworkPlayer NetworkOwner;

        private new void OnEnable()
        {
            if (!photonView.IsMine) return;

            Init();

            PhotonNetwork.NetworkingClient.EventReceived += DeductPlayerLife_EventReceived;
            PhotonNetwork.NetworkingClient.EventReceived += AnnounceLoss_EventRecieved;
            PhotonNetwork.NetworkingClient.EventReceived += IncreasePlayerGold_EventRecieved;
            PhotonNetwork.NetworkingClient.EventReceived += DeductPlayerGold_EventRecieved;
            PhotonNetwork.NetworkingClient.EventReceived += IncreasePlayerIncome_EventRecieved;

        }
        public new void OnDisable()
        {
            PhotonNetwork.NetworkingClient.EventReceived -= DeductPlayerLife_EventReceived;
            PhotonNetwork.NetworkingClient.EventReceived -= AnnounceLoss_EventRecieved;
            PhotonNetwork.NetworkingClient.EventReceived -= IncreasePlayerGold_EventRecieved;
            PhotonNetwork.NetworkingClient.EventReceived -= DeductPlayerGold_EventRecieved;
            PhotonNetwork.NetworkingClient.EventReceived -= IncreasePlayerIncome_EventRecieved;
        }

        private void Update()
        {
            IncomeTimer();
        }

        private void Init()
        {
            photonView.RPC("RPC_StartingPlayerData", RpcTarget.AllViaServer);
            StartCoroutine(TimedInit());

        }
        private IEnumerator TimedInit()
        {
            yield return new WaitUntil(() => MatchManager.Instance.MatchStarted);

            startIncomeTimer = (int)PhotonNetwork.CurrentRoom.CustomProperties[START_INCOME_TIMER];

        }

        private void IncomeTimer()
        {
            if (!MatchManager.Instance.MatchStarted) return;

            IncomeTime -= Time.deltaTime;

            if (IncomeTime <= 0)
            {
                IncreasePlayerGold_Event(PlayerIncome);

                IncomeTime = startIncomeTimer;
                return;
            }
        }

        #region RPC's

        [PunRPC]
        private void RPC_StartingPlayerData()
        {
            PlayerLives = 50;
            PlayerGold = 20;
            PlayerIncome = 5;
        }

        #endregion

        #region NetworkEvents
        /// <summary> Deduct player's lives over the network. </summary>
        public void DeductPlayerLife_Event()
        {
            if (NetworkOwner.PlayerLost || MatchManager.Instance.MatchEnd || PlayerLives == 0)
            {
                Debug.Log(NetworkOwner.PlayerName + " has ran out of lives!");
                AnnounceLoss_Event();
                return;
            }

            PlayerLives--;

            object[] sendPlayerLivesData = new object[] { photonView.ViewID, PlayerLives };

            RaiseEventOptions options = new RaiseEventOptions()
            {
                CachingOption = EventCaching.DoNotCache,
                Receivers = ReceiverGroup.Others
            };

            PhotonNetwork.RaiseEvent((byte)EventIdHandler.EVENT_IDs.PLAYER_DEDUCT_LIFE, sendPlayerLivesData, options, SendOptions.SendUnreliable);
        }
        private void DeductPlayerLife_EventReceived(EventData obj)
        {
            if (obj.Code == (byte)EventIdHandler.EVENT_IDs.PLAYER_DEDUCT_LIFE)
            {
                object[] data = (object[])obj.CustomData;

                if ((int)data[0] != photonView.ViewID) return;

                var playerLives = (int)data[1];

                if (playerLives == 0) return;

                playerLives--;
            }
        }

        /// <summary> Announce when the player has lost all his lives over the network. </summary>
        public void AnnounceLoss_Event()
        {
            Debug.Log(NetworkOwner.PlayerName + " has lost the match!");
            NetworkOwner.PlayerLost = true;
            MatchManager.Instance.MatchEnd_Event();

            object[] sendAnnounceLossData = new object[] { photonView.ViewID, NetworkOwner.PlayerLost };

            RaiseEventOptions options = new RaiseEventOptions()
            {
                CachingOption = EventCaching.DoNotCache,
                Receivers = ReceiverGroup.Others
            };

            PhotonNetwork.RaiseEvent((byte)EventIdHandler.EVENT_IDs.PLAYER_ANNOUNCE_LOSS, sendAnnounceLossData, options, SendOptions.SendUnreliable);

        }
        private void AnnounceLoss_EventRecieved(EventData obj)
        {
            if (obj.Code == (byte)EventIdHandler.EVENT_IDs.PLAYER_ANNOUNCE_LOSS)
            {
                object[] data = (object[])obj.CustomData;

                if ((int)data[0] != photonView.ViewID) return;

                var playerLost = (bool)data[1];

                playerLost = true;
            }
        }

        /// <summary> Increase players gold over the network. </summary>
        public void IncreasePlayerGold_Event(int increaseAmmount)
        {
            PlayerGold += increaseAmmount;

            if(DebugLogNetworkEvents)
            {
                Debug.Log("Increased " + NetworkOwner.PlayerName + " gold by " + increaseAmmount + ".");
            }

            object[] sendIncreasedPlayerGoldData = new object[] { photonView.ViewID, PlayerGold, increaseAmmount };

            RaiseEventOptions options = new RaiseEventOptions()
            {
                CachingOption = EventCaching.DoNotCache,
                Receivers = ReceiverGroup.Others
            };

            PhotonNetwork.RaiseEvent((byte)EventIdHandler.EVENT_IDs.PLAYER_INCREASE_GOLD, sendIncreasedPlayerGoldData, options, SendOptions.SendUnreliable);
        }
        private void IncreasePlayerGold_EventRecieved(EventData obj)
        {
            if (obj.Code == (byte)EventIdHandler.EVENT_IDs.PLAYER_INCREASE_GOLD)
            {
                object[] data = (object[])obj.CustomData;

                if ((int)data[0] != photonView.ViewID) return;

                var playerGold = (int)data[1];
                var increaseAmmount = (int)data[2];

                playerGold += increaseAmmount;
            }
        }

        /// <summary> Decrease players gold over the network. </summary>
        public void DeductPlayerGold_Event(int deductAmmount)
        {
            if (PlayerGold == 0) return;
        
            PlayerGold -= deductAmmount;

            if(DebugLogNetworkEvents)
            {
                Debug.Log("Deducted " + NetworkOwner.PlayerName + " gold by " + deductAmmount + ".");
            }

            object[] sendDeductedPlayerGoldData = new object[] { photonView.ViewID, PlayerGold, deductAmmount };

            RaiseEventOptions options = new RaiseEventOptions()
            {
                CachingOption = EventCaching.DoNotCache,
                Receivers = ReceiverGroup.Others
            };

            PhotonNetwork.RaiseEvent((byte)EventIdHandler.EVENT_IDs.PLAYER_DEDUCT_GOLD, sendDeductedPlayerGoldData, options, SendOptions.SendUnreliable);
        }
        private void DeductPlayerGold_EventRecieved(EventData obj)
        {
            if (obj.Code == (byte)EventIdHandler.EVENT_IDs.PLAYER_DEDUCT_GOLD)
            {
                object[] data = (object[])obj.CustomData;

                if ((int)data[0] != photonView.ViewID) return;

                var playerGold = (int)data[1];
                var deductAmmount = (int)data[2];

                playerGold -= deductAmmount;
            }
        }

        /// <summary> Increase players income over the network. </summary>
        public void IncreasePlayerIncome_Event(int increaseAmmount)
        {
            PlayerIncome += increaseAmmount;

            if(DebugLogNetworkEvents)
            {
                Debug.Log("Increased " + NetworkOwner.PlayerName + " income by " + increaseAmmount + ".");
            }

            object[] sendIncreasedIncomeData = new object[] { photonView.ViewID, PlayerIncome, increaseAmmount };

            RaiseEventOptions options = new RaiseEventOptions()
            {
                CachingOption = EventCaching.DoNotCache,
                Receivers = ReceiverGroup.Others
            };

            PhotonNetwork.RaiseEvent((byte)EventIdHandler.EVENT_IDs.PLAYER_INCREASE_INCOME, sendIncreasedIncomeData, options, SendOptions.SendUnreliable);
        }
        private void IncreasePlayerIncome_EventRecieved(EventData obj)
        {
            if (obj.Code == (byte)EventIdHandler.EVENT_IDs.PLAYER_INCREASE_INCOME)
            {
                object[] data = (object[])obj.CustomData;

                if ((int)data[0] != photonView.ViewID) return;

                var playerIncome = (int)data[1];
                var increaseAmmount = (int)data[2];

                playerIncome += increaseAmmount;
            }
        }

        #endregion

        //player lost logic
        #region PlayerLostAnnounce

        //private IEnumerator PlayerLostAnnounce()
        //{
        //    yield return new WaitUntil(() => photonView.IsMine && PlayerLives.Equals(0));

        //    var matchDataInterface = GetMatchDataInterface();
        //    matchDataInterface.IfPlayerHasZeroLives(_player);
        //}

        //[PunRPC]
        //public void RPC_SendPlayerLost() //Sends over network that this player has lost when has 0 lives
        //{
        //    Debug.Log(photonView.Owner.NickName + " has " + PlayerLives + " lives.");
        //    PlayerLost = true;
        //}

        #endregion

        //[PunRPC]
        //public void RPC_IncreaseGold()
        //{
        //    PlayerGold += PlayerIncome;
        //}

        //private IPlayerMatchData GetMatchDataInterface()
        //{
        //    return GetComponent<IPlayerMatchData>();
        //}

        public bool CanAfford(int cost)
        {
            return PlayerGold >= cost;
        }
    }
}