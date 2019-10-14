using System;
using UnityEngine;
namespace JoelQ.GameSystem.Tower {

    public class Tower : MonoBehaviour, IPoolable<Tower> {

        public event Action<Tower> OnReturnPoolEvent;
        [HideInInspector] public TowerData data;

        public void ReturnToPool() {
            OnReturnPoolEvent.Invoke(this);
        }
    }
}
