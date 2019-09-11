using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;

public class CreepButton : MonoBehaviour, IPointerClickHandler
{
    public Button Button;
    public CreepButtonUI Ui;

    private PhotonPlayer _player;
    private int _maxSendLimit;
    private int _sendLimit;
    private int _creepCost;
    private int _creepIncome;
    private float _sendRefreshRate;
    private PhotonView _creepPv;

    public float Timer;

    private void Init() //initialize values from UI
    {
        _maxSendLimit = int.Parse(Ui.SendLimitText.text); 
        _sendLimit = _maxSendLimit;

        _player = GetComponentInParent<CreepSender>().Owner; //get _player of the creepsender that's connected to the button
        _creepCost = int.Parse(Ui.costText.text);
        _creepIncome = int.Parse(Ui.IncomeText.text);

        _sendRefreshRate = Ui.RefreshSendRate;
        Timer = _sendRefreshRate;

        _creepPv = GetComponentInParent<CreepSender>().photonView;
    }

    private void Start()
    {
        Init();
    }

    private void FixedUpdate()
    {
        ToggleInteractable();
        IncrementSendLimit();
    }

    private void ToggleInteractable()
    {
        Ui.SendLimitText.text = _sendLimit.ToString();

        if (_sendLimit == 0 || !CanAffordItem(_player.GetComponent<PlayerMatchData>().PlayerGold, _creepCost))
        {
            Button.interactable = false;
        }
        else
        {
            Button.interactable = true;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!Button.IsInteractable()) return;

        Debug.Log(eventData);

        if (_sendLimit <= 0 || !CanAffordItem(_player.GetComponent<PlayerMatchData>().PlayerGold, _creepCost))
        {
            Debug.Log(_player + " ran out of gold.");
            return;
        }

        _sendLimit--;
        _creepPv.RPC("BuyCreep", RpcTarget.AllViaServer, _creepCost);
        _creepPv.RPC("UpdatePlayerIncome", RpcTarget.AllViaServer, _creepIncome);
    }

    private void IncrementSendLimit()
    {
        if (_sendLimit == _maxSendLimit) return;

        Timer -= Time.deltaTime;

        Ui.RefreshBarFilled.fillAmount += Time.deltaTime / _sendRefreshRate;

        if (!(Timer <= 0)) return;

        _sendLimit++;
        Timer = _sendRefreshRate;
        Ui.RefreshBarFilled.fillAmount = 0;
    }

    private static bool CanAffordItem(int goldAmount, int cost)
    {
        return goldAmount >= cost;
    }
}
