#pragma warning disable CS0649
using UnityEngine;
namespace JoelQ.GameSystem.Tower {
    public class TowerManager : MonoBehaviour {

        [SerializeField] private TowerPool[] towers;
        [SerializeField] private TowerProjectilePool[] projectiles;
        public TowerPool[] Towers => towers;

        private void Awake() {
            foreach (TowerPool tower in towers)
                tower.InitializePool();
            foreach (TowerProjectilePool projectile in projectiles)
                projectile.InitializePool();
        }
    }
}