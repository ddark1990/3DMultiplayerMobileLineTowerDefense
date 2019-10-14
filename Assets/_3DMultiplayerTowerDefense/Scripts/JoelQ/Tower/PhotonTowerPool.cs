using UnityEngine;

namespace JoelQ.GameSystem.Tower {
    [System.Serializable]
    public class PhotonTowerPool : GenericPool<PhotonTower> {
        [SerializeField] private TowerData data = default;
        public TowerData Data => data;
    }
}