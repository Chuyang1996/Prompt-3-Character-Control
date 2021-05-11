using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        this.audioManager.InitAudio();
        AudioManager.Instance.PlayMusic("");
    }


}
