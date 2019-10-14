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
        private GameObject toolTipPanel = default;
        private TextMeshProUGUI toolTipText = default;
        private int id;
        private Action<int> OnClickEvent;
        private Coroutine toolTipCoroutine;
        private PointerEventData mouse;
        private string toolTip;

        public void SetupButton(TowerData data, int id, Action<int> BuildCallback, GameObject toolTipPanel) {
            //Data
            icon.sprite = data.Icon;
            costIcon.sprite = data.CostIcon;
            costText.text = data.Cost.ToString();
            toolTip = data.ToolTip;
            this.id = id;
            OnClickEvent = BuildCallback;
            //Tooltip
            this.toolTipPanel = toolTipPanel;
            toolTipText = toolTipPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }

        private void Update() {
            if(toolTipPanel.activeSelf && mouse != null) {
                toolTipPanel.transform.position = mouse.position;
            }
        }

        public void OnPointerDown(PointerEventData eventData) {
            if (toolTipCoroutine == null) { 
                toolTipCoroutine = StartCoroutine(OpenToolTip()); 
            }
            mouse = eventData;
        }

        public void OnPointerUp(PointerEventData eventData) {
            CloseToolTip();
        }

        public void OnPointerClick(PointerEventData eventData) {
            CloseToolTip();
            OnClickEvent.Invoke(id);
        }

        public void OnPointerExit(PointerEventData eventData) {
            CloseToolTip();
        }

        private IEnumerator OpenToolTip() {
            toolTipText.text = toolTip;
            yield return new WaitForSeconds(1f);
            toolTipPanel.SetActive(true);
            toolTipCoroutine = null;
        }

        private void CloseToolTip() {
            if (toolTipCoroutine != null) {
                StopCoroutine(toolTipCoroutine);
                toolTipCoroutine = null;
            }
            toolTipPanel.SetActive(false);
        }
    }
}