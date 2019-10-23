#pragma warning disable CS0649
using System;
using UnityEngine;
namespace JoelQ.GameSystem.Tower {
    public class TowerProjectile : MonoBehaviour, IPoolable<TowerProjectile> {
        [SerializeField] protected float speed;
        public event Action<TowerProjectile> OnReturnPoolEvent;
        private Creep target;

        public void Setup(Vector3 spawnPos, Creep target) {
            transform.position = spawnPos;
            this.target = target;
        }

        private void Update() {
            if (target)
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed);
        }

        private void OnTriggerEnter(Collider other) {
            if (other == target.GetComponent<BoxCollider>()) {
                target = null;
                OnReturnPoolEvent.Invoke(this);
            }
        }
    }
}