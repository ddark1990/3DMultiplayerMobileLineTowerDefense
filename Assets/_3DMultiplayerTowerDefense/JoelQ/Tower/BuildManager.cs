#pragma warning disable CS0649
using UnityEngine;
namespace JoelQ.GameSystem.Tower {

    public class BuildManager : MonoBehaviour {

        [SerializeField] private BuildGrid grid;
        [SerializeField] private GameObject buttonContainer;
        [SerializeField] private BuildButton buttonPrefab;
        [SerializeField] private GameObject towerPanel;
        [SerializeField] private UpgradeButton upgradeButton;
        [SerializeField] private SellButton sellButton;
        [SerializeField] private TowerManager towerPool;
        private BuildButton[] buildButtons;
        private Node currentNode;
        private Tower currentTower;

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
            buildButtons = new BuildButton[towerPool.Towers.Length];
            for (int i = 0; i < towerPool.Towers.Length; i++) {
                BuildButton button = Instantiate(buttonPrefab, buttonContainer.transform);
                button.Setup(towerPool.Towers[i].Data, i);
                //Subscribe build event
                button.OnClick += Build;
                buildButtons[i] = button;
            }
            //Subscribe tower ui event
            TowerUI.OnClick = OpenTowerUI;
            upgradeButton.OnClick += Sell;
            sellButton.OnClick += Sell;
        }

        public void ToggleUI(Vector3 worldPosition) {
            Node node = grid.GetNodeFromWorldPoint(worldPosition);
            if (node.buildable && !buttonContainer.activeSelf)
                buttonContainer.SetActive(true);
            else if (currentNode == node)
                buttonContainer.SetActive(false);

            currentNode = node;
        }

        public void Build(int towerID) {
            //Tower
            Tower tower = towerPool.Towers[towerID].Get();
            tower.data = towerPool.Towers[towerID].Data;
            tower.OnSpawnProjectile += towerPool.Towers[towerID].projectilePool.Get;
            tower.transform.position = currentNode.worldPosition;
            tower.RefreshData();
            //Node
            currentNode.buildable = false;
            currentNode.walkable = false;
            currentNode = null;
            buttonContainer.SetActive(false);
        }

        #region Tower Panel
        public void OpenTowerUI(Tower tower) {
            currentNode = grid.GetNodeFromWorldPoint(tower.transform.position);
            currentTower = tower;
            towerPanel.SetActive(true);
            upgradeButton.Setup(tower.data);
            sellButton.Setup(tower.data);
        }

        public void Sell(TowerData data) {
            currentNode.buildable = true;
            currentNode.walkable = true;
            currentNode = null;
            towerPanel.SetActive(false);
            currentTower.ReturnToPool();
        }
        #endregion

        private void OnDestroy() {
            //Unsubscribe tower ui event
            TowerUI.OnClick = null;
            upgradeButton.OnClick -= Sell;
            sellButton.OnClick -= Sell; 
            foreach (BuildButton button in buildButtons) {
                //Unsubscribe build event
                button.OnClick -= Build;
            }
        }
    }
}