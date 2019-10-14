using UnityEngine;

namespace JoelQ.GameSystem.Tower {
    public class TowerManager : MonoBehaviour {

        [SerializeField] private TowerPool[] towers = default;
        public TowerPool[] Towers => towers;

        private void Awake() {
            foreach (TowerPool tower in towers)
                tower.InitializePool();
        }
    }
}