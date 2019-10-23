using Pathfinding;
using UnityEngine;

public class Creep : MonoBehaviour, IPooledObject, ICreepSender
{
    [Header("Primary Creep Data")]
    public string CreepName;
    public float Health;
    public int Attack;
    public int Defense;
    public int SenderViewId;
    public PhotonPlayer Owner;

    [Header("Secondary Creep Data")]
    public int SendLimit; 
    public int CreepCost;
    public float RefreshSendRate;

    private AIDestinationSetter _destination;
    private float _startHealth;

    private void Start()
    {
        _startHealth = Health;
    }

    private void Update()
    {
        if(Health <= 0)
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
        if (Health < _startHealth) //health check when creep spawns
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
}
