using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    public bool muted;
    public Image spr;
    public Button self;
    public Sprite muteDef, unmuteDef;

    public delegate void Mute(bool condition); 
    public static event Mute  OnMuteCondition;


    public void ToggleMute()
    {
        AudioSource[] As = FindObjectsOfType<AudioSource>();
        muted = !muted;

        if (OnMuteCondition != null)
        {
            OnMuteCondition(muted);
        }

        if (muted)
        {
            //foreach (AudioSource audio in As)
            //{
                //audio.volume = 0;
            //}

            spr.sprite = unmuteDef;
        }
        else if (!muted)
        {
            //spr.color = Color.white;
            //foreach (AudioSource audio in As)
            //{
                //if (audio.tag == "sfx") audio.volume = 0.6f;
                //else if (audio.tag == "bg") audio.volume = 0.3f;
            //}

            spr.sprite = muteDef;
        }
    }
}
