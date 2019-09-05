using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LookAtCamera : MonoBehaviour
{
    Camera cam;

    void Start()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("GameScene"))
        {
            cam = FindObjectOfType<Camera>();
        }
    }

    void Update()
    {
        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("GameScene"))
        {
            transform.LookAt(cam.transform);
        }
    }
}
