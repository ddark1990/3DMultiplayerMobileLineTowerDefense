using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class PopUpSystem : MonoBehaviour
{
    public static PopUpSystem Instance;

    public GameObject PopupButtonPrefab;
    public GameObject ButtonHolder;
    public GameObject Canvas;
    public List<GameObject> InfoButtons;
    public float PopupWaitTime = 5;


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
            DontDestroyOnLoad(Instance);
        }

        Canvas.SetActive(false);
    }

    private void PopUpInfo(string popupInfoText)
    {
        Canvas.SetActive(true);

        var popupButton = Instantiate(PopupButtonPrefab);
        InfoButtons.Add(popupButton);

        var p = RectTransformUtility.WorldToScreenPoint(null, popupButton.transform.position);

        popupButton.transform.SetParent(ButtonHolder.transform, false);

        for (int i = 0; i < InfoButtons.Count; i++)
        {
            var button = InfoButtons[i];

            var buttonRect = button.GetComponent<RectTransform>();

            const int buttonPadding = 2;

            if (InfoButtons.Count > 1)
            {
                button.transform.position = new Vector3(buttonRect.position.x, buttonRect.transform.position.y - 20 - buttonPadding, buttonRect.position.z);
                break;
            }
        }

        //buttonRect.position = new Vector3(0, 0, 0);

        StartCoroutine(PopUpInfoTimer(popupButton, popupInfoText));
    }

    private IEnumerator PopUpInfoTimer(GameObject obj, string infoText)
    {
        obj.GetComponent<PopUpButton>().ButtonText.text = infoText;

        yield return new WaitForSeconds(PopupWaitTime);

        InfoButtons.Remove(obj);
        Destroy(obj);

        if (InfoButtons.Count == 0)
        {
            Canvas.SetActive(false);
        }
    }


    public void SelfConnected_Message()
    {
        PopUpInfo("Connected to network.");
    }

    public void SelfDisconnected_Message()
    {
        PopUpInfo("Disconnected From network.");
    }

    public void PlayerJoinedRoom_Message(Player player)
    {;
        PopUpInfo(player.NickName + " joined the room.");
    }

    public void PlayerLeftRoom_Message(Player player)
    {
        PopUpInfo(player.NickName + " has left the room.");
    }
}
