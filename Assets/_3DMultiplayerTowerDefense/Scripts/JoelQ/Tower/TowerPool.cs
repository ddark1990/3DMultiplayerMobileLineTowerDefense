using UnityEngine;

namespace JoelQ.GameSystem.Tower {
    [System.Serializable]
    public class TowerPool : GenericPool<Tower> {
        [SerializeField] private TowerData data = default;
        public TowerData Data => data;
    }
}