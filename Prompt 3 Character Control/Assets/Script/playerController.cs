using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameManager gameManager;
    public bool isDead = false;
    public GameObject badResult;
    [Range(0,100)]
    public float healthPoint;
    public Slider healthBar;

    private float healthMax;

    public bool isDefend = false;
    public bool isHeal = false;

    #region  //Physics
    public float moveSpeed;
    public float cameraSpeed;
    public Rigidbody rigidbody;
    public Animator anim;
    public Transform camera;
    private float speedup = 0.5f;
    private AnimatorStateInfo animStateInfoZero;
    private AnimatorStateInfo animStateInfoOne;
    #endregion


    [Range(3, 6)]
    private float battleTime;

    private float battleTimeCount;



    float weight;

    private AudioSource currentAudio;

    float turnSmoothT = 0.1f;
    float turnSmoothV;

    public ParticleSystem dust;
    // Start is called before the first frame update
    void Start()
    {
        this.healthMax = this.healthPoint;
        this.battleTimeCount = this.battleTime;
        this.animStateInfoZero = this.anim.GetCurrentAnimatorStateInfo(0);
        Cursor.visible = false;//
        Cursor.lockState = CursorLockMode.Locked;//
    }

    void Update()
    {
        //this.axisX += Input.GetAxis("Mouse X") * cameraSpeed;
        //this.axisY = Input.GetAxis("Mouse Y") * -cameraSpeed;
        this.IsDead();
        float targetAngle = camera.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothV, turnSmoothT);
        this.transform.rotation = Quaternion.Euler(0, angle, 0);

        //if ((this.camera.transform.eulerAngles.x >= 70.0f && this.axisY < 0)
        //    || (this.camera.transform.eulerAngles.x <= -70.0f && this.axisY > 0)
        //    ||(this.camera.transform.eulerAngles.x<=70.0f && this.camera.transform.eulerAngles.x>=-70.0f))
        //float angle = this.camera.transform.eulerAngles.x + this.axisY;
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
        //this.camera.transform.RotateAround(this.cameraPoint.transform.position, this.transform.right, this.axisY);
        //Debug.Log(camera.transform.eulerAngles.x);
        //this.camera.transform.eulerAngles = new Vector3(this.camera.transform.rotation.x, this.camera.transform.rotation.y, 0);
        //if (Input.GetKeyDown(KeyCode.Space) && !animStateInfoZero.IsName("Jump"))
        //{
        //    this.anim.SetTrigger("Jump");
        //}
        if (this.IfCannotControl())
            return;

        this.animStateInfoZero = this.anim.GetCurrentAnimatorStateInfo(0);

        //if (this.IsHeal())
        //    return;
        this.anim.SetBool("isDefend", this.isDefend);
        if (Input.GetMouseButton(1) && (this.animStateInfoZero.IsName("BattleState") || this.animStateInfoZero.IsName("NormalState")))
        {
            this.weight = Mathf.Lerp(this.weight, 1, Time.deltaTime * 8.0f);
            this.anim.SetLayerWeight(1, weight);
            this.anim.SetLayerWeight(0, 0);
            this.isDefend = true;
            
            return;
        }
        else 
        {
            this.isDefend = false;
            this.weight = Mathf.Lerp(this.weight, 0, Time.deltaTime * 8.0f);
            this.anim.SetLayerWeight(0, 1);
            this.anim.SetLayerWeight(1, this.weight);
        }
        if (Input.GetKeyDown(KeyCode.Space) && !this.animStateInfoZero.IsName("Dodge"))
        {
            if ((this.animStateInfoZero.IsName("Attack1") || this.animStateInfoZero.IsName("Attack2") || this.animStateInfoZero.IsName("Attack3")) && this.animStateInfoZero.normalizedTime < 0.5f)
                return;
            this.anim.SetTrigger("Dodge");
        }
        else if (Input.GetMouseButtonDown(0) && this.animStateInfoZero.normalizedTime > 0.5f && this.animStateInfoZero.IsName("Attack2"))
        {
            Debug.Log("ssss");
            this.anim.SetTrigger("Attack3");
            this.anim.ResetTrigger("Attack1");
            this.anim.ResetTrigger("Attack2");
        }
        else if (Input.GetMouseButtonDown(0) && this.animStateInfoZero.normalizedTime > 0.5f && this.animStateInfoZero.IsName("Attack1"))
        {
            Debug.Log("aaaa");
            this.anim.SetTrigger("Attack2");
            this.anim.ResetTrigger("Attack1");
            this.anim.ResetTrigger("Attack3");
        }
        else if (Input.GetMouseButtonDown(0) && (this.animStateInfoZero.IsName("BattleState")|| this.animStateInfoZero.IsName("NormalState")))
        {
            
            this.battleTimeCount = 0.0f;
            this.anim.SetTrigger("Attack1");
            this.anim.ResetTrigger("Attack2");
            this.anim.ResetTrigger("Attack3");
            StartCoroutine(BattleTimeCount());
        }

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.IfCannotControl())
            return;
        float h = Input.GetAxis("Horizontal");
        float xD = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical") ;
        float yD = Input.GetAxis("Vertical") ;
        if(h == 0 && v == 0)
        {
            this.speedup = 0.0f;
            dust.enableEmission = false;
        }
        else
        {
            dust.enableEmission = true;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                this.speedup += Time.deltaTime;
                if (this.speedup > 1.0f)
                    this.speedup = 1.0f;
            }
            else
            {
                if (this.speedup > 0.6)
                    this.speedup -= Time.deltaTime;
                else if(this.speedup < 0.5)
                    this.speedup += Time.deltaTime;



            }
        }
        v *= this.speedup;
        h *= this.speedup;
        this.animStateInfoZero = this.anim.GetCurrentAnimatorStateInfo(0);
        if (Input.GetMouseButton(1))
        {
            if (this.speedup > 0.5)
                this.speedup -= Time.deltaTime;
            else
                this.speedup = 0.5f;
        }
        
        if (this.anim.GetLayerWeight(1) > 0.1f)
        {
            if (this.speedup > 0.5)
                this.speedup -= Time.deltaTime;
            else
                this.speedup = 0.5f;
        }

        if (this.animStateInfoZero.IsName("Dodge"))
        {
            this.speedup = 0.5f;
            this.anim.SetFloat("Xspeed", xD * 0.5f);
            this.anim.SetFloat("Yspeed", yD *0.5f);
        }
        else
        {
            this.anim.SetFloat("Xdir", xD);
            this.anim.SetFloat("Ydir", yD);
            this.anim.SetFloat("Xspeed", h);
            this.anim.SetFloat("Yspeed", v);
        }

        //Debug.Log(this.speedup);
        if (!this.animStateInfoZero.IsName("Attack1") && !this.animStateInfoZero.IsName("Attack2") && !this.animStateInfoZero.IsName("Attack3"))
        {
            
            this.transform.Translate(h * Time.deltaTime * moveSpeed, 0, v * Time.deltaTime * moveSpeed);
        }
        //Debug.Log(animStateInfoTemp.normalizedTime);


    }

    //void PlaySound(float xS, float yS)
    //{
    //    if (this.animStateInfoZero.IsName("NormalState") || this.animStateInfoZero.IsName("BattleState"))
    //    {
    //        this.currentAudio = AudioManager.Instance.PlayAudio(this.transform, "Run",true);
    //    }
    //    else if (this.animStateInfoZero.IsName("Dodge"))
    //    {
    //        if (this.currentAudio != null)
    //            AudioManager.Instance.StopSound(this.currentAudio);
    //    }
    //    else if (this.animStateInfoZero.IsName("Attack1"))
    //    {
    //        if (this.currentAudio != null)
    //            AudioManager.Instance.StopSound(this.currentAudio);
    //    }
    //    else if (this.animStateInfoZero.IsName("Attack2"))
    //    {
    //        if (this.currentAudio != null)
    //            AudioManager.Instance.StopSound(this.currentAudio);
    //    }
    //    else if (this.animStateInfoZero.IsName("Attack3"))
    //    {
    //        if (this.currentAudio != null)
    //            AudioManager.Instance.StopSound(this.currentAudio);
    //    }
    //}

    IEnumerator BattleTimeCount()
    {
        yield return new WaitForSeconds(this.battleTime);
        if(!this.gameManager.isBattle)
            this.anim.SetBool("Battle", false);

    }


    public void HitFly()
    {
        this.anim.SetTrigger("Hit2");
    }

    public void Hit()
    {
        this.anim.SetTrigger("Hit1");
    }

    public void IsDead()
    {
        if (this.isDead)
        {
            this.badResult.SetActive(true);
            this.healthBar.value = 0.0f;
            return;
        }
        this.healthBar.value = this.healthPoint / this.healthMax;
        if(this.healthPoint <= 0.0f)
        {
            this.isDead = true;
            this.anim.SetBool("isDead",this.isDead);
        }
    }
    public bool IfCannotControl()
    {
        this.animStateInfoZero = this.anim.GetCurrentAnimatorStateInfo(0);
        if (this.animStateInfoZero.IsName("HurtFly")|| this.animStateInfoZero.IsName("Hurt") || this.animStateInfoZero.IsName("Death") || this.animStateInfoZero.IsName("Laying Death") || this.animStateInfoZero.IsName("NormalDeath"))
        {
            return true;
        }
        return false;
    }

    public bool IsHeal()
    {
        this.animStateInfoOne = this.anim.GetCurrentAnimatorStateInfo(2);
        if (Input.GetKeyDown(KeyCode.E) && !this.isHeal)
        {
            this.isHeal = true;
        }
        //if (this.animStateInfoOne.IsName("Heal") && this.animStateInfoOne.length > 0.8f)
        //{
        //    this.isHeal = false;
        //}
        if (this.isHeal)
        {
            Debug.Log("SSSSSS");
            this.weight = 1;// Mathf.Lerp(this.weight, 1, Time.deltaTime * 80.0f);
            this.anim.SetLayerWeight(2, weight);
            this.anim.SetLayerWeight(0, 0);
            return true;
        }
        else
        {
            this.weight = 0;// Mathf.Lerp(this.weight, 0, Time.deltaTime * 80.0f);
            this.anim.SetLayerWeight(0, 1);
            this.anim.SetLayerWeight(2, this.weight);
            return false;
        }
       

    }
}
