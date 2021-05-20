using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISceneOne : MonoBehaviour
{
    public GameObject mainPanel;
    public Button resumeBtn;
    public Button exitBtn;
    // Start is called before the first frame update
    void Start()
    {
        this.resumeBtn.onClick.AddListener(() => {
            Cursor.visible = false;//
            Cursor.lockState = CursorLockMode.Locked;//
            this.mainPanel.SetActive(false);
            Time.timeScale = 1.0f;
        });
        this.exitBtn.onClick.AddListener(() => { Time.timeScale = 1.0f; DestroyImmediate(AudioManager.Instance.gameObject); Application.LoadLevel(0);  });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;//
            Cursor.lockState = CursorLockMode.None;//
            this.mainPanel.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }
}
