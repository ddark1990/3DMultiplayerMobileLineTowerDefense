﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeOwnership : MonoBehaviour
{
    public static NodeOwnership instance;

    public List<Node> nodes;

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

    public void ApplyOwnershipToNodes()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            Node node = nodes[i].GetComponent<Node>();

            switch (node.nodeOwner)
            {
                case Node.NodeOwner.Player1:
                    node.owner = GameManager.instance.PlayersInGame[0];
                    break;
                case Node.NodeOwner.Player2:
                    node.owner = GameManager.instance.PlayersInGame[1];
                    break;
                case Node.NodeOwner.Player3:
                    node.owner = GameManager.instance.PlayersInGame[2];
                    break;
                case Node.NodeOwner.Player4:
                    node.owner = GameManager.instance.PlayersInGame[3];
                    break;
                case Node.NodeOwner.Player5:
                    node.owner = GameManager.instance.PlayersInGame[4];
                    break;
            }

            Debug.Log("AppliedNodeOwnership");
        }

        //for (int i = 0; i < GameManager.instance.playersInGame.Count; i++)
        //{
        //    var player = GameManager.instance.playersInGame[i];

        //    foreach (var node in nodes)
        //    {
        //        switch (node.nodeOwner)
        //        {
        //            case Node.NodeOwner.Player1:
        //                node.owner = GameManager.instance.playersInGame[player.PlayerNumber - 1];
        //                break;
        //            case Node.NodeOwner.Player2:
        //                node.owner = GameManager.instance.playersInGame[player.PlayerNumber - 1];
        //                break;
        //            case Node.NodeOwner.Player3:
        //                node.owner = GameManager.instance.playersInGame[player.PlayerNumber - 1];
        //                break;
        //            case Node.NodeOwner.Player4:
        //                node.owner = GameManager.instance.playersInGame[player.PlayerNumber - 1];
        //                break;
        //            case Node.NodeOwner.Player5:
        //                node.owner = GameManager.instance.playersInGame[player.PlayerNumber - 1];
        //                break;
        //        }
        //    }

        //    Debug.Log("AppliedNodeOwnership");
        //}
    }
}
