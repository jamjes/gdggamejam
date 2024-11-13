using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] GameObject[] toggleElements;
    [SerializeField] TMP_Text text, playerHealth, enemyHealth;
    bool isShowing;

    private void Awake()
    {
        HideElements();
        text.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Bullet.OnTutorialEnter += ShowElements;
        Player.OnMiss += MissUI;
    }

    private void OnDisable()
    {
        Bullet.OnTutorialEnter -= ShowElements;
        Player.OnMiss -= MissUI;
    }

    private void ShowElements()
    {
        foreach (GameObject element in toggleElements)
        {
            element.SetActive(true);
        }
    }

    private void HideElements()
    {
        foreach (GameObject element in toggleElements)
        {
            element.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))
        {
            HideElements();
        }
    }

    private void MissUI()
    {
        if (isShowing)
        {
            return;
        }

        StartCoroutine(FlashMiss());
    }

    private IEnumerator FlashMiss()
    {
        isShowing = true;
        text.gameObject.SetActive(true);
        yield return new WaitForSeconds(.3f);
        text.gameObject.SetActive(false);
        isShowing = false;
    }

    private void UpdateHealth(string name, int health)
    {
        if (name == "player")
        {
            playerHealth.text = health.ToString();
            return;
        }

        enemyHealth.text = health.ToString();
    }
}
