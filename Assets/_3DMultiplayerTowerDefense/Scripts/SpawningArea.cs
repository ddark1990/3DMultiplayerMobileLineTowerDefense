using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningArea : MonoBehaviour
{
    public static SpawningArea instance;

    public BoxCollider[] spawners;

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
    }

    public void ApplyOwnershipToSpawners()
    {
        for (int i = 0; i < spawners.Length; i++)
        {
            Spawner spawner = spawners[i].GetComponent<Spawner>();

            switch (spawner.spawnerOwner)
            {
                case Spawner.SpawnerOwner.Player1:
                    spawner.owner = GameManager.instance.playersInGame[0];
                    GameManager.instance.playersInGame[0].SpawnerOwnership = true;
                    break;
                case Spawner.SpawnerOwner.Player2:
                    spawner.owner = GameManager.instance.playersInGame[1];
                    GameManager.instance.playersInGame[1].SpawnerOwnership = true;
                    break;
                case Spawner.SpawnerOwner.Player3:
                    spawner.owner = GameManager.instance.playersInGame[2];
                    break;
                case Spawner.SpawnerOwner.Player4:
                    spawner.owner = GameManager.instance.playersInGame[3];
                    break;
                case Spawner.SpawnerOwner.Player5:
                    spawner.owner = GameManager.instance.playersInGame[4];
                    break;
            }

            Debug.Log("AppliedSpawnerOwnership");
        }
    }
}
