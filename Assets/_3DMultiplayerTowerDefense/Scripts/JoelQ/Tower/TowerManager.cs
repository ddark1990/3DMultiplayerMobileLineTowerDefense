#pragma warning disable CS0649
using UnityEngine;
namespace JoelQ.GameSystem.Tower {
    public class TowerManager : MonoBehaviour {

        [SerializeField] private TowerPoolList towerList;
        [SerializeField] private TowerProjectilePoolList projectileList;
        public TowerPool[] Towers => towerList.Towers;
        public TowerProjectilePool[] Projectiles => projectileList.Projectiles;

        private void Awake() {
            foreach (TowerPool tower in Towers)
                tower.InitializePool();
            foreach (TowerProjectilePool projectile in Projectiles)
                projectile.InitializePool();
        }
    }
}