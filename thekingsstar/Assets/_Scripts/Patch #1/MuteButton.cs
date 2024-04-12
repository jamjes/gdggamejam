using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    public bool muted;
    public Image spr;

    public void ToggleMute()
    {
        AudioSource[] As = FindObjectsOfType<AudioSource>();
        muted = !muted;

        if (muted)
        {
            spr.color = Color.grey;
            foreach (AudioSource audio in As)
            {
                audio.volume = 0;
            }
        }
        else if (!muted)
        {
            spr.color = Color.white;
            foreach (AudioSource audio in As)
            {
                if (audio.tag == "sfx") audio.volume = 0.6f;
                else if (audio.tag == "bg") audio.volume = 0.3f;
            }
        }
    }
}
