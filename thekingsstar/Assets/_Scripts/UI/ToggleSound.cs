using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSound : MonoBehaviour
{

    bool mute = false;
    public void ToggleSoundMode()
    {
        mute = !mute;
    }
}
