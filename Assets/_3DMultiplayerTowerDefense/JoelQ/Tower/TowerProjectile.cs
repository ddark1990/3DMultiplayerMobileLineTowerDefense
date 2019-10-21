#pragma warning disable CS0649
using System;
using UnityEngine;
namespace JoelQ.GameSystem.Tower {
    public class TowerProjectile : MonoBehaviour, IPoolable<TowerProjectile> {
        [SerializeField] protected float speed;
        public event Action<TowerProjectile> OnReturnPoolEvent;
        private AI target;

        public void Setup(AI target) {
            this.target = target;
        }

        private void Update() {
            if (target)
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed);
        }

        private void OnTriggerEnter(Collider other) {
            if (other == target.GetComponent<Collider>()) {
                target = null;
                OnReturnPoolEvent.Invoke(this);
            }
        }
    }
}