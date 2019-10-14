using System;
using UnityEngine;
namespace JoelQ.GameSystem.Tower {


    public class PhotonTower : MonoBehaviour, IPoolable<PhotonTower> {

        public event Action<PhotonTower> OnReturnPoolEvent;
        [HideInInspector] public TowerData data;

    }
}
