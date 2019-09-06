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

    private void Update()
    {

    }

    private void PopUpInfo(string popupInfoText)
    {
        Canvas.SetActive(true);

        var popupButton = Instantiate(PopupButtonPrefab);
        InfoButtons.Add(popupButton);

        var p = RectTransformUtility.WorldToScreenPoint(null, popupButton.transform.position);

        popupButton.transform.SetParent(ButtonHolder.transform, false);

        //for (int i = 0; i < InfoButtons.Count; i++)
        //{
        //    var button = InfoButtons[i];

        //    var buttonRect = button.GetComponent<RectTransform>();

        //    const int buttonPadding = 2;
        //    if (InfoButtons.Count > 1)
        //    {
        //        button.transform.position = new Vector3(buttonRect.position.x, buttonRect.transform.position.y - 60 - buttonPadding, buttonRect.position.z);
        //        break;
        //    }

        //}

        ////buttonRect.position = new Vector3(0, 0, 0);

        popupButton.GetComponent<PopUpButton>().ButtonText.text = popupInfoText;

        PopUpInfoTimer();

        if (!(PopupWaitTime <= 0)) return;

        Canvas.SetActive(false);
        InfoButtons.Remove(popupButton);
        Destroy(popupButton);
    }

    private void PopUpInfoTimer()
    {
        var waitTime = PopupWaitTime;

        while (PopupWaitTime > 0)
        {
            PopupWaitTime -= Time.deltaTime;

            //PopupWaitTime = waitTime;
            Debug.Log("tidies");
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
