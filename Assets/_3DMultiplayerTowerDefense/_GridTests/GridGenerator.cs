using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public Vector2 GridWorldSize;
    public GameObject Node;
    public float BuildWaitTime;

    int gridSizeX, gridSizeY;
    Vector3 worldBottomLeft;

    private void Start()
    {
        worldBottomLeft = transform.position - Vector3.right * GridWorldSize.x / 2 - Vector3.forward * GridWorldSize.y / 2;
    }

    public void StartGeneration() //for button
    {
        StartCoroutine(CreateGrid());
    }
    public IEnumerator CreateGrid()
    {
        worldBottomLeft = transform.position - Vector3.right * GridWorldSize.x / 2 - Vector3.forward * GridWorldSize.y / 2;

        gridSizeX = Mathf.RoundToInt(GridWorldSize.x / 1);
        gridSizeY = Mathf.RoundToInt(GridWorldSize.y / 1);

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                yield return new WaitForSeconds(BuildWaitTime);

                var node = Instantiate(Node, worldBottomLeft + new Vector3(x + 0.5f, 0, y + 0.5f), Quaternion.identity);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(GridWorldSize.x, 0, GridWorldSize.y));
    }
}
