#pragma warning disable CS0649
using UnityEngine;
namespace JoelQ.GameSystem.Tower {
    public class TowerManager : MonoBehaviour {

        [SerializeField] private TowerManagerData data;
        public TowerPool[] Towers => data.Towers;
        public TowerProjectilePool[] Projectiles => data.Projectiles;

        private void Awake() {
            foreach (TowerPool tower in Towers) {
                tower.InitializePool();
                tower.projectilePool = Projectiles[tower.Data.Projectile];
            }
            foreach (TowerProjectilePool projectile in Projectiles)
                projectile.InitializePool();
        }
    }
}