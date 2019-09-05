using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationSet : MonoBehaviour
{
    public bool skipIntro;

    private void Awake()
    {
        if(this != null)
        {
            Destroy(this);
        }
        Application.targetFrameRate = 60;
        SignInPlayGames();
        LoadIntro();
    }

    private void LoadIntro()
    {
        if (skipIntro)
        {
            SceneManager.LoadScene(2);
            return;
        }

        SceneManager.LoadScene(1);
    }

    public void SignInPlayGames()
    {
        PlayGamesClientConfiguration.Builder build = new PlayGamesClientConfiguration.Builder();
        PlayGamesPlatform.InitializeInstance(build.Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        Social.localUser.Authenticate(ProcessGooglePlayAuth);
    }

    void ProcessGooglePlayAuth(bool success)
    {
        if (success)
        {
            PhotonNetwork.NickName = Social.localUser.userName;
            //FindObjectOfType<MainUI>().playerNameText.text = Social.localUser.userName;
            FindObjectOfType<MainUI>().playerSprite = Social.localUser.image;
            FindObjectOfType<MainUI>().playerId = int.Parse(Social.localUser.id);
        }
        else if (PhotonNetwork.NickName.Equals(""))
        {
            PhotonNetwork.NickName = "Player" + Random.Range(1,10000);
            Debug.LogWarning("Failed to authenticate!");
        }
    }
}
