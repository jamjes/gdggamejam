using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSound : MonoBehaviour
{
    public AudioSource source;
    public AudioClip miss, parry;

    public void PlaySound(bool parrySuccess)
    {
        if (parrySuccess == true)
        {
            source.PlayOneShot(parry);
            return;
        }

        source.PlayOneShot(miss);
    }
}
