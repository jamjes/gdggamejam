using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    public enum Sound
    {
        none, parry, slash, deactivate, idle
    };
    
    public AudioSource AsFX;
    public AudioSource AsMain;
    public AudioClip Parry, Slash, Deactivate, Idle;

    public void PlayAudio(Sound target)
    {
        switch(target)
        {
            case Sound.parry: AsFX.clip = Parry; break;
            
            case Sound.slash: AsFX.clip = Slash; break;

            case Sound.deactivate: AsFX.clip = Deactivate; break;

        }

        AsFX.Play();
    }

    public void PlayAudioContinuous(Sound target)
    {
        switch (target)
        {
            case Sound.idle: AsMain.clip = Idle; break;

        }

        AsMain.Play();
    }

    public void StopAudioContinuous()
    {
        AsMain.Stop();
    }
}
