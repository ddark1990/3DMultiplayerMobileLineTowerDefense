using UnityEngine;

namespace JoelQ.GameSystem.Tower {
    [CreateAssetMenu(fileName = "TowerDataList", menuName = "JoelQ/Tower Builder/Create new tower list")]
    public class TowerDataList : ScriptableObject {
        [SerializeField] private TowerData[] towers = default;
        public TowerData[] Towers => towers;
    }
}