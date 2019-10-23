#pragma warning disable CS0649
using System;
using System.Collections.Generic;
using UnityEngine;
namespace JoelQ.GameSystem.Tower {

    public class Tower : MonoBehaviour, IPoolable<Tower> {

        public event Action<Tower> OnReturnPoolEvent;
        public event Func<TowerProjectile> OnSpawnProjectile;
        [HideInInspector] public TowerData data;
        protected TowerState state;
        protected Collider[] targets;
        private float currentRate;
        private Creep[] selectedTargets;
        private int currentTargetCount;
        private SortTargetByDistance targetSorter = new SortTargetByDistance();

        protected virtual void Update() {

            if (state == TowerState.Idle) {
                Idle();
            }
        }

        protected virtual void FixedUpdate() {

            if (state == TowerState.Search) {
                Search();
            } else if (state == TowerState.Attack) {
                Attack();
            }
        }

        public virtual void RefreshData() {
            state = TowerState.Idle;
            targets = new Collider[data.TargetCount];
            selectedTargets = new Creep[data.TargetCount];
        }

        protected virtual void Idle() {
            Debug.Log("Idle");
            state = TowerState.Search;
        }

        protected virtual void Search() {

            currentTargetCount = Physics.OverlapSphereNonAlloc(transform.position, data.Range, targets, data.TargetMask);
            if (currentTargetCount == 1) {
                selectedTargets[0] = targets[0].GetComponent<Creep>();
                state = TowerState.Attack;
            } else if (currentTargetCount > 1) {
                targetSorter.GetTransform(transform.position);
                Array.Sort(targets, 0, currentTargetCount, targetSorter);
                for (int i = 0; i < currentTargetCount; i++) {
                    selectedTargets[i] = targets[i].GetComponent<Creep>();
                }
                state = TowerState.Attack;
            }
        }

        protected virtual void Attack() {

            //Validate whether targets are still in range.
            int inRangeCount = 0;
            float distance;
            for (int i = 0; i < currentTargetCount; i++) {
                distance = (selectedTargets[i].transform.position - transform.position).sqrMagnitude;
                if (distance <= data.Range * data.Range) {
                    inRangeCount++;
                    currentRate -= Time.deltaTime;
                }
            }

            if (inRangeCount < data.TargetCount) {
                state = TowerState.Search;
            }

            if (currentRate < 0) {

                //Reset fire rate
                currentRate = data.FireRate;

                for (int i = 0; i < currentTargetCount; i++) {
                    selectedTargets[i].TakeDamage(data.Damage);
                }

                //Calculate Projectile Arc

                OnSpawnProjectile.Invoke().Setup(transform.position, selectedTargets[0]);
            }

#if UNITY_EDITOR
            for (int i = 0; i < currentTargetCount; i++) {
                Debug.DrawLine(transform.position, selectedTargets[i].transform.position, Color.red);
            }
#endif
        }

        public void ReturnToPool() {
            OnReturnPoolEvent.Invoke(this);
        }

        protected enum TowerState {
            Idle,
            Search,
            Attack
        }

        private class SortTargetByDistance : IComparer<Collider> {

            private Vector3 towerPos;

            public void GetTransform(Vector3 towerPos) {
                this.towerPos = towerPos;
            }

            public int Compare(Collider x, Collider y) {
                if ((towerPos - x.transform.position).sqrMagnitude > (towerPos - y.transform.position).sqrMagnitude) {
                    return 1;
                } else if ((towerPos - x.transform.position).sqrMagnitude < (towerPos - y.transform.position).sqrMagnitude) {
                    return -1;
                } else {
                    return 0;
                }
            }
        }
    }

}
