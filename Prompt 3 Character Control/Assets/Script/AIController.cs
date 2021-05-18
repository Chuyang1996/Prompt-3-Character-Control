using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AIController : MonoBehaviour
{
    [Range(0, 1000)]
    public float healthPoint;

    //public Button BTN;
    public float speed;
    public float moveSpeed;
    [Range(1, 6)]
    public float minDis;
    [Range(6, 9)]
    public float closeDis;
    [Range(13, 15)]
    public float confrontDis;
    [Range(20, 40)]
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
    private List<AttackEvent> minAction = new List<AttackEvent>();
    private List<AttackEvent> closeAction = new List<AttackEvent>();
    private List<AttackEvent> farAction = new List<AttackEvent>();

    // Start is called before the first frame update
    void Start()
    {
        this.ResetData();
        this.isAttack = false;
        
        this.minAction.Add(()=>{Attack("Attack1"); });
        this.minAction.Add(()=>{ Back(); });

        this.closeAction.Add(() => { Attack("Attack2"); });
        this.closeAction.Add(() => { Attack("Attack3"); });
        this.closeAction.Add(() => { Back(); });

        this.farAction.Add(() => { Attack("Attack4"); });
        this.farAction.Add(() => { Attack("Attack5"); });


    }

    // Update is called once per frame
    void Update()
    {
        this.anim.SetFloat("Health", this.healthPoint);
        Debug.LogWarning(this.DistanceForTarget());
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
        if (this.DistanceForTarget() > this.detectDis)
        {
            Debug.Log("Portal");
            return true;

        }
        return false;

    }
    private bool PursuitPlayer()
    {
        if (this.DistanceForTarget() > this.confrontDis)
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
        //else if (Vector3.Distance(this.transform.position, this.player.transform.position) < this.minDis)
        //{
        //    this.ResetData();
        //    if (this.animStateInfoZero.IsName("NormalState")/* && this.animStateInfoZero.normalizedTime < 1.0f*/)
        //    {
        //        this.isAttack = false;
        //    }
        //    this.Back();
        //    return false;
        //}
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
                if (this.DistanceForTarget() < this.minDis)
                {
                    int index = Random.Range(0, 11);
                    this.confrontTimeCount = 2.0f;
                    if (0 <= index && index <= 6)
                    {
                        this.minAction[0].Invoke();
                    }
                    else
                    {
                        this.minAction[1].Invoke();

                    }
                }
                else if (this.DistanceForTarget() < this.closeDis)
                {
                    int index = Random.Range(0, 11);
                    this.confrontTimeCount = 2.0f;
                    if (0 <= index && index <= 3)
                    {
                        this.closeAction[0].Invoke();
                    }
                    else if (4 <= index && index <= 7)
                    {
                        this.closeAction[1].Invoke();
                    }
                    else
                    {
                        this.closeAction[2].Invoke();

                    }
                }
                else
                {
                    int index = Random.Range(0, 2);
                    this.confrontTimeCount = 0.0f;
                    this.farAction[index].Invoke();
                }
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
        this.ResetData();
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
        this.anim.ResetTrigger("Attack4");
        this.anim.ResetTrigger("Attack5");
        this.anim.ResetTrigger("Back");
        //this.xDir = 0.0f;
        //this.yDir = 0.0f;
        //this.anim.SetFloat("Yspeed", 0);
        //this.anim.SetFloat("Xspeed", 0);



    }

    private float DistanceForTarget()
    {
        return Vector2.Distance(new Vector2(this.transform.position.x, this.transform.position.z), new Vector2(this.player.transform.position.x, this.player.transform.position.z));
    }

}
