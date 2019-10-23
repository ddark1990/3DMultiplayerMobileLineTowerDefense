#pragma warning disable CS0649
using UnityEngine;

namespace JoelQ.GameSystem.Tower {
    public class Node {
        public bool buildable;
        public Vector3 worldPosition;
        public Node(bool _buildable, Vector3 _worldPos) {
            buildable = _buildable;
            worldPosition = _worldPos;
        }
    }
}