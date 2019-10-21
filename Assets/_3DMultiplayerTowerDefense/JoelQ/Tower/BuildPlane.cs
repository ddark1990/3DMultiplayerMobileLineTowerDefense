#pragma warning disable CS0649
using System;
using UnityEngine;
using UnityEngine.EventSystems;
namespace JoelQ.GameSystem.Tower {

    public class BuildPlane : MonoBehaviour, IPointerClickHandler {

        public Action<Vector3> OnClickEvent;

        public void OnPointerClick(PointerEventData eventData) {
            OnClickEvent.Invoke(eventData.pointerCurrentRaycast.worldPosition);
        }
    }
}