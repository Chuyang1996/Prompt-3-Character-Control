using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scenetransition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Image img = this.GetComponent<Image>();
        var tempcolor = img.color;

        if (this.gameObject.activeSelf)
        {
            Debug.Log("transition");
            tempcolor.a += Time.deltaTime * 2f;
            
        }

        img.color = tempcolor;
    }
}
