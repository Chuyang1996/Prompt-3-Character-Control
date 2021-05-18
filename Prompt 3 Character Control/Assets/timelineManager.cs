using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class timelineManager : MonoBehaviour
{
    public GameObject player;
    public PlayableDirector director;


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

}
