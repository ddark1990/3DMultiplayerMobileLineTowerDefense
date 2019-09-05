using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenController : MonoBehaviour
{
    public ParticleSystem goomerIncDisolver;

    void Start()
    {
        StartCoroutine(SplashCoroutine());
    }

    IEnumerator SplashCoroutine()
    {
        yield return new WaitForSeconds(.5f);
        goomerIncDisolver.gameObject.SetActive(true);
        yield return new WaitUntil(() => !goomerIncDisolver.isPlaying);
        goomerIncDisolver.gameObject.SetActive(false);
        //SceneManager.LoadScene("MenuScene");
        StartCoroutine(SceneFader.instance.FadeToScene("MenuScene", 1));
    }
}
