using UnityEngine;

namespace JoelQ.GameSystem.Tower {
    public class TowerManager : MonoBehaviour {

        [SerializeField] private PhotonTowerPool[] towers = default;
        public PhotonTowerPool[] Towers => towers;

        private void Awake() {
            foreach (PhotonTowerPool tower in towers)
                tower.InitializePool();
        }
    }
}