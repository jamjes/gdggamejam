using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextScene();
        }
    }

    void NextScene()
    {
        //Coroutine FadeOut

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
