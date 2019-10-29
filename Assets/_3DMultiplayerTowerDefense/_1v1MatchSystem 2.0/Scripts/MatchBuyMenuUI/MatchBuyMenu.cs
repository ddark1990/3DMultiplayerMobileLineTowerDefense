using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatchSystem
{
    public class MatchBuyMenu : MonoBehaviour
    {
        public static MatchBuyMenu Instance;

        public GameObject UICanvas;
        [Space]
        public GameObject ButtonMenu;

        public GameObject TowerButton;
        public GameObject CreepButton;
        public RuntimeAnimatorController TowerAnimCont;
        public RuntimeAnimatorController CreepAnimCont;

        public bool BuyTowerMenuOpen, BuyCreepMenuOpen, MenusLoaded;

        public Button BuyCreepButton;

        private SelectionManager SM;
        private bool check = true;

        private GameObject towerMenu;
        private GameObject creepMenu;

        [HideInInspector] public Animator creepMenuAnim;
        [HideInInspector] public Animator towerMenuAnim;


        private void Awake()
        {
            StartCoroutine(ToggleCreepShopButton());

            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            else
            {
                Instance = this;
            }

            SM = SelectionManager.Instance;

        }

        private GameObject CreateButtonMenu(GameObject buttonMenu, string menuName)
        {
            var menu = Instantiate(buttonMenu, UICanvas.transform);
            menu.name = menuName;
            Debug.Log("Building " + menuName + " is done.");

            return menu;
        }
        //build buy menus for the player
        private void BuildTowerMenu(NetworkPlayer player)
        {
            towerMenu = CreateButtonMenu(ButtonMenu, "TowerMenu");
            towerMenuAnim = towerMenu.GetComponent<Animator>();

            towerMenuAnim.runtimeAnimatorController = TowerAnimCont;

            var buttonHolder = towerMenu.transform.GetChild(0);

            var towerPlacer = player.playerControllers.GetComponent<TowerPlacer>();

            for (int i = 0; i < towerPlacer.Towers.Count; i++)
            {
                var tower = towerPlacer.Towers[i];
                var towerBuildButton = Instantiate(TowerButton, buttonHolder);

                var buttonInterface = towerBuildButton.GetComponent<TowerButton>();

                //ui data
                buttonInterface.TowerImage.sprite = tower.sprite;
                buttonInterface.NameText.text = tower.name;
                buttonInterface.CostText.text = tower.cost.ToString();

                //extra
                buttonInterface.TowerPrefab = tower.prefab;
                buttonInterface.TowerPlacer = towerPlacer;
            }
        }
        private void BuildCreepMenu(NetworkPlayer player)
        {
            creepMenu = CreateButtonMenu(ButtonMenu, "CreepMenu");

            creepMenu.GetComponent<Animator>().runtimeAnimatorController = CreepAnimCont;

            var buttonHolder = creepMenu.transform.GetChild(0);

            var creepSender = player.playerControllers.GetComponent<CreepSender>();

            for (int i = 0; i < creepSender.Creeps.Count; i++)
            {
                var creep = creepSender.Creeps[i];
                var creepSendButton = Instantiate(CreepButton, buttonHolder);

                var buttonInterface = creepSendButton.GetComponent<CreepButton>();

                buttonInterface.Img.sprite = creep.Sprite;
                buttonInterface.NameText.text = creep.Name;
                buttonInterface.HealthText.text = creep.Health.ToString();
                buttonInterface.DefenseText.text = creep.Defense.ToString();
                buttonInterface.CostText.text = creep.Cost.ToString();
                buttonInterface.IncomeText.text = creep.Income.ToString();
                buttonInterface.SendLimitText.text = creep.SendLimit.ToString();
                buttonInterface.RefreshSendRate = creep.RefreshSendRate;
                buttonInterface.creepSender = creepSender;
            }
        }

        public void BuildMenus(NetworkPlayer player)
        {
            BuildCreepMenu(player);
            BuildTowerMenu(player);
            MenusLoaded = true;
        }

        public void OnCreepShopPress() //creep menu
        {
            ResetMenus();

            BuyCreepMenuOpen = !BuyCreepMenuOpen;

            creepMenu.GetComponent<Animator>().SetBool("CreepMenuOpen", BuyCreepMenuOpen);

            if(SelectionManager.Instance.CurrentlySelectedObject)
            {
                NodeController.Instance.DehighlightNode();
                SelectionManager.Instance.CurrentlySelectedObject = null;
            }
        }

        public void ResetMenus()
        {
            creepMenu.GetComponent<Animator>().SetBool("CreepMenuOpen", false);
            towerMenu.GetComponent<Animator>().SetBool("TowerMenuOpen", false);
        }

        private IEnumerator ToggleCreepShopButton()
        {
            BuyCreepButton.interactable = false;
            yield return new WaitUntil(() => MatchManager.Instance.MatchStarted);
            BuyCreepButton.interactable = true;
            yield return new WaitUntil(() => MatchManager.Instance.MatchEnd);
            BuyCreepButton.interactable = false;
            ResetMenus();
        }
    }
}