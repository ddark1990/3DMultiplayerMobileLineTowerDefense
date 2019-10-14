using Photon.Pun;
using UnityEngine.SceneManagement;

public class ExitMatchButton : MonoBehaviourPunCallbacks
{
    public void ExitMatch()
    {
        PhotonNetwork.LeaveRoom();

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("GameScene"))
        {
            StartCoroutine(SceneFader.instance.FadeToScene("MenuScene", 1));
        }
    }
}
