using UnityEngine;
namespace JoelQ.GameSystem.Tower {

    [CreateAssetMenu(fileName = "XData", menuName = "JoelQ/Tower Builder/Create new tower")]
    public sealed class TowerData : ScriptableObject {
        [SerializeField] private new string name = default;
        [SerializeField, TextArea] private string tooltip = default;
        [SerializeField] private Sprite icon = default;
        [SerializeField] private Sprite costIcon = default;
        [SerializeField] private int cost = default;
        [SerializeField] private int sellCost = default;
        [SerializeField] private float damage = default;
        [SerializeField] private float fireRate = default;
        [SerializeField] private float range = default;
        [Header("Build Effect")]
        [SerializeField] private int buildEffect = default;
        [SerializeField] private int buildSFX = default;
        [Header("Projectile"), Space]
        [SerializeField] private float projectileSpeed = default;
        [SerializeField] private int projectile = default;
        [SerializeField] private int shootEffect = default;
        [SerializeField] private int impactEffect = default;
        [SerializeField] private int shootSFX = default;
        [SerializeField] private int impactSFX = default;

        public string Name => name;
        public string ToolTip => tooltip;
        public Sprite Icon => icon;
        public Sprite CostIcon => costIcon;
        public int Cost => cost;
        public int SellCost => sellCost;
        public float Damage => damage;
        public float FireRate => fireRate;
        public float Range => range;
        public float ProjectileSpeed => projectileSpeed;
        public int Projectile => projectile;
    }
}