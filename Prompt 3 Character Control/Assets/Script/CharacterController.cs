using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float moveSpeed;
    public float cameraSpeed;
    public Rigidbody rigidbody;
    public Animator anim;
    public GameObject cameraPoint;
    public Camera camera;
    private float speedup = 0.5f;
    private AnimatorStateInfo animStateInfo;

    float axisX;
    float axisY;
    // Start is called before the first frame update
    void Start()
    {
        this.animStateInfo = this.anim.GetCurrentAnimatorStateInfo(0);
        Cursor.visible = false;//
        Cursor.lockState = CursorLockMode.Locked;//
    }

    void Update()
    {
        this.axisX += Input.GetAxis("Mouse X") * cameraSpeed;
        this.axisY = Input.GetAxis("Mouse Y") * cameraSpeed; 
        this.transform.rotation = Quaternion.Euler(0, this.axisX, 0);

        //if ((this.camera.transform.eulerAngles.x >= 70.0f && this.axisY < 0)
        //    || (this.camera.transform.eulerAngles.x <= -70.0f && this.axisY > 0)
        //    ||(this.camera.transform.eulerAngles.x<=70.0f && this.camera.transform.eulerAngles.x>=-70.0f))
        float angle = this.camera.transform.eulerAngles.x + this.axisY;
        //if (angle > 180)
        //{
        //    angle -= 180.0f;
        //    angle = -angle;
        //}
        //if(angle < -85)
        //    this.camera.transform.RotateAround(this.cameraPoint.transform.position, this.transform.right, this.axisY - (-85 - angle));
        //else if (angle > 85)
        //    this.camera.transform.RotateAround(this.cameraPoint.transform.position, this.transform.right, this.axisY- (angle - 85));
        //else
        this.camera.transform.RotateAround(this.cameraPoint.transform.position, this.transform.right, this.axisY);
        Debug.Log(camera.transform.eulerAngles.x);
        //this.camera.transform.eulerAngles = new Vector3(this.camera.transform.rotation.x, this.camera.transform.rotation.y, 0);
        if (Input.GetKeyDown(KeyCode.Space) && !animStateInfo.IsName("Jump"))
        {
            this.anim.SetTrigger("Jump");
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            this.speedup += Time.deltaTime;
        }
        else
        {
            if (this.speedup > 0.5)
                this.speedup -= Time.deltaTime;
            else
                this.speedup = 0.5f;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical") * this.speedup;

        this.anim.SetFloat("Xspeed", h );
        this.anim.SetFloat("Yspeed", v );
       
        this.transform.Translate(h * Time.deltaTime * moveSpeed, 0, v * Time.deltaTime * moveSpeed);
    }
}
