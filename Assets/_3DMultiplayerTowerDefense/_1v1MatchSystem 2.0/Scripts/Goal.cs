using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatchSystem
{
    public class Goal : MonoBehaviour
    {
        public NetworkPlayer NetworkOwner;

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.GetComponent<Creep>())
            {
                other.gameObject.GetComponent<Creep>().GoalReached(NetworkOwner);
            }
        }
    }
}
