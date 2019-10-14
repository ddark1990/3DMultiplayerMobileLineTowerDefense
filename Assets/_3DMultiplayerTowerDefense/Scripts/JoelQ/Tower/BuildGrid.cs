using UnityEngine;
namespace JoelQ.GameSystem.Tower {

    public class BuildGrid : MonoBehaviour {

        [SerializeField] private BuildPlane buildPlane = default;
        [SerializeField] private LayerMask unwalkableMask = default;
        [SerializeField] private Vector2Int gridWorldSize = default;
        [SerializeField] private float nodeRadius = default;
        private Node[,] grid;
        private float nodeDiameter;
        int gridSizeX, gridSizeY;
        public BuildPlane BuildPlane => buildPlane;

        public void SetupGrid() {

            nodeDiameter = nodeRadius * 2;
            gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
            gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
            buildPlane = Instantiate(buildPlane, transform);
            buildPlane.transform.localScale = new Vector3(gridSizeX, gridSizeY, 1f);
            
            CreateGrid();
        }

        private void CreateGrid() {
            grid = new Node[gridSizeX, gridSizeY];
            Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

            for (int x = 0; x < gridSizeX; x++) {
                for (int y = 0; y < gridSizeY; y++) {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                    bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                    grid[x, y] = new Node(walkable, worldPoint);
                }
            }
        }

        public Node GetNodeFromWorldPoint(Vector3 worldPosition) {
            float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
            float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
            int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

            return grid[x, y];
        }

        private void OnDrawGizmos() {
            //Gizmos
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 0f, gridWorldSize.y));
            if (grid != null) {
                foreach (Node n in grid) {
                    Gizmos.color = n.walkable ? Color.white : Color.red;
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
                }
            }
        }
    }
}

