#pragma warning disable CS0649
using System;
using UnityEngine;
namespace JoelQ.GameSystem.Tower {
    public class TowerProjectile : MonoBehaviour, IPoolable<TowerProjectile> {
        [SerializeField] protected float speed;
        public event Action<TowerProjectile> OnReturnPoolEvent;
        private Collider target;

        public void Setup(Collider target) {
            this.target = target;
        }

        private void OnCollisionEnter(Collision other) {
            if(other.collider == target) {
                OnReturnPoolEvent.Invoke(this);
            }
        }
    }
}