using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //Audio
    [SerializeField] public AudioClip playerDeathSound, playerFireSound, playerMoveSound, enemyFireSound;
    AudioSource audioSrc;

    void Start ()
    {
        audioSrc = GetComponent<AudioSource>();
    }


    //sound code
    public void PlaySound(string clip)
    {
        switch(clip)
        {
            case "playerFire":
                audioSrc.PlayOneShot(playerFireSound);
                break;
            case "death":
                audioSrc.PlayOneShot(playerDeathSound);
                break;
            case "playerMove":
                audioSrc.PlayOneShot(playerMoveSound);
                break;
            case "enemyFire":
                audioSrc.PlayOneShot(enemyFireSound);
                break;
        }
    }

}
