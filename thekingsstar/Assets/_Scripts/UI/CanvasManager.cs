using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class CanvasManager : MonoBehaviour
{
    public MuteButton muteButton;
    public RestartButton restartButton;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            muteButton.ToggleMute();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            restartButton.RestartScene();
        }
    }

    
}
