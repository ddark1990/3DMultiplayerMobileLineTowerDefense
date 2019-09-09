﻿using Pathfinding;
using UnityEngine;
using Photon.Pun;

public class Creep : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback, IPooledObject
{
    public string CreepName;
    public float Health;
    public float RefreshSendRate;
    public int Attack;
    public int Defense;
    public int SendLimit;
    public int SenderViewId;
    public PhotonPlayer Owner;

    private AIDestinationSetter _destination;
    private int _startHealth;

    private void Start()
    {
        _startHealth = (int)Health;
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
        Health = _startHealth;
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {

    }

    public void OnObjectSpawn(GameObject obj)
    {
        //Debug.Log("Spawning " + obj.name);
    }

    public void OnObjectDespawn(GameObject obj)
    {
        //photonView.TransferOwnership(0); //resets the ownership back to scene 
        //photonView.ViewID = 0;
    }

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.IsWriting)
    //    {
    //        // We own this player: send the others our data
    //        stream.SendNext(Health);
    //    }
    //    else
    //    {
    //        // Network player, receive data
    //        this.Health = (float)stream.ReceiveNext();
    //    }
    //}

}
