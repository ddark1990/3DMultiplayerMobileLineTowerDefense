using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenController : MonoBehaviour
{
    public ParticleSystem goomerIncDisolver;
    public ParticleSystem AlphaPNGDisolver;

    void Start()
    {
        StartCoroutine(SplashCoroutine());
    }

    IEnumerator SplashCoroutine()
    {
        yield return new WaitForSeconds(.5f);
        AlphaPNGDisolver.gameObject.SetActive(true);
        yield return new WaitUntil(() => !AlphaPNGDisolver.isPlaying);
        AlphaPNGDisolver.gameObject.SetActive(false);

        yield return new WaitForSeconds(.2f);
        goomerIncDisolver.gameObject.SetActive(true);
        yield return new WaitUntil(() => !goomerIncDisolver.isPlaying);
        goomerIncDisolver.gameObject.SetActive(false);
        StartCoroutine(SceneFader.instance.FadeToScene("MenuScene", 1));
    }
}
