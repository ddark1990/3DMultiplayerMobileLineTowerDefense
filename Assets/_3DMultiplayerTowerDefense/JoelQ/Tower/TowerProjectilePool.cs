#pragma warning disable CS0649
using UnityEngine;
namespace JoelQ.GameSystem.Tower {
    [System.Serializable]
    public class TowerProjectilePool : GenericPool<TowerProjectile> {
        public TowerProjectile TowerProjectile => prefab;
    }
}