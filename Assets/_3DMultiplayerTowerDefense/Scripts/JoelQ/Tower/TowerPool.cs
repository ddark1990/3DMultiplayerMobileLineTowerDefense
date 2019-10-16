#pragma warning disable CS0649
using UnityEngine;

namespace JoelQ.GameSystem.Tower {
    [System.Serializable]
    public class TowerPool : GenericPool<Tower> {
        [SerializeField] private TowerData data;
        public TowerData Data => data;
    }
}