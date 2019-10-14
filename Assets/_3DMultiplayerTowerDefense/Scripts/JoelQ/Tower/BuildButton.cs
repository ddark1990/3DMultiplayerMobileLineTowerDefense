﻿using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
namespace JoelQ.GameSystem.Tower {

    public class BuildButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerClickHandler {

        [SerializeField] private Image icon = default;
        [SerializeField] private Image costIcon = default;
        [SerializeField] private TextMeshProUGUI costText = default;
        private int id;
        public event Action<int> OnClick;
        //Tooltip
        private string toolTip;

        public void Setup(TowerData data, int id) {
            //Data
            icon.sprite = data.Icon;
            costIcon.sprite = data.CostIcon;
            costText.text = data.Cost.ToString();
            toolTip = data.ToolTip;
            this.id = id;
        }

        public void OnPointerDown(PointerEventData eventData) {
            ToolTipSystem.OnOpenToolTip.Invoke(toolTip);
        }

        public void OnPointerUp(PointerEventData eventData) {
            ToolTipSystem.OnCloseToolTip.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData) {
            ToolTipSystem.OnCloseToolTip.Invoke();
            OnClick.Invoke(id);
        }

        public void OnPointerExit(PointerEventData eventData) {
            ToolTipSystem.OnCloseToolTip.Invoke();
        }
    }
}