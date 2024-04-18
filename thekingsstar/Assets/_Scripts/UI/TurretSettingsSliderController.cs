using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurretSettingsSliderController : MonoBehaviour
{
    public TurretSettingsScriptableObject TurretSettings;
    public Slider HealthSlider, DelaySlider, ReloadSlider;
    public TextMeshProUGUI HealthLabel, DelayLabel, ReloadLabel;

    private void OnEnable()
    {
        StartButton.OnGameBegin += HideUI;
    }

    private void OnDisable()
    {
        StartButton.OnGameBegin -= HideUI;
    }

    private void Start()
    {
        HealthSlider.value = TurretSettings.Health;
        DelaySlider.value = TurretSettings.DelayDuration;
        ReloadSlider.value = TurretSettings.ReloadSpeed;
    }

    public void UpdateTurretHealth()
    {
        TurretSettings.Health = HealthSlider.value;
        HealthLabel.text = TurretSettings.Health.ToString();
    }

    public void UpdateTurretDelay()
    {
        TurretSettings.DelayDuration = DelaySlider.value;
        DelayLabel.text = DelaySlider.value.ToString();
    }

    public void UpdateTurretReload()
    {
        TurretSettings.ReloadSpeed = ReloadSlider.value;
        ReloadLabel.text = ReloadSlider.value.ToString();
    }

    void HideUI()
    {
        gameObject.SetActive(false);
    }
}
