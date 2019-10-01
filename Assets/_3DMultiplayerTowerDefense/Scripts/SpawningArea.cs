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
        //for (int i = 0; i < spawners.Length; i++)
        //{
        //    Spawner spawner = spawners[i].GetComponent<Spawner>();

        //    switch (spawner.spawnerOwner)
        //    {
        //        case Spawner.SpawnerOwner.Player1:
        //            spawner.owner = GameManager.instance.playersInGame[0];
        //            break;
        //        case Spawner.SpawnerOwner.Player2:
        //            spawner.owner = GameManager.instance.playersInGame[1];
        //            break;
        //        case Spawner.SpawnerOwner.Player3:
        //            spawner.owner = GameManager.instance.playersInGame[2];
        //            break;
        //        case Spawner.SpawnerOwner.Player4:
        //            spawner.owner = GameManager.instance.playersInGame[3];
        //            break;
        //        case Spawner.SpawnerOwner.Player5:
        //            spawner.owner = GameManager.instance.playersInGame[4];
        //            break;
        //    }

        //    Debug.Log("AppliedSpawnerOwnership");
        //}

        for (int i = 0; i < GameManager.instance.playersInGame.Count; i++)
        {
            var player = GameManager.instance.playersInGame[i];

            foreach (var spawner in spawners)
            {
                var _spawner = spawner.GetComponent<Spawner>();

                switch (_spawner.spawnerOwner)
                {
                    case Spawner.SpawnerOwner.Player1:
                        _spawner.owner = GameManager.instance.playersInGame[player.PlayerNumber - 1];
                        break;
                    case Spawner.SpawnerOwner.Player2:
                        _spawner.owner = GameManager.instance.playersInGame[player.PlayerNumber - 1];
                        break;
                    case Spawner.SpawnerOwner.Player3:
                        _spawner.owner = GameManager.instance.playersInGame[player.PlayerNumber - 1];
                        break;
                    case Spawner.SpawnerOwner.Player4:
                        _spawner.owner = GameManager.instance.playersInGame[player.PlayerNumber - 1];
                        break;
                    case Spawner.SpawnerOwner.Player5:
                        _spawner.owner = GameManager.instance.playersInGame[player.PlayerNumber - 1];
                        break;
                }

                Debug.Log("AppliedSpawnerOwnership");
            }
        }
    }
}
