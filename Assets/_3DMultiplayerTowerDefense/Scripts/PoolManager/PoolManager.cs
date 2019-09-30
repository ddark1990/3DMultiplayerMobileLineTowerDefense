using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using System.Threading.Tasks;

public class PoolManager : MonoBehaviourPunCallbacks
{
    public static PoolManager Instance;

    public List<GameObject> ActiveObjects;
    public List<PhotonPool> PhotonPools;
    public Dictionary<string, Queue<GameObject>> PoolDictionary;

    private bool _expandIfEmpty = true;

    private GameObject _poolParent;
    private GameObject _categoryPoolParent;
    public GameObject _objToSpawn;
    private GameObject _parent;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            //Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    private new void OnEnable()
    {
        SpawnPools();
    }

    private void SpawnPools()
    {
        if (!PhotonNetwork.IsConnected) return;

        var watch = System.Diagnostics.Stopwatch.StartNew();

        PoolDictionary = new Dictionary<string, Queue<GameObject>>();
        _parent = new GameObject("Extra");
        _parent.transform.SetParent(transform);

        foreach (var item in Enum.GetValues(typeof(PhotonPool.PoolType))) 
        {
            int num = Convert.ToInt32(item);

            _categoryPoolParent = new GameObject(item + "Pools");
            _categoryPoolParent.transform.SetParent(this.transform);

            foreach (var pool in PhotonPools)
            {
                if (pool.size == 0)
                {
                    Debug.LogWarning("You have");
                    return;
                }

                Queue<GameObject> objectPool = new Queue<GameObject>();

                if (num != (int) pool.poolType) continue;

                _poolParent = new GameObject();
                _poolParent.transform.SetParent(_categoryPoolParent.transform);

                for (int i = 0; i < pool.size; i++)
                {
                    if (i >= 0)
                    {
                        switch (pool.poolType)
                        {
                            case PhotonPool.PoolType.Creep:

                                var creep = Instantiate(ObjectToPool(pool), new Vector3(0, 0, 0), Quaternion.identity);
                                PoolData.SetCreepData(creep, pool, _poolParent, objectPool);
                                SetSecondaryData(creep, pool);

                                break;
                            case PhotonPool.PoolType.Tower:
                                var tower = Instantiate(ObjectToPool(pool), new Vector3(0, 0, 0), Quaternion.identity);
                                PoolData.SetTowerData(tower, pool, _poolParent, objectPool);

                                break;
                            case PhotonPool.PoolType.Object:
                                var obj = Instantiate(ObjectToPool(pool), new Vector3(0, 0, 0), Quaternion.identity);
                                PoolData.SetObjectData(obj, pool, _poolParent, objectPool);

                                break;
                            default:
                                Debug.Log("Empty");
                                break;
                        }
                    }
                    else if (pool.size == 0)
                    {
                        Debug.LogWarning("Some pool sizes are set to 0!");
                    }
                }

                PoolDictionary.Add(pool.Name, objectPool);
                Debug.Log("SpawningPools");
            }
        }

        watch.Stop();
        var elapsedTime = watch.ElapsedMilliseconds;

        Debug.Log("Total Time To Spawn Pools = " + elapsedTime + " milliseconds");
    }

    public GameObject SpawnFromPool(string poolName, Vector3 pos, Quaternion rot)
    {
        if (PoolDictionary[poolName].Count.Equals(0) && !_expandIfEmpty)
        {
            Debug.Log("Queue is empty, check the ExpandIfEmpty box to allow the pool to expand if more object are needed");
            return null;
        }

        if (!PoolDictionary.ContainsKey(poolName))
        {
            Debug.LogWarning("Pool does not exist.");
            return null;
        }

        if (PoolDictionary[poolName].Count.Equals(0) && _expandIfEmpty) //allows pool expansion if bool is true
        {
            foreach (var pool in PhotonPools)
            {
                var objectToPool = ObjectToPool(pool);

                if (objectToPool.name == poolName)
                {
                    _objToSpawn = objectToPool;

                    var objToExpand = Instantiate(_objToSpawn, pos, rot);

                    PoolDictionary[poolName].Enqueue(objToExpand);

                    objToExpand.transform.SetParent(_parent.transform);
                    objToExpand.name = objToExpand.name.Replace("(Clone)", ""); //gets rid of (Clone) on a newly instantiated object so it can pool correctly

                    SetSecondaryData(objToExpand, pool);
                }
            }
        }

        _objToSpawn = PoolDictionary[poolName].Dequeue();

        //Debug.Log("SpawningFromPool: " + _objToSpawn + " | " + poolName);

        _objToSpawn.SetActive(true);

        _objToSpawn.transform.position = pos;
        _objToSpawn.transform.rotation = rot;

        var pooledObject = GetPooledObject(_objToSpawn);

        pooledObject?.OnObjectSpawn(_objToSpawn);

        ActiveObjects.Add(_objToSpawn);

        //ApplyObjectOwnership(_objToSpawn, player); //photon specific, applies network ID and ownership to the object based on who spawned it

        return _objToSpawn;
    }

    private static void SetSecondaryData(GameObject obj, PhotonPool pool)
    {
        if (obj.Equals(null))
        {
            Debug.LogWarning(obj + " does not have secondary data set! Check the PoolManager.");
            return;
        }

        if (obj.GetComponent<Creep>())
        {
            var creep = obj.GetComponent<Creep>();

            creep.CreepCost = pool.creep.Cost;
            creep.RefreshSendRate = pool.creep.RefreshSendRate;
            creep.SendLimit = pool.creep.SendLimit;
            return;
        }
    }

    public void ReturnToPool(GameObject obj)
    {
        var pooledObject = GetPooledObject(obj);
        pooledObject?.OnObjectDespawn(obj);

        PoolDictionary[obj.name].Enqueue(obj); //queue's the object back into the pool

        ActiveObjects.Remove(obj); //removes object from active pool list

        obj.SetActive(false);
    }

    IPooledObject GetPooledObject(GameObject obj)
    {
        return obj.GetComponent<IPooledObject>();
    }

    private void ApplyObjectOwnership(GameObject obj, Player player)
    {
        if (obj.GetComponent<Projectile>()) return;

        obj.GetPhotonView().TransferOwnership(player); //transfer correct ownership

        var num = PhotonNetwork.AllocateViewID(player.ActorNumber); //allocate ID based on player in room

        obj.GetPhotonView().ViewID = num; //set a new viewID
    }

    public GameObject ObjectToPool(PhotonPool pool)
    {
        if (pool.creep)
        {
            return pool.creep.Prefab;
        }

        if (pool.tower)
        {
            return pool.tower.prefab;
        }

        return pool.gameObj ? pool.gameObj : null;
    }


    [Serializable]
    public class PhotonPool
    {
        public enum PoolType { Creep, Tower, Object};
        public PoolType poolType;

        public string Name;
        public SpawnableNPC creep;
        public PlaceableTower tower;
        public GameObject gameObj;
        public int size;
    }
}
