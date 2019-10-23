using System;
using UnityEngine;
using Pathfinding;
namespace JoelQ.GameSystem.Tower {

    public class AI : MonoBehaviour, IPoolable<AI> {
        
        public event Action<AI> OnReturnPoolEvent;

        public void TakeDamage(float damage) {
            Debug.Log($"{name} took {damage} damage!");
        }
    }
}