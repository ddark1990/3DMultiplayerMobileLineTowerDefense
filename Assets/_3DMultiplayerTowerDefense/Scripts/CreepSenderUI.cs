using GoomerScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CreepSenderUI : MonoBehaviourPunCallbacks
{
    public GameObject creepButtonPrefab;
    [Header("UI Essentials")]
    public GameObject uICanvas;
    public GameObject backgroundPanel;
    public GameObject buttonPanel;

    [Header("Cache")]
    public CreepSender CreepSender;

    public List<CreepButtonUI> CreepButtonsList;
    public bool IsSelected;

    private void Start()
    {
        uICanvas.SetActive(false);

        if (!CreepSender.IsSelectable) return;

        if (CreepButtonsList.Count == 0)
        {
            StartCoroutine(PopulateCreepButtons());
        }
    }

    private void Update()
    {
        if (!CreepSender.IsSelectable)
            return;

        SelectBuilding();
    }

    private void SelectBuilding()
    {
        if (SelectionManager.Instance.currentlySelectedObject == transform.parent.gameObject)
        {
            if(IsSelected)
            {
                return;
            }
            Debug.Log("OpenMenu");
            CreepBuyOpenMenu();
            IsSelected = true;
        }
        else
        {
            if(!IsSelected)
            {
                return;
            }
            CreepBuyCloseMenu();
            Debug.Log("CloseMenu");
            IsSelected = false;
        }
    }

    void CreepBuyOpenMenu()
    {
        uICanvas.SetActive(true);
        iTween.ScaleTo(backgroundPanel, new Vector3(1f, 1f, 1f), .5f);
    }
    void CreepBuyCloseMenu()
    {
        iTween.ScaleTo(backgroundPanel, new Vector3(0f, 0f, 0f), .5f);
        if(backgroundPanel.transform.localScale == Vector3.zero)
        {
            uICanvas.SetActive(false);
        }
    }

    IEnumerator PopulateCreepButtons()
    {
        yield return new WaitUntil(() => GameManager.instance.allPlayersLoaded);

        for (int i = 0; i < ListManager.instance.creeps.Count; i++)
        {
            var instanceCreep = ListManager.instance.creeps[i];

            var button = Instantiate(creepButtonPrefab, buttonPanel.transform);

            var creepButton = button.GetComponent<CreepButtonUI>();

            creepButton.img.sprite = instanceCreep.Sprite;
            creepButton.costText.text = instanceCreep.Cost.ToString(); //display single creepcost x 2 based on how many sides creeps come from, map based, temp solution
            creepButton.healthText.text = instanceCreep.Health.ToString();
            creepButton.defenseText.text = instanceCreep.Defense.ToString();
            creepButton.nameText.text = instanceCreep.Name;
            creepButton.SendLimitText.text = instanceCreep.SendLimit.ToString();
            creepButton.IncomeText.text = instanceCreep.Income.ToString();
            creepButton.RefreshSendRate = instanceCreep.RefreshSendRate;

            for (int x = 0; x < SpawningArea.instance.spawners.Length; x++)
            {
                var spawner = SpawningArea.instance.spawners[x].GetComponent<Spawner>();

                if (CreepSender.Owner != spawner.owner && CreepSender.photonView.IsMine)
                {
                    Debug.Log("AddingListener");

                    button.GetComponent<Button>().onClick.AddListener(
                        () => CreepSender.SendCreep(instanceCreep.Prefab.name, spawner.transform.position, spawner.transform.rotation));
                }
            }

            Debug.Log("PopulatedListOfCreepButtons");
        }
    }
}
