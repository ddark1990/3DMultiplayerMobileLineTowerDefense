using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpSystem : MonoBehaviour
{
    public static PopUpSystem Instance;

    public GameObject PopupButtonPrefab;
    public GameObject ButtonHolder;
    public GameObject Canvas;
    public List<GameObject> InfoButtons;


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

    private IEnumerator PopUpInfoButton(string popupInfoText)
    {
        Canvas.SetActive(true);

        var popupButton = Instantiate(PopupButtonPrefab);
        InfoButtons.Add(popupButton);

        var buttonRect = popupButton.GetComponent<RectTransform>();
        var buttonPositionY = buttonRect.sizeDelta.y;

        var p = RectTransformUtility.WorldToScreenPoint(null, popupButton.transform.position);

        popupButton.transform.SetParent(ButtonHolder.transform, false);

        var buttonPadding = 2;
        if (InfoButtons.Count == 2)
        {
            popupButton.transform.position = new Vector3(buttonRect.position.x, popupButton.transform.position.y - buttonPositionY + 30 - buttonPadding, buttonRect.position.z);
        }

        //buttonRect.position = new Vector3(0, 0, 0);

        popupButton.GetComponent<PopUpButton>().ButtonText.text = popupInfoText;
        yield return new WaitForSeconds(3);

        Canvas.SetActive(false);
        InfoButtons.Remove(popupButton);
        Destroy(popupButton);
    }

    public void SelfConnected_Message()
    {
        StartCoroutine(PopUpInfoButton("Connected to network."));
    }

    public void SelfDisconnected_Message()
    {
        StartCoroutine(PopUpInfoButton("Disconnected From network."));
    }

    public void PlayerJoinedRoom_Message(Player player)
    {
        StartCoroutine(PopUpInfoButton(player.NickName + " joined the room."));
    }

    public void PlayerLeftRoom_Message(Player player)
    {
        StartCoroutine(PopUpInfoButton(player.NickName + " has left the room."));
    }
}
