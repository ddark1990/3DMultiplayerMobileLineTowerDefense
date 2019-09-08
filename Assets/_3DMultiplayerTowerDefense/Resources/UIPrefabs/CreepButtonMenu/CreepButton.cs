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
    public float _sendRefreshRate;
    public float Timer;

    private void Start()
    {
        _maxSendLimit = int.Parse(Ui.SendLimitText.text);
        _sendLimit = _maxSendLimit;
        _sendRefreshRate = Ui.RefreshSendRate;
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
            Timer = _sendRefreshRate;
        }
    }

    private void IncrementSendLimit()
    {
        if (_sendLimit == _maxSendLimit) return;

        Timer -= Time.deltaTime;

        if (!(Timer <= 0)) return;

        _sendLimit++;
        Timer = _sendRefreshRate;
    }
}
