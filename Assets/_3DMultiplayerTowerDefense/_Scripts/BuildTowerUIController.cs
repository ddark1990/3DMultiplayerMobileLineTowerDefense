using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using GoomerScripts;

public class BuildTowerUIController : MonoBehaviourPunCallbacks
{
    public static BuildTowerUIController Instance;

    public GameObject BuildTowerButton;
    public Transform ButtonHolder;
    public GameObject Canvas;
    public GameObject BackgroundPanel;

    public List<BuildTowerButton> _buildTowerButtons;
    private bool _isSelected;

    private PhotonPlayer _player;
    private TowerPlacer _placer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(Instance);
        }
    }

    private void Start()
    {
        Canvas.SetActive(false);
        StartCoroutine(PopulateBuildTowerButtons());
    }

    private void SetOwnership()
    {
        foreach (var player in GameManager.instance.PlayersInGame)
        {
            if (PhotonNetwork.LocalPlayer.Equals(player.photonView.Owner))
            {
                _player = player;
            }
        }

        foreach (var placer in ConstructionManager.instance.towerPlacers)
        {
            if (PhotonNetwork.LocalPlayer.Equals(placer.photonView.Owner))
            {
                _placer = placer;
            }
        }

        foreach (var button in _buildTowerButtons)
        {
            button.GetComponent<BuildTowerButton>().Owner = _player;
            button.GetComponent<BuildTowerButton>().TowerPlacer = _placer;
        }
    }

    public void TowerBuyOpenMenu()
    {
        Canvas.SetActive(true);
        iTween.ScaleTo(BackgroundPanel, new Vector3(1f, 1f, 1f), .5f);
    }
    public void TowerBuyCloseMenu()
    {
        iTween.ScaleTo(BackgroundPanel, new Vector3(0f, 0f, 0f), .5f);
        if (BackgroundPanel.transform.localScale == Vector3.zero)
        {
            Canvas.SetActive(false);
        }
    }

    private IEnumerator PopulateBuildTowerButtons()
    {
        yield return new WaitUntil(() => GameManager.instance.AllPlayersReady);

        for (int i = 0; i < ListManager.instance.towers.Count; i++)
        {
            var tower = ListManager.instance.towers[i];

            var buttonGameObject = Instantiate(BuildTowerButton, ButtonHolder);
            var button = buttonGameObject.GetComponent<Button>();
            var buildTowerButton = buttonGameObject.GetComponent<BuildTowerButton>();

            _buildTowerButtons.Add(buildTowerButton);

            buildTowerButton.NameText.text = tower.name;
            buildTowerButton.TowerImage.sprite = tower.sprite;
            buildTowerButton.CostText.text = tower.cost.ToString();
            buildTowerButton.TowerPrefab = tower.prefab;

            var selMan = SelectionManager.Instance;

            button.onClick.AddListener(() => _placer.PlaceTower(buildTowerButton.TowerPrefab.name,
                selMan.currentlySelectedObject.transform.position + new Vector3(0, .6f, 0), selMan.currentlySelectedObject.transform.rotation));

            Debug.Log("PopulatedListOfBuildTowerButtons");
        }

        yield return new WaitForSeconds(1);

        SetOwnership();
    }
}
