using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks.Sources;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerController.OnDeathEnter += StopAudioContinuous;
    }

    private void OnDisable()
    {
        PlayerController.OnDeathEnter -= StopAudioContinuous;
    }

    public enum Sound
    {
        none, parry, slash, deactivate, idle, jump, death, damage
    };
    
    public AudioSource AsFX;
    public AudioSource AsMain;
    public AudioClip Parry, Slash, Deactivate, Idle, Jump, Death, Damage;

    public void PlayAudio(Sound target)
    {
        switch(target)
        {
            case Sound.parry: AsFX.clip = Parry; break;
            
            case Sound.slash: AsFX.clip = Slash; break;

            case Sound.deactivate: AsFX.clip = Deactivate; break;

            case Sound.jump: AsFX.clip = Jump; break;

            case Sound.death: AsFX.clip = Death; break;

            case Sound.damage: AsFX.clip = Damage; break;
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
        if (AsMain != null ) AsMain.Stop();
    }
}
