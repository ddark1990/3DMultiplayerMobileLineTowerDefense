using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public Vector2 GridWorldSize;
    public GameObject Node;
    public float BuildTime;

    int gridSizeX, gridSizeY;

    public void StartGeneration()
    {
        StartCoroutine(CreateGrid());
    }

    public IEnumerator CreateGrid()
    {
        gridSizeX = Mathf.RoundToInt(GridWorldSize.x / 1);
        gridSizeY = Mathf.RoundToInt(GridWorldSize.y / 1);

        Vector3 worldBottomLeft = transform.position - Vector3.right * GridWorldSize.x / 2 - Vector3.forward * GridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                yield return new WaitForSeconds(BuildTime);

                var node = Instantiate(Node, worldBottomLeft + new Vector3(x + 0.5f, 0, y + 0.5f), Quaternion.identity);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(GridWorldSize.x, 0, GridWorldSize.y));
    }
}
