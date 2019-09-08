using System;
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

    private int _maxSendLimit;
    private int _sendLimit;
    private float _sendRefreshRate;
    private float _startTimer;
    public float Timer;

    private void Start()
    {
        _maxSendLimit = int.Parse(Ui.SendLimitText.text); //initialize values from UI
        _sendLimit = _maxSendLimit;

        _sendRefreshRate = Ui.RefreshSendRate;
        Timer = _sendRefreshRate;
        _startTimer = Timer;
    }

    private void FixedUpdate()
    {
        ToggleInteractble();
    }

    private void Update()
    {
        IncrementSendLimit();
    }

    private void ToggleInteractble()
    {
        Ui.SendLimitText.text = _sendLimit.ToString();

        Button.interactable = Ui.SendLimitText.text != "0";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(eventData);

        if (_sendLimit > 0)
        {
            _sendLimit--;
        }
    }

    private void IncrementSendLimit()
    {
        if (_sendLimit == _maxSendLimit) return;

        Timer -= Time.deltaTime;

        UpdateRefreshBar();

        if (!(Timer <= 0)) return;

        _sendLimit++;
        Timer = _sendRefreshRate;
    }

    private void UpdateRefreshBar()
    {
        Ui.RefreshBarFilled.fillAmount = Timer / _startTimer;
    }
}
