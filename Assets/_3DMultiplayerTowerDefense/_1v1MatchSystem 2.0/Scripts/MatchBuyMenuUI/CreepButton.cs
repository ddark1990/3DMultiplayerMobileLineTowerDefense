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
        [HideInInspector] public CreepSender creepSender;
        private float timer;
        public int sendLimit;
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

            Button.onClick.AddListener(() => creepSender.SendCreep(NameText.text)); //set buttons send creep event
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

    }
}