using Pathfinding;
using UnityEngine;
using Photon.Pun;

public class Creep : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback, IPooledObject, ICreepSender
{
    [Header("Primary Creep Data")]
    public string CreepName; //primary data
    public float Health;
    public int Attack;
    public int Defense;
    public int SenderViewId;
    public PhotonPlayer Owner;

    [Header("Secondary Creep Data")]
    public int SendLimit; //secondary data
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

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {

    }

    public void OnObjectSpawn(GameObject obj)
    {
        if (Health < _startHealth)
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
}
