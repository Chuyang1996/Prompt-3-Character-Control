using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoBehaviour
{
    public Button startGameBtn;
    public Button exitGameBtn;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;//
        Cursor.lockState = CursorLockMode.None;//
        this.startGameBtn.onClick.AddListener(() => { Application.LoadLevel(1); });
        this.exitGameBtn.onClick.AddListener(() => { Application.Quit(); });
    }

}
