using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseToggle : MonoBehaviour
{
    public bool paused;
    public GameObject pauseMenu;

    public void Toggle()
    {
        paused = !paused;
        GameManager.Instance.ToggleRunState();

        if (paused)
        {
            pauseMenu.SetActive(true);
        }
        else
        {
            Debug.Log("Toggle OFF");
            pauseMenu.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Toggle();
        }
    }
}
