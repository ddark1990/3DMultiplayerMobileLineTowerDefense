using System.IO;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Turret : MonoBehaviourPunCallbacks, IPooledObject
{
    public Transform target;

    [HideInInspector] public int damage;
    [HideInInspector] public float fireRate;
    [HideInInspector] public float range;
    [HideInInspector] public float projectileSpeed;

    [Header("Unity Setup")]
    public Transform headPivot;
    public Transform firePoint;
    [HideInInspector] public GameObject projectilePrefab;
    [HideInInspector] public GameObject shootEffect;
    [HideInInspector] public GameObject impactEffect;
    [HideInInspector] public bool isLaser;
    [HideInInspector] public bool isSpash;
    public string towerName;
    public string enemyTag = "Enemy";
    public float headTurnSpeed = 10f;

    [HideInInspector] public string shootSound;
    [HideInInspector] public string impactSound;

    private float fireCountDown = 0f;
    private LineRenderer lineRend;

    private void Start()
    {
        lineRend = GetComponent<LineRenderer>();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    private void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if(distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if(nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    private void Update()
    {
        if (target == null)
        {
            if(isLaser)
            {
                if(lineRend.enabled)
                {
                    lineRend.enabled = false;
                }
            }
            return;
        }

        LockOnTarget();

        if (isLaser)
        {
            ShootLaser();
        }
        else
        {
            if (fireCountDown <= 0f)
            {
                Shoot();
                fireCountDown = 1f / fireRate;
            }
        }

        fireCountDown -= Time.deltaTime;
    }

    private void LockOnTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(headPivot.rotation, lookRotation, Time.deltaTime * headTurnSpeed).eulerAngles;
        headPivot.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    private void Shoot()
    {
        AudioManager.AM.Play(shootSound);

        var bulletGo = PoolManager.Instance.SpawnFromPool(projectilePrefab.name, firePoint.position, firePoint.rotation);
        var proj = bulletGo.GetComponent<Projectile>();

        if (proj != null)
        {
            proj.Initialize(target, projectileSpeed, impactEffect, damage, impactSound);
        }

        object[] shootData = new object[]
        {
            projectilePrefab.name,
            firePoint.position,
            firePoint.rotation,
        };

        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.Others
        };

        PhotonNetwork.RaiseEvent((byte)EventIdHandler.EVENT_IDs.TOWER_SHOOT_EVENT, shootData, options, SendOptions.SendReliable);
    }
    private void Shoot_EventReceived(EventData obj)
    {
        if (obj.Code == (byte)EventIdHandler.EVENT_IDs.TOWER_SHOOT_EVENT)
        {
            object[] data = (object[])obj.CustomData;

            if ((int)data[0] == photonView.ViewID)
            {
                var tag = (string) data[1];
                var pos = (Vector3) data[2];
                var rot = (Quaternion) data[3];

                PoolManager.Instance.SpawnFromPool(tag, pos, rot);
            }
        }
    }

    private void ShootLaser()
    {
        if (!lineRend.enabled)
        {
            lineRend.enabled = true;
        }

        lineRend.SetPosition(0, firePoint.position);
        lineRend.SetPosition(1, target.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public void OnObjectSpawn(GameObject obj)
    {
        if (!photonView.IsMine) return;

        SpawnEffect(obj);
    }

    public void OnObjectDespawn(GameObject obj)
    {
        
    }

    private void SpawnEffect(GameObject obj)
    {
        PhotonNetwork.Instantiate(Path.Combine("VFXPrefabs", "BuildEffectMain"), obj.transform.position + new Vector3(0,- .4f,0), obj.transform.rotation);
    }
}
