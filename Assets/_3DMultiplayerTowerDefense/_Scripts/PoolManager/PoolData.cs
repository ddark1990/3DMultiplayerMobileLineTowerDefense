using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PoolData
{
    public static GameObject SetCreepData(GameObject obj, PoolManager.PhotonPool pool, GameObject poolParent, Queue<GameObject> objectPool)
    {
        var creep = obj.GetComponent<MatchSystem.Creep>();

        if (creep == null) return null;

        pool.Name = pool.creep.Prefab.name;
        poolParent.name = pool.creep.Prefab.name + "Pool";

        creep.Health = pool.creep.Health;
        creep.Attack = pool.creep.Attack;
        creep.Defense = pool.creep.Defense;
        creep.CreepName = pool.creep.Prefab.name;
        creep.Income = pool.creep.Income;
        creep.CreepCost = pool.creep.Cost;

        Debug.Log("Creating " + creep + " " + PhotonNetwork.LocalPlayer.NickName);
        creep.gameObject.SetActive(false);
        obj.transform.SetParent(poolParent.transform);
        obj.name = obj.name.Replace("(Clone)", ""); //gets rid of (Clone) on a newly instantiated object so it can pool correctly
        objectPool.Enqueue(obj);

        return obj;
    }

    public static GameObject SetTowerData(GameObject obj, PoolManager.PhotonPool pool, GameObject poolParent, Queue<GameObject> objectPool)
    {
        var turret = obj.GetComponent<Turret>();
        if (turret == null) return null;

        pool.Name = pool.tower.prefab.name;
        poolParent.name = pool.tower.prefab.name + "Pool";

        turret.damage = pool.tower.damage;
        turret.fireRate = pool.tower.fireRate;
        turret.range = pool.tower.range;
        turret.projectilePrefab = pool.tower.projectilePrefab;
        turret.projectileSpeed = pool.tower.projectileSpeed;
        turret.shootEffect = pool.tower.shootEffect;
        turret.impactEffect = pool.tower.impactEffect;
        turret.towerName = pool.tower.prefab.name;
        turret.TowerCost = pool.tower.cost;

        Debug.Log("Creating " + obj + " " + PhotonNetwork.LocalPlayer.NickName);
        obj.transform.SetParent(poolParent.transform);
        obj.name = obj.name.Replace("(Clone)", ""); //gets rid of (Clone) on a newly instantiated object so it can pool correctly

        obj.SetActive(false);
        objectPool.Enqueue(obj);

        return obj;
    }

    public static GameObject SetObjectData(GameObject obj, PoolManager.PhotonPool pool, GameObject poolParent, Queue<GameObject> objectPool)
    {
        var proj = obj.GetComponent<Projectile>();

        pool.Name = pool.gameObj.name;
        poolParent.name = pool.gameObj.name + "Pool";

        if (proj)
        {
            proj.ProjectileName = pool.gameObj.name;
        }

        Debug.Log("Creating " + obj + " " + PhotonNetwork.LocalPlayer.NickName);
        obj.transform.SetParent(poolParent.transform);
        obj.name = obj.name.Replace("(Clone)", ""); //gets rid of (Clone) on a newly instantiated object so it can pool correctly
        obj.SetActive(false);
        objectPool.Enqueue(obj);

        return obj;
    }
}
