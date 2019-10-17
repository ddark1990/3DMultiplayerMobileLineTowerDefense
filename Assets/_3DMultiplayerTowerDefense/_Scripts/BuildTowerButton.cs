using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.EventSystems;

public class BuildTowerButton : MonoBehaviourPunCallbacks, IPointerClickHandler
{
    [Header("ManualSet")]
    public Image TowerImage;
    public TextMeshProUGUI CostText;
    public TextMeshProUGUI NameText;
    public Button Button;

    [Header("AutoSet")]
    public GameObject TowerPrefab;
    public PhotonPlayer Owner;
    public TowerPlacer TowerPlacer;

    private int _towerCost;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _towerCost = int.Parse(CostText.text);

        StartCoroutine(ToggleCheck(.1f));
    }

    private IEnumerator ToggleCheck(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            ToggleInteractable();
        }
    }

    private void ToggleInteractable()
    {
        Button.interactable = CanAffordItem(Owner.GetComponent<PlayerMatchData>().PlayerGold, _towerCost);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!Button.IsInteractable()) return;

        Debug.Log(eventData);

        if (!CanAffordItem(Owner.GetComponent<PlayerMatchData>().PlayerGold, _towerCost))
        {
            Debug.Log(Owner + " ran out of gold.");
            return;
        }

        TowerPlacer.photonView.RPC("BuyTower", RpcTarget.AllViaServer, _towerCost);
    }

    private static bool CanAffordItem(int goldAmount, int cost)
    {
        return goldAmount >= cost;
    }
}
