using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationSet : MonoBehaviour
{
    public bool skipIntro;

    public GameObject PopupSystemPrefab;

    private void Awake()
    {
        if(this != null)
        {
            Destroy(this);
        }

        Instantiate(PopupSystemPrefab);

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
        //if (Social.localUser.userName.StartsWith(""))
        //{
        //    PhotonNetwork.NickName = "Player" + Random.Range(1, 10000);
        //}

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
        else
        {
            PhotonNetwork.NickName = "Player" + Random.Range(1,10000);
            Debug.LogWarning("Failed to authenticate!");
        }
    }
}
