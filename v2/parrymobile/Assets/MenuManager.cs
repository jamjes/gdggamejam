using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Canvas _startCanvas;
    [SerializeField] Canvas _menuCanvas;

    private void Awake()
    {
        _startCanvas.gameObject.SetActive(true);
        _menuCanvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ToggleCanvas();
        }
    }

    private void ToggleCanvas()
    {
        if (_startCanvas.gameObject.activeSelf)
        {
            _startCanvas.gameObject.SetActive(false);
            _menuCanvas.gameObject.SetActive(true);
        }
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }
}
