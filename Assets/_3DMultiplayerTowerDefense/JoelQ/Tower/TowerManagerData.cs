#pragma warning disable CS0649
using UnityEngine;
namespace JoelQ.GameSystem.Tower {
    [CreateAssetMenu(fileName = "Tower Data Management", menuName = "JoelQ/Tower Management/Create new Management")]
    public class TowerManagerData : ScriptableObject {
        [SerializeField] private TowerPool[] towers;
        [SerializeField] private TowerProjectilePool[] projectiles;
        public TowerPool[] Towers => towers;
        public TowerProjectilePool[] Projectiles => projectiles;
    }
}