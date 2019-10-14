using System.Collections.Generic;
using UnityEngine;

namespace JoelQ.GameSystem {
    
    public abstract class GenericPool<T> : MonoBehaviour where T : Component {

        [SerializeField] private T prefab = default;
        [SerializeField] private int size = default;
        [SerializeField] private int expandSize = default;
        private Queue<T> pool;
        
        private void Awake() {

            if (pool != null)
                return;
            pool = new Queue<T>(size);
            Expand(size);
        }

        private void Expand(int amount) {
           
            for (int i = 0; i < amount; i++) {
                T poolObject = Instantiate(prefab);
                poolObject.gameObject.SetActive(false);
                if (poolObject is IPoolable<T> poolable)
                    poolable.OnReturnPoolEvent += Return;
                pool.Enqueue(poolObject);
            }
            Debug.Log("Pool has increased by " + amount);
        }

        public T Get() {
            if (pool.Count == 0)
                Expand(expandSize);

            T poolObject = pool.Dequeue();
            poolObject.gameObject.SetActive(true);
            Debug.Log("Retrieved " + poolObject.name + " from pool.");
            return poolObject;
        }

        public void Return(T poolObject) {
            poolObject.gameObject.SetActive(false);
            pool.Enqueue(poolObject);
            Debug.Log(poolObject.name + " has been returned to pool.");
        }
    }
}