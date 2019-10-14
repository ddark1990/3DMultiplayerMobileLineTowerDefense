using System;
using UnityEngine;
using UnityEngine.EventSystems;
namespace JoelQ.GameSystem.Tower {

    public class TowerUI : MonoBehaviour, IPointerClickHandler {

        public static Action<Tower> OnClick;

        public void OnPointerClick(PointerEventData eventData) {
            OnClick.Invoke(GetComponent<Tower>());
        }
    }
}