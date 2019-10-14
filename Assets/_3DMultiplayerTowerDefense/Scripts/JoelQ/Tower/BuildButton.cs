using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace JoelQ.GameSystem.Tower {
    public class BuildButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerClickHandler {

        [SerializeField] private Image icon = default;
        [SerializeField] private Image costIcon = default;
        [SerializeField] private TextMeshProUGUI costText = default;
        [SerializeField] private GameObject toolTipPanel = default;
        [SerializeField] private TextMeshProUGUI toolTipText = default;
        private int id;
        private Action<int> OnClickEvent;
        private Coroutine toolTipCoroutine;

        public void SetupButton(TowerData data, int id, Action<int> BuildMethod) {
            icon.sprite = data.Icon;
            costIcon.sprite = data.CostIcon;
            costText.text = data.Cost.ToString();
            toolTipText.text = data.ToolTip;
            this.id = id;
            OnClickEvent = BuildMethod;
        }

        public void OnPointerDown(PointerEventData eventData) {
            if (toolTipCoroutine == null)
                toolTipCoroutine = StartCoroutine(OpenToolTip());
        }

        public void OnPointerUp(PointerEventData eventData) {
            if(toolTipCoroutine != null)
                StopCoroutine(toolTipCoroutine);
        }

        public void OnPointerClick(PointerEventData eventData) {
            OnClickEvent.Invoke(id);
        }

        public void OnPointerExit(PointerEventData eventData) {
            if (toolTipCoroutine != null)
                StopCoroutine(toolTipCoroutine);
        }

        private IEnumerator OpenToolTip() {
            yield return new WaitForSeconds(1f);
            toolTipPanel.SetActive(true);
        }
    }
}