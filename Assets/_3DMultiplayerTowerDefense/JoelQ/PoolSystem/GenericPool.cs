using System.Collections.Generic;
using UnityEngine;

namespace JoelQ.GameSystem {
    
    public abstract class GenericPool<T> where T : Component {

        [SerializeField] protected T prefab = default;
        [SerializeField] protected int size = default;
        [SerializeField] protected int expandSize = default;
        private Queue<T> pool;
        
        public void InitializePool() {

            if (pool != null)
                return;
            pool = new Queue<T>(size);
            Expand(size);
        }

        private void Expand(int amount) {
           
            for (int i = 0; i < amount; i++) {
                T poolObject = Object.Instantiate(prefab);
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