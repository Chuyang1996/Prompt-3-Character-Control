using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AIController : MonoBehaviour
{
    //public Button BTN;
    public float speed;
    public float moveSpeed;
    [Range(1, 3)]
    public float minDis;
    [Range(3, 9)]
    public float battleDis;
    [Range(9, 40)]
    public float detectDis;
    [Range(1, 9)]
    public float confrontTime;

    private float confrontTimeCount;
    private string attackName;
    private float xDir;
    private float yDir;
    private bool isAttack;

    public GameObject player;
    public Animator anim;
    public NavMeshAgent nav;

    private AnimatorStateInfo animStateInfoZero;
    private delegate void AttackEvent();
    private List<AttackEvent> attacks = new List<AttackEvent>();

    // Start is called before the first frame update
    void Start()
    {
        this.ResetData();
        this.isAttack = false;
        
        this.attacks.Add(()=>{Attack("Attack1"); });
        this.attacks.Add(()=>{Attack("Attack2"); });
        this.attacks.Add(()=>{Attack("Attack3"); });

        
    }

    // Update is called once per frame
    void Update()
    {

        Debug.LogWarning(Vector3.Distance(this.player.transform.position, this.transform.position));
        if (!this.Portal())
        {
            if (this.PursuitPlayer())
            {
                if (!this.ConfrontPlayer())
                {
                    if (this.RandomAttack())
                    {
                        this.Back();
                    }
                }
            }
        }

    }

    private bool Portal()
    {
        this.animStateInfoZero = this.anim.GetCurrentAnimatorStateInfo(0);
        if (this.animStateInfoZero.IsName("Back"))
        {
            this.ResetData();
        }
        if (Vector3.Distance(this.transform.position, this.player.transform.position) > this.detectDis)
        {
            Debug.Log("Portal");
            return true;

        }
        return false;

    }
    private bool PursuitPlayer()
    {
        if (Vector3.Distance(this.transform.position, this.player.transform.position) > this.battleDis)
        {
            this.confrontTimeCount = 0.0f;
            this.nav.Resume();
            this.nav.speed = this.moveSpeed;
            this.nav.SetDestination(player.transform.position);
            if (this.yDir < 0.0f)
                this.yDir = 0.0f;
            this.yDir += Time.deltaTime *this.speed;
            this.xDir -= Time.deltaTime * this.speed;
            this.xDir = this.xDir < 0.0f ? 0.0f : this.xDir;
            this.yDir = this.yDir > 1.0f ? 1.0f : this.yDir;
            this.anim.SetFloat("Yspeed", this.yDir);
            this.anim.SetFloat("Xspeed", this.xDir);
            Debug.Log("Pursuit");
            
            return false;

        }
        else if (Vector3.Distance(this.transform.position, this.player.transform.position) < this.minDis)
        {
            this.ResetData();
            if (this.animStateInfoZero.IsName("NormalState")/* && this.animStateInfoZero.normalizedTime < 1.0f*/)
            {
                this.isAttack = false;
            }
            this.Back();
            return false;
        }
        return true;
    }

    private bool ConfrontPlayer()
    {
        this.nav.speed = 0.0f;
        //this.nav.Stop();
        this.animStateInfoZero = this.anim.GetCurrentAnimatorStateInfo(0);
        if (!this.isAttack && this.animStateInfoZero.IsName("NormalState"))
        {
            this.yDir -= Time.deltaTime * this.speed;
            this.xDir += Time.deltaTime * this.speed;
            this.yDir = this.yDir < 0.0f ? 0.0f : this.yDir;
            this.xDir = this.xDir > 1.0f ? 1.0f : this.xDir;
            this.anim.SetFloat("Yspeed", this.yDir);
            this.anim.SetFloat("Xspeed", this.xDir);
            Vector3 temp = this.player.transform.position - this.gameObject.transform.position;
            this.transform.forward = new Vector3(temp.x, this.transform.forward.y, temp.z);
            this.transform.RotateAround(this.player.transform.position, Vector3.up, -this.speed * 10.0f * Time.deltaTime);
        }
        if (this.confrontTimeCount <= this.confrontTime)
        {
            Debug.Log("Battle");
            this.confrontTimeCount += Time.deltaTime;
            if (this.animStateInfoZero.IsName("NormalState")/* && this.animStateInfoZero.normalizedTime < 1.0f*/)
            {
                this.anim.ResetTrigger("Attack1");
                this.anim.ResetTrigger("Attack2");
                this.anim.ResetTrigger("Attack3");
                this.isAttack = false;
            }
            return true;
        }
        else
        {
            return false;
        }

    }


    private bool RandomAttack()
    {
        //this.ResetData();
        
        if (!this.animStateInfoZero.IsName("NormalState")/* && this.animStateInfoZero.normalizedTime < 1.0f*/)
        {
            this.ResetData();
            this.isAttack = false;
            return false;
        }
        else
        {
            if (this.animStateInfoZero.IsName("NormalState") && !this.isAttack)
            {
                int index = Random.Range(0, 2);
                this.confrontTimeCount = 2.0f;//Random.Range(0, this.confrontTime);
                this.attacks[index].Invoke();
                Debug.Log("index: " + index);
                Debug.Log("confrontTimeCount: " + this.confrontTimeCount);
                this.isAttack = true;
            }
            return false;
        }
    
    }

    private void Attack(string name)
    {
        this.anim.SetTrigger(name);
        this.animStateInfoZero = this.anim.GetCurrentAnimatorStateInfo(0);
        this.attackName = name;


    }


    private void Back()
    {
        this.anim.SetTrigger("Back");
        //this.yDir -= Time.deltaTime * this.speed;
        //this.xDir -= Time.deltaTime * this.speed;
        //this.yDir = this.yDir < -1.0f ? -1.0f : this.yDir;
        //this.xDir = this.xDir < 0.0f ? 0.0f : this.xDir;
        //this.anim.SetFloat("Yspeed", this.yDir);
        //this.anim.SetFloat("Xspeed", this.xDir);
        //this.transform.Translate(0, 0, -10*Time.deltaTime);
        //if(!this.animStateInfoZero.IsName("NormalState"))
        //    this.anim.SetTrigger("Back");

    }


    private void ResetData()
    {
        this.anim.ResetTrigger("Attack1");
        this.anim.ResetTrigger("Attack2");
        this.anim.ResetTrigger("Attack3");
        this.anim.ResetTrigger("Back");
        //this.xDir = 0.0f;
        //this.yDir = 0.0f;
        //this.anim.SetFloat("Yspeed", 0);
        //this.anim.SetFloat("Xspeed", 0);



    }


}
