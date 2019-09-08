using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New SpawnableNPC", menuName = "Create New Objects/Create New SpawnableNPC")]
public class SpawnableNPC : ScriptableObject, IComparable<SpawnableNPC>
{
    [Header("Creep Settings")]
    public int Id;
    public string Name;
    public GameObject Prefab;
    public Sprite Sprite;
    [TextArea]public string Description;
    public int SendLimit;

    [Header("Creep Stats")]
    public int Cost;
    public int Income;
    public float Health;
    public int Attack;
    public int Defense;

    public int CompareTo(SpawnableNPC other)
    {
        if (this.Cost > other.Cost)
            return 1;
        else if (this.Cost < other.Cost)
            return -1;
        else
            return 0;
    }
}
