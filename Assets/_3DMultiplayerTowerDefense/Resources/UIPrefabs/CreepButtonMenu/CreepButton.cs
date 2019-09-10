﻿using System;
using System.Collections;
using System.Collections.Generic;
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
    public int _creepCost;
    private float _sendRefreshRate;

    public float Timer;

    private void Init() //initialize values from UI
    {
        _maxSendLimit = int.Parse(Ui.SendLimitText.text); 
        _sendLimit = _maxSendLimit;

        _player = GetComponentInParent<CreepSender>().Owner; //get owner of the creepsender that's connected to the button
        _creepCost = int.Parse(Ui.costText.text); 

        _sendRefreshRate = Ui.RefreshSendRate;
        Timer = _sendRefreshRate;

    }

    private void Start()
    {
        Init();
    }

    private void FixedUpdate()
    {
        ToggleInteractable();
    }

    private void Update()
    {
        IncrementSendLimit();
    }

    private void ToggleInteractable()
    {
        Ui.SendLimitText.text = _sendLimit.ToString();

        if (_sendLimit == 0 || !AffordItem(_player.GetComponent<PlayerMatchData>().PlayerGold, _creepCost))
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

        if (_sendLimit <= 0 || !AffordItem(_player.GetComponent<PlayerMatchData>().PlayerGold, _creepCost))
        {
            Debug.Log(_player + " ran out of gold.");
            return;
        }

        _sendLimit--;
        BuyCreep(_creepCost);
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

    public void BuyCreep(int creepCost)
    {
        _player.GetComponent<PlayerMatchData>().PlayerGold -= creepCost;
    }

    private static bool AffordItem(int goldAmount, int cost)
    {
        return goldAmount >= cost;
    }
}
