using System;
using System.Collections;
using UnityEngine;
using TMPro;
namespace JoelQ.GameSystem.Tower {
    public class ToolTipSystem : MonoBehaviour {
        [SerializeField] private GameObject toolTipPanel = default;
        [SerializeField] private TextMeshProUGUI toolTipText = default;
        public static Action<string> OnOpenToolTip;
        public static Action OnCloseToolTip;
        private Coroutine toolTipCoroutine;

        private void Awake() {
            OnOpenToolTip = OpenTooltip;
            OnCloseToolTip = CloseToolTip;
        }

        private void Update() {
            if (toolTipPanel.activeSelf) {
                toolTipPanel.transform.position = Input.mousePosition;
            }
        }

        public void OpenTooltip(string toolTip) {
            if (toolTipCoroutine == null) {
                toolTipCoroutine = StartCoroutine(OpenToolTip(toolTip));
            }
        }

        public void CloseToolTip() {
            if (toolTipCoroutine != null) {
                StopCoroutine(toolTipCoroutine);
                toolTipCoroutine = null;
            }
            toolTipPanel.SetActive(false);
        }

        private IEnumerator OpenToolTip(string toolTip) {
            toolTipText.text = toolTip;
            yield return new WaitForSeconds(1f);
            toolTipPanel.SetActive(true);
            toolTipCoroutine = null;
        }

        private void OnDestroy() {
            OnOpenToolTip = null;
            OnCloseToolTip = null;
        }
    }
}
