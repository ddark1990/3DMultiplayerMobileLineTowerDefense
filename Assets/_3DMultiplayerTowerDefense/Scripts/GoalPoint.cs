using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GoalPoint : MonoBehaviourPunCallbacks
{
    public enum GoalOwner { Player1, Player2, Player3, Player4, Player5, Player6, Player7, Player8 };
    public GoalOwner goalOwner;

    public PhotonPlayer Owner;

    private void Start()
    {
        StartCoroutine(SetOwner());
    }

    IEnumerator SetOwner()
    {
        yield return new WaitUntil(() => GameManager.instance.AllPlayersReady);
        switch (goalOwner)
        {
            case GoalOwner.Player1:
                Owner = GameManager.instance.playersInGame[0];
                break;
            case GoalOwner.Player2:
                Owner = GameManager.instance.playersInGame[1];
                break;
            case GoalOwner.Player3:
                Owner = GameManager.instance.playersInGame[2];
                break;
            case GoalOwner.Player4:
                Owner = GameManager.instance.playersInGame[3];
                break;
            case GoalOwner.Player5:
                Owner = GameManager.instance.playersInGame[4];
                break;
        }
        Debug.Log("AppliedOwnerhipToGoalPoint");
    }

    private void OnTriggerEnter(Collider other)
    {
        GoalActivated(other);
    }

    private void GoalActivated (Component other) //clears creep by calling die on him and decrements player life 
    {
        var creep = other.GetComponent<Creep>();
        creep.Die();

        if (Owner.photonView.IsMine) return;

        Owner.photonView.RPC("RPC_PlayerLoseLife", RpcTarget.AllViaServer);
    }
}
