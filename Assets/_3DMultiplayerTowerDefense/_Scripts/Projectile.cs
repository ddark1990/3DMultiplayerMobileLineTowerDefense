using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour, IPooledObject
{
    private Transform _target;
    private float _projectileSpeed;
    private GameObject _impactEffect;
    private int _damage;
    private string _impactSound;

    public string ProjectileName;

    public void Initialize(Transform target, float projectileSpeed, GameObject impactEffect, int damage, string impactSound)
    {
        _target = target;
        _projectileSpeed = projectileSpeed;
        _impactEffect = impactEffect;
        _damage = damage;
        _impactSound = impactSound;
    }

    private void Update()
    {
        if (_target == null)
        {
            gameObject.SetActive(false);
            return;
        }

        var dir = _target.position - transform.position;
        var distanceThisFrame = _projectileSpeed * Time.deltaTime;

        if(dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.LookAt(_target);
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    private void HitTarget()
    {
        PoolManager.Instance.ReturnToPool(gameObject); //cleans up projectile when reached _target

        DealDamage();
        AudioManager.AM.Play(_impactSound);
        //Destroy(_target.gameObject);
        //GetComponentInChildren<TrailRenderer>().enabled = false;
        //transform.SetParent(_target);
    }

    private void DealDamage()
    {
        _target.GetComponent<MatchSystem.Creep>().TakeDamage(_damage);
    }

    public void OnObjectSpawn(GameObject obj)
    {

    }

    public void OnObjectDespawn(GameObject obj)
    {
        if (_target == null) return;
        else if (_impactEffect != null)
        {
            GameObject impactEffect = Instantiate(this._impactEffect, _target.transform.position, Quaternion.identity); //particle effect
        }
    }
}
