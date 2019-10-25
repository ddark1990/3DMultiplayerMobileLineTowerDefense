using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnContainer : MonoBehaviour
{
    public static SpawnContainer Instance;

    public Transform[] GridSpawnPoints;
    public Transform[] CreepSpawnAreas;
    public Transform[] CameraSpawnPoints;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }
}
