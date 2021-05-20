using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationSoundEvent : MonoBehaviour
{
    public AudioSource currentAudio;
    public AudioClip[] audios;
    private void Walk()
    {
        this.currentAudio.clip = audios[0];
        this.currentAudio.loop = false;
        this.currentAudio.Play();

    }

    private void Run() { 
        this.currentAudio.clip = audios[1];
        this.currentAudio.loop = false;
        this.currentAudio.Play();
    }
    private void Dodge()
    {
        this.currentAudio.clip = audios[2];
        this.currentAudio.loop = false;
        this.currentAudio.Play();

    }
    private void Landing()
    {
        this.currentAudio.clip = audios[3];
        this.currentAudio.loop = false;
        this.currentAudio.Play();

    }
    private void Roll()
    {
        this.currentAudio.clip = audios[4];
        this.currentAudio.loop = false;
        this.currentAudio.Play();
    }
    private void Attack1()
    {
        this.currentAudio.clip = audios[5];
        this.currentAudio.loop = false;
        this.currentAudio.Play();

    }
    private void Attack2()
    {
        this.currentAudio.clip = audios[6];
        this.currentAudio.loop = false;
        this.currentAudio.Play();

    }
    private void Attack3()
    {
        this.currentAudio.clip = audios[7];
        this.currentAudio.loop = false;
        this.currentAudio.Play();

    }

    private void Hit1()
    {
        this.currentAudio.clip = audios[8];
        this.currentAudio.loop = false;
        this.currentAudio.Play();
    }

    private void Lay1()
    {
        this.currentAudio.clip = audios[9];
        this.currentAudio.loop = false;
        this.currentAudio.Play();
    }
    private void Lay2()
    {
        this.currentAudio.clip = audios[10];
        this.currentAudio.loop = false;
        this.currentAudio.Play();
    }
    private void Defend()
    {
        this.currentAudio.clip = audios[11];
        this.currentAudio.loop = false;
        this.currentAudio.Play();
    }
}
