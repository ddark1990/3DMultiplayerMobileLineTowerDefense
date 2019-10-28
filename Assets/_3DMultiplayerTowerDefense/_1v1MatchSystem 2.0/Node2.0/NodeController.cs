using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatchSystem
{
    public class NodeController : MonoBehaviour //convert to static
    {
        public static NodeController Instance;

        public Node SelectedNode;
        public Color SelectedColor;

        private Renderer rend;
        private Color StartColor;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            else
            {
                Instance = this;
            }

        }

        #region NodeAnimation

        public void HighlightNode()
        {
            rend = SelectedNode.GetComponentInChildren<Renderer>();
            StartColor = rend.material.color;

            Color lerpedColor = Color.Lerp(rend.material.color, SelectedColor, Time.deltaTime * 5);
            rend.material.color = SelectedColor;

            //Debug.Log("HighLighted Node");
        }
        public void DehighlightNode()
        {
            rend.material.color = StartColor/*Color.Lerp(rend.material.color, StartColor, Time.deltaTime * 5)*/;

            //Debug.Log("DehighLighted Node");
        }

        #endregion
    }
}