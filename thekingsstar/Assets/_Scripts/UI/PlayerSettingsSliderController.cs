using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSettingsSliderController : MonoBehaviour
{
    public PlayerSettingsScriptableObject PlayerSettings;
    public Slider JumpForce;

    void Start()
    {
        JumpForce.value = PlayerSettings.JumpForce;
    }

    void Update()
    {
        PlayerSettings.JumpForce = JumpForce.value;
    }
}
