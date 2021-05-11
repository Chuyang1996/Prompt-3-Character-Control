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
        this.axisY = Input.GetAxis("Mouse Y") * -cameraSpeed; 
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
        //Debug.Log(camera.transform.eulerAngles.x);
        //this.camera.transform.eulerAngles = new Vector3(this.camera.transform.rotation.x, this.camera.transform.rotation.y, 0);
        if (Input.GetKeyDown(KeyCode.Space) && !animStateInfo.IsName("Jump"))
        {
            this.anim.SetTrigger("Jump");
        }
        this.animStateInfo = this.anim.GetCurrentAnimatorStateInfo(0);
        //if (Input.GetMouseButton(1))
        //{
        //    this.anim.SetLayerWeight(1, 1);
        //    this.anim.SetLayerWeight(0, 0);
        //    return;
        //}
        //else
        //{
        //    this.anim.SetLayerWeight(0, 1);
        //    this.anim.SetLayerWeight(1, 0);
        //}
        if (Input.GetMouseButtonDown(0) && this.animStateInfo.normalizedTime > 0.5f && this.animStateInfo.IsName("Attack2"))
        {
            Debug.Log("ssss");
            this.anim.SetTrigger("Attack3");
            this.anim.ResetTrigger("Attack1");
            this.anim.ResetTrigger("Attack2");
        }
        else if (Input.GetMouseButtonDown(0) && this.animStateInfo.normalizedTime > 0.5f && this.animStateInfo.IsName("Attack1") )
        {
            Debug.Log("aaaa");
            this.anim.SetTrigger("Attack2");
            this.anim.ResetTrigger("Attack1");
            this.anim.ResetTrigger("Attack3");
        }
        else if (Input.GetMouseButtonDown(0)&& this.animStateInfo.IsName("BattleState"))
        {
            this.anim.SetTrigger("Attack1");
            this.anim.ResetTrigger("Attack2");
            this.anim.ResetTrigger("Attack3");
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical") ;
        if(h == 0 && v == 0)
        {
            this.speedup = 0.0f;
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                this.speedup += Time.deltaTime;
                if (this.speedup > 1.0f)
                    this.speedup = 1.0f;
            }
            else
            {

                if (this.speedup > 0.5)
                    this.speedup -= Time.deltaTime;
                else
                    this.speedup = 0.5f;


            }
        }
        v *= this.speedup;
        this.anim.SetFloat("Xspeed", h );
        this.anim.SetFloat("Yspeed", v );
        //Debug.Log(this.speedup);
        this.transform.Translate(h * Time.deltaTime * moveSpeed, 0, v * Time.deltaTime * moveSpeed);
        AnimatorStateInfo animStateInfoTemp = anim.GetCurrentAnimatorStateInfo(0);
        //Debug.Log(animStateInfoTemp.normalizedTime);


    }
}
