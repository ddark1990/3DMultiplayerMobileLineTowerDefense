using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListManager : MonoBehaviour
{
    public static ListManager instance;

    public List<SpawnableNPC> creeps;
    public List<PlaceableTower> towers;
    public GoalPoint[] goals;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            //DontDestroyOnLoad(Instance);
        }
        creeps.Sort();
        towers.Sort();
    }
}
