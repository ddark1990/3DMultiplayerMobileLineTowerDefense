using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Photon.Pun;
using UnityEngine;

public class GoogleSignIn : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated()) return;

        SignInPlayGames();
    }

    public void SignInPlayGames()
    {
        //if (Social.localUser.userName.StartsWith(""))
        //{
        //    PhotonNetwork.NickName = "Player" + Random.Range(1, 10000);
        //}

        Debug.Log("GooglePlayLogin Attemp...");
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
            ConnectionInfoPanel.instance.playerNameText.text = PhotonNetwork.NickName;
            //FindObjectOfType<MainUI>().playerNameText.text = Social.localUser.userName;
            //FindObjectOfType<MainUI>().playerSprite = Social.localUser.image;
            //FindObjectOfType<MainUI>().playerId = int.Parse(Social.localUser.id);
            Debug.LogWarning("Authenticated!");
        }
        else
        {
            PhotonNetwork.NickName = "Player" + Random.Range(1, 10000);
            ConnectionInfoPanel.instance.playerNameText.text = PhotonNetwork.NickName;
            Debug.LogWarning("Failed to authenticate!");
        }
    }

    public override void OnConnected()
    {

    }
}
