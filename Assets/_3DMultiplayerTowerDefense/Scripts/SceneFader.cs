using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    public static SceneFader instance;

    public int currentSceneIndex;
    public string currentSceneName;

    public Animator animator;

    void Update()
    {
        GetSceneInfo();
    }

    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }

        SceneManager.activeSceneChanged += ChangedActiveScene;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= ChangedActiveScene;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void ChangedActiveScene(Scene current, Scene next)
    {
        
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

    }

    public void OnSceneUnloaded(Scene current)
    {

    }

    public IEnumerator FadeToScene(string scene, int waitTime)
    {
        animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(scene);
    }

    public IEnumerator FadeToNetworkScene(string scene, int waitTime)
    {
        animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(waitTime);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(scene);
        }
    }

    void GetSceneInfo()
    {
        GetSceneIndex(SceneManager.GetActiveScene().buildIndex);
        GetSceneName(SceneManager.GetActiveScene().name);
    }

    void GetSceneIndex(int index)
    {
        currentSceneIndex = index;
    }

    void GetSceneName(string sceneName)
    {
        currentSceneName = sceneName;
    }
}
