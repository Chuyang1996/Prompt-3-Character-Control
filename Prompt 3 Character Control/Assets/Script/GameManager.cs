using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AudioManager audioManager;
    public bool isBattle;
    // Start is called before the first frame update
    void Start()
    {
        this.isBattle = false;
        this.audioManager.InitAudio();
        AudioManager.Instance.musicVolume = 0.2f;
        AudioManager.Instance.PlayAudio(this.transform, "Desert",true);
    }


}
