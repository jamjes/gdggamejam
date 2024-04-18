using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CanvasController : MonoBehaviour
{
    public bool muted;
    public Image SoundImage, PauseImage;
    public Sprite muteSprite, unmuteSprite, PausedSprite, PlaySprite;
    public delegate void Mute(bool condition);
    public static event Mute OnMuteCondition;
    public TextMeshProUGUI timerLabel;

    public GameObject AdditionalSettings; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleSound();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (FindObjectOfType<PlayerController>().IsDead || !GameManager.Instance.Run)
        {
            return;
        }

        timerLabel.text = GameManager.Instance.TimeReference.ToString("00:00");
    }

    public void TogglePause()
    {
        GameManager.Instance.ToggleRunState();

        if (GameManager.Instance.Run == false &&
            PauseImage.sprite != PausedSprite)
        {
            PauseImage.sprite = PausedSprite;
            AdditionalSettings.SetActive(true);
        }
        else if (GameManager.Instance.Run == true &&
            PauseImage.sprite != PlaySprite)
        {
            PauseImage.sprite = PlaySprite;
            AdditionalSettings.SetActive(false);
        }
    }

    public void ToggleSound()
    {
        muted = !muted;

        if (OnMuteCondition != null)
        {
            OnMuteCondition(muted);
        }

        if (muted)
        {
            SoundImage.sprite = unmuteSprite;
        }
        else if (!muted)
        {
            SoundImage.sprite = muteSprite;
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnEnable()
    {
        EnemyTurret.OnTurretDeath += TogglePause;
    }

    private void OnDisable()
    {
        EnemyTurret.OnTurretDeath -= TogglePause;
    }
}
