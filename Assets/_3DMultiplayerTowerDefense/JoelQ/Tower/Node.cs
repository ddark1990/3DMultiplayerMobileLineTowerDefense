#pragma warning disable CS0649
using UnityEngine;

namespace JoelQ.GameSystem.Tower {
    public class Node {
        public bool buildable;
        public bool walkable;
        public Vector3 worldPosition;
        public Node(bool _walkable, Vector3 _worldPos) {
            buildable = _walkable;
            walkable = _walkable;
            worldPosition = _worldPos;
        }
    }
}