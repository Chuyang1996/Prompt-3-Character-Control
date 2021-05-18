using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimationSoundEvent : MonoBehaviour
{
    public AudioSource currentAudio;
    public AudioClip[] audios;
    private void Walk()
    {
        this.currentAudio.clip = audios[0];
        this.currentAudio.loop = false;
        this.currentAudio.Play();

    }
    private void Jump()
    {
        this.currentAudio.clip = audios[1];
        this.currentAudio.loop = false;
        this.currentAudio.Play();

    }
    private void Land()
    {
        this.currentAudio.clip = audios[2];
        this.currentAudio.loop = false;
        this.currentAudio.Play();

    }

    private void Attack1()
    {
        this.currentAudio.clip = audios[3];
        this.currentAudio.loop = false;
        this.currentAudio.Play();

    }
    private void Attack2()
    {
        this.currentAudio.clip = audios[4];
        this.currentAudio.loop = false;
        this.currentAudio.Play();

    }
    private void Attack3One()
    {
        this.currentAudio.clip = audios[5];
        this.currentAudio.loop = false;
        this.currentAudio.Play();

    }
    private void Attack3Sec()
    {
        this.currentAudio.clip = audios[6];
        this.currentAudio.loop = false;
        this.currentAudio.Play();

    }

    private void CreateFireBall()
    {
        this.currentAudio.clip = audios[7];
        this.currentAudio.loop = false;
        this.currentAudio.Play();
    }

    private void ShootFireBall()
    {
        this.currentAudio.clip = audios[8];
        this.currentAudio.loop = false;
        this.currentAudio.Play();
    }

    private void ThrowStone()
    {
        this.currentAudio.clip = audios[9];
        this.currentAudio.loop = false;
        this.currentAudio.Play();
    }


    private void LayDeath()
    {
        this.currentAudio.clip = audios[10];
        this.currentAudio.loop = false;
        this.currentAudio.Play();
    }

}
