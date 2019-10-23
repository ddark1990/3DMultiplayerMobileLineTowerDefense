#pragma warning disable CS0649
using UnityEngine;
namespace JoelQ.GameSystem.Tower {

    [CreateAssetMenu(fileName = "XData", menuName = "JoelQ/Tower Builder/Create new tower")]
    public sealed class TowerData : ScriptableObject {
        [SerializeField] private new string name;
        [SerializeField, TextArea] private string tooltip;
        [SerializeField] private Sprite icon;
        [SerializeField] private Sprite costIcon;
        [SerializeField] private int cost;
        [SerializeField] private int sellCost;
        [SerializeField] private float damage;
        [SerializeField] private float fireRate;
        [SerializeField] private float range;
        [SerializeField] private TargetType type;
        [SerializeField] private int targetCount;
        [SerializeField] private LayerMask targetMask;
        [Header("Build Effect")]
        [SerializeField] private int buildEffect;
        [SerializeField] private int buildSFX;
        [Header("Projectile"), Space]
        [SerializeField] private int projectile;
        [SerializeField] private float projectileSpeed;
        [SerializeField] private int shootEffect;
        [SerializeField] private int impactEffect;
        [SerializeField] private int shootSFX;
        [SerializeField] private int impactSFX;

        public string Name => name;
        public string ToolTip => tooltip;
        public Sprite Icon => icon;
        public Sprite CostIcon => costIcon;
        public int Cost => cost;
        public int SellCost => sellCost;
        public float Damage => damage;
        public float FireRate => fireRate;
        public float Range => range;
        public TargetType Type => type;
        public int TargetCount => targetCount;
        public LayerMask TargetMask => targetMask;
        public float ProjectileSpeed => projectileSpeed;
        public int Projectile => projectile;

        public enum TargetType {
            Target,
            AOE
        }
    }
}