using UnityEngine;
namespace JoelQ.GameSystem.Tower {

    public class BuildManager : MonoBehaviour {

        [SerializeField] private BuildGrid grid = default;
        [SerializeField] private GameObject buttonContainer = default;
        [SerializeField] private BuildButton buttonPrefab = default;
        [SerializeField] private TowerDataList towerList = default;
        [SerializeField] private PhotonTowerPool towerPool = default;
        private Node currentNode;

        private void Awake() {
            if (grid == null) {
                Debug.LogError("Grid is missing!");
                Debug.Break();
            } else {
                grid.SetupGrid();
                grid.BuildPlane.OnClickEvent = ToggleUI;
            }

            SetupButton();
        }

        private void SetupButton() {
            for(int i = 0; i < towerList.Towers.Length; i++) {
                BuildButton button = Instantiate(buttonPrefab, buttonContainer.transform);
                button.SetupButton(towerList.Towers[i], i, Build);
            }
        }

        public void ToggleUI(Vector3 worldPosition) {
            Node node = grid.GetNodeFromWorldPoint(worldPosition);
            if (node.buildable && !buttonContainer.activeSelf)
                buttonContainer.SetActive(true);
            else if(currentNode == node)
                buttonContainer.SetActive(false);

            currentNode = node;
        }

        public void Build(int towerID) {
            PhotonTower tower = towerPool.Get();
            tower.data = towerList.Towers[towerID];
            tower.transform.position = currentNode.worldPosition;
            currentNode.buildable = false;
            currentNode.walkable = false;
            currentNode = null;
            buttonContainer.SetActive(false);
        }
    }
}