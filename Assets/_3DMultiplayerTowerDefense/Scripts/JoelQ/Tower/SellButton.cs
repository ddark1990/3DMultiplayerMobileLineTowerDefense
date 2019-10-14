using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
namespace JoelQ.GameSystem.Tower {

    public class SellButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerClickHandler {
        
        [SerializeField] private Image icon = default;
        [SerializeField] private Image costIcon = default;
        [SerializeField] private TextMeshProUGUI costText = default;
        public event Action<TowerData> OnClick;
        private TowerData data;

        public void Setup(TowerData data) {
            icon.sprite = data.Icon;
            costIcon.sprite = data.CostIcon;
            costText.text = data.Cost.ToString();
            this.data = data;
        }

        public void OnPointerDown(PointerEventData eventData) {
            ToolTipSystem.OnOpenToolTip.Invoke(data.ToolTip);
        }

        public void OnPointerUp(PointerEventData eventData) {
            ToolTipSystem.OnCloseToolTip.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData) {
            ToolTipSystem.OnCloseToolTip.Invoke();
            OnClick.Invoke(data);
        }

        public void OnPointerExit(PointerEventData eventData) {
            ToolTipSystem.OnCloseToolTip.Invoke();
        }
    }
}
