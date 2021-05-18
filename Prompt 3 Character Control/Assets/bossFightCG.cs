using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class bossFightCG : MonoBehaviour
{
    public GameObject player;
    public PlayableDirector director;

    private bool playing;
    // Update is called once per frame
    void Update()
    {

        if (director.state == PlayState.Playing)
        {
            Debug.Log("is playing");
            player.GetComponent<PlayerController>().enabled = false;

        }
        else
        {
            player.GetComponent<PlayerController>().enabled = true;

        }

    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
        if(other.tag == "Player")
        {
            director.Play();
            Destroy(gameObject, 1f);
        }
    }
}
