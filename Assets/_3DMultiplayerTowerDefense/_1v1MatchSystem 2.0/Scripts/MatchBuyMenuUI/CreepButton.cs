using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;

namespace MatchSystem
{
    //public class CreepButton : MonoBehaviour, IPointerClickHandler
    //{
    //    public Button Button;
    //    public CreepButtonUI Ui;

    //    private PhotonPlayer _player;
    //    private int _maxSendLimit;
    //    private int _sendLimit;
    //    private int _creepCost;
    //    private int _creepIncome;
    //    private float _sendRefreshRate;
    //    private PhotonView _creepSenderPv;

    //    public float Timer;

    //    private void Init() //initialize values from UI
    //    {
    //        _maxSendLimit = int.Parse(Ui.SendLimitText.text);
    //        _sendLimit = _maxSendLimit;

    //        _player = GetComponentInParent<CreepSender>().Owner; //get _player of the creepsender that's connected to the button
    //        _creepCost = int.Parse(Ui.costText.text);
    //        _creepIncome = int.Parse(Ui.IncomeText.text);

    //        _sendRefreshRate = Ui.RefreshSendRate;
    //        Timer = _sendRefreshRate;

    //        _creepSenderPv = GetComponentInParent<CreepSender>().photonView;
    //    }

    //    private void Start()
    //    {
    //        Init();
    //    }

    //    private void Update()
    //    {
    //        ToggleInteractable();
    //        IncrementSendLimit();
    //    }

    //    private void ToggleInteractable()
    //    {
    //        if (!GameManager.instance.MatchStarted) return;

    //        Ui.SendLimitText.text = _sendLimit.ToString();

    //        //while (_sendLimit > 0)
    //        //{
    //        //    if (_sendLimit == 0 || !CanAffordItem(_player.GetComponent<PlayerMatchData>().PlayerGold, _creepCost))
    //        //    {
    //        //        Button.interactable = false;
    //        //        continue;
    //        //    }

    //        //    Button.interactable = true;
    //        //}

    //        if (GameManager.instance.MatchEnd) return;

    //        if (_sendLimit == 0 || !CanAffordItem(_player.GetComponent<PlayerMatchData>().PlayerGold, _creepCost))
    //        {
    //            Button.interactable = false;
    //        }
    //        else
    //        {
    //            Button.interactable = true;
    //        }
    //    }

    //    public void OnPointerClick(PointerEventData eventData)
    //    {
    //        if (!Button.IsInteractable()) return;

    //        Debug.Log(eventData);

    //        if (!CanAffordItem(_player.GetComponent<PlayerMatchData>().PlayerGold, _creepCost))
    //        {
    //            Debug.Log(_player + " can't afford creep.");
    //            return;
    //        }

    //        _sendLimit--;
    //        _creepSenderPv.RPC("RPC_BuyCreep", RpcTarget.AllViaServer, _creepCost, _creepIncome);
    //    }

    //    private void IncrementSendLimit()
    //    {
    //        if (_sendLimit == _maxSendLimit) return;

    //        Timer -= Time.deltaTime;

    //        Ui.RefreshBarFilled.fillAmount += Time.deltaTime / _sendRefreshRate;

    //        if (!(Timer <= 0)) return;

    //        _sendLimit++;
    //        Timer = _sendRefreshRate;
    //        Ui.RefreshBarFilled.fillAmount = 0;
    //    }

    //    private static bool CanAffordItem(int goldAmount, int cost)
    //    {
    //        return goldAmount >= cost;
    //    }

    //}

    public class CreepButton : MonoBehaviour
    {
        public Image Img;
        public Button Button;
        public TextMeshProUGUI CostText;
        public TextMeshProUGUI HealthText;
        public TextMeshProUGUI DefenseText;
        public TextMeshProUGUI NameText;
        public TextMeshProUGUI IncomeText;
        public TextMeshProUGUI SendLimitText;
        public Image RefreshBarFilled;

        [HideInInspector] public float RefreshSendRate;
        private float timer;
        private int sendLimit;
        private int maxSendLimit;

        private void Start()
        {
            Init();
        }

        private void Update()
        {
            IncrementSendLimit();
            SendLimitText.text = sendLimit.ToString();
        }

        private void Init()
        {
            timer = RefreshSendRate;

            maxSendLimit = int.Parse(SendLimitText.text);
            sendLimit = maxSendLimit;
        }

        private void IncrementSendLimit()
        {
            if (sendLimit == maxSendLimit) return;

            timer -= Time.deltaTime;

            RefreshBarFilled.fillAmount += Time.deltaTime / RefreshSendRate;

            if (!(timer <= 0)) return;

            sendLimit++;
            timer = RefreshSendRate;
            RefreshBarFilled.fillAmount = 0;
        }

        public void OnButtonPress()
        {
            if (sendLimit == 0)
            {
                Button.interactable = false; //maybe
                return;
            }

            sendLimit--;

            //Button.onClick.AddListener(() => ); //send creep event
        }

    }
}