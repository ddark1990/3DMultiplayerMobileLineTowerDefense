using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New PlaceableTower", menuName = "Create New Objects/Create New PlaceableTower")]
public class PlaceableTower : ScriptableObject, IComparable<PlaceableTower> 
{
    [Header("Tower Settings")]
    public int id; //has to coordinate with UI button numbers
    public new string name;
    public GameObject prefab;
    public Sprite sprite;
    [TextArea]public string description;
    public GameObject buildEffect;
    public string buildSound;
    public bool splashDamage;
    public bool laser; //some sort of a laser so a projectile is not required 
    public int cost;
    public int damage;
    public int defense;
    public float fireRate;
    public float range;

    [Header("Projectile Settings")]
    public float projectileSpeed;
    public GameObject projectilePrefab;
    public GameObject shootEffect;
    public GameObject impactEffect;
    public string shootingSound;
    public string impactSound;

    public int CompareTo(PlaceableTower other)
    {
        if (this.cost > other.cost)
            return 1;
        else if (this.cost < other.cost)
            return -1;
        else
            return 0;
    }
}
