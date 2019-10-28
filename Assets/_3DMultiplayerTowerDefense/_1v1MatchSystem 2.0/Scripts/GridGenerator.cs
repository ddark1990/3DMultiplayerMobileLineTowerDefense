using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatchSystem
{
    public class GridGenerator : MonoBehaviour
    {
        public NetworkPlayer NetworkOwner;
        public List<Node> Nodes;
        [Space]

        public Vector2 GridWorldSize;
        public GameObject Node;
        [Tooltip("Time to build the grid using a coroutine.")] public float BuildWaitTime;
        [Range(-1, 1)] public float GridYOffset = -0.1f;
        public bool GridGenerated;

        int gridSizeX, gridSizeY;
        Vector3 worldBottomLeft;

        private void Start()
        {
            StartGeneration();
        }

        public void StartGeneration() //for button
        {
            StartCoroutine(CreateGrid());
        }
        public IEnumerator CreateGrid()
        {
            yield return new WaitUntil(() => NetworkOwner);
            yield return new WaitUntil(() => NetworkOwner.PlayerReady);

            var increment = 0;

            worldBottomLeft = transform.position - Vector3.right * GridWorldSize.x / 2 - Vector3.forward * GridWorldSize.y / 2;

            gridSizeX = Mathf.RoundToInt(GridWorldSize.x / 1);
            gridSizeY = Mathf.RoundToInt(GridWorldSize.y / 1);

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    yield return new WaitForSeconds(BuildWaitTime);
                    increment++;

                    var node = Instantiate(Node, worldBottomLeft + new Vector3(x + 0.5f, GridYOffset, y + 0.5f), Quaternion.identity, transform);

                    node.GetComponent<Node>().NetworkOwner = NetworkOwner;

                    if(NetworkOwner.photonView.Owner == PhotonNetwork.LocalPlayer)
                    {
                        node.GetComponent<Node>().Interactable = true;
                    }

                    Nodes.Add(node.GetComponent<Node>());

                    node.name = "Node" + increment.ToString();
                }
            }

            GridGenerated = true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(GridWorldSize.x, 0, GridWorldSize.y));
        }
    }

}