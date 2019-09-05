using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public enum SpawnerOwner { Player1, Player2, Player3, Player4, Player5, Player6, Player7, Player8 };
    public SpawnerOwner spawnerOwner;

    public PhotonPlayer owner;
}
