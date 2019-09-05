using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GoalPoint : MonoBehaviourPunCallbacks
{
    public enum GoalOwner { Player1, Player2, Player3, Player4, Player5, Player6, Player7, Player8 };
    public GoalOwner goalOwner;

    public PhotonPlayer owner;

    private void Start()
    {
        StartCoroutine(SetOwner());
    }

    IEnumerator SetOwner()
    {
        yield return new WaitUntil(() => GameManager.instance.allPlayersLoaded);
        switch (goalOwner)
        {
            case GoalOwner.Player1:
                owner = GameManager.instance.playersInGame[0];
                break;
            case GoalOwner.Player2:
                owner = GameManager.instance.playersInGame[1];
                break;
            case GoalOwner.Player3:
                owner = GameManager.instance.playersInGame[2];
                break;
            case GoalOwner.Player4:
                owner = GameManager.instance.playersInGame[3];
                break;
            case GoalOwner.Player5:
                owner = GameManager.instance.playersInGame[4];
                break;
        }
        Debug.Log("AppliedOwnerShipToGoalPoint");
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Creep>().Die();
    }
}
