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

}
