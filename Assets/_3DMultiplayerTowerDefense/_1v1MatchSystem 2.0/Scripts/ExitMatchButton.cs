using Photon.Pun;
using UnityEngine.SceneManagement;

namespace MatchSystem
{
    public class ExitMatchButton : MonoBehaviourPunCallbacks
    {
        public void ExitMatch()
        {
            PhotonNetwork.Disconnect();

            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(3))
            {
                StartCoroutine(SceneFader.instance.FadeToScene("MenuScene2.0", 1));
            }
        }

    }
}