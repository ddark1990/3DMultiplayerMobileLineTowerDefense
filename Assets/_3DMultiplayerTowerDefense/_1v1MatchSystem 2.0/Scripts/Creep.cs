using Pathfinding;
using UnityEngine;

namespace MatchSystem
{
    public class Creep : MonoBehaviour, IPooledObject, ICreepSender
    {
        public Transform AttackTarget;

        [Header("Primary Creep Data")]
        public string CreepName;
        public float Health;
        public int Attack;
        public int Defense;
        public int Income;
        public int SenderViewId;
        public NetworkPlayer NetworkOwner;

        [Header("Secondary Creep Data")]
        public int SendLimit;
        public int CreepCost;
        public float RefreshSendRate;

        private AIDestinationSetter _destination;
        private float _startHealth;

        private void OnEnable()
        {
            _startHealth = Health;
        }

        private void Update()
        {
            if (Health <= 0)
            {
                Die();
            }
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
        }

        public void Die()
        {
            PoolManager.Instance.ReturnToPool(gameObject);
        }

        #region Interfaces

        public void OnObjectSpawn(GameObject obj)
        {
            if (Health < _startHealth) //health check when the pool manager needs to create extra creeps
            {
                Health = _startHealth;
            }

        }

        public void OnObjectDespawn(GameObject obj)
        {

        }

        public void CreepSent(GameObject obj)
        {

        }

        #endregion


        public void GoalReached(NetworkPlayer player)
        {
            Die();
            player.GetComponent<PlayerMatchData>().DeductPlayerLife_Event();
        }
    }
}