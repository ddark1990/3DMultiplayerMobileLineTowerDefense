#pragma warning disable CS0649
using UnityEngine;
namespace JoelQ.GameSystem.Tower {
    [CreateAssetMenu(fileName = "ProjectilePoolList", menuName = "JoelQ/Tower System/Create new Projectile Pool List")]
    public class TowerProjectilePoolList : ScriptableObject {
        [SerializeField] private TowerProjectilePool[] projectiles;
        public TowerProjectilePool[] Projectiles => projectiles;
    }
}