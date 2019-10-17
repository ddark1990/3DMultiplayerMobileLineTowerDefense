#pragma warning disable CS0649
using UnityEngine;
namespace JoelQ.GameSystem.Tower {
    [CreateAssetMenu(fileName = "TowerPoolList", menuName = "JoelQ/Tower System/Create new Tower Pool List")]
    public class TowerPoolList : ScriptableObject {
        [SerializeField] private TowerPool[] towers;
        public TowerPool[] Towers => towers;
    }
}