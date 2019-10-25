using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Node2
{
    public class NodeController : MonoBehaviour
    {
        public static NodeController Instance;

        public List<Node> Nodes;

        public Node CurrentlySelectedNode;
        public Color HoverColor;

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
            rend = CurrentlySelectedNode.GetComponentInChildren<Renderer>();
            StartColor = rend.material.color;

            Color lerpedColor = Color.Lerp(rend.material.color, HoverColor, Time.deltaTime * 5);
            rend.material.color = Color.white;

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