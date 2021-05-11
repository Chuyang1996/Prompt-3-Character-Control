using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AIController : MonoBehaviour
{
    public Button BTN;
    public float speed;
    [Range(3, 5)]
    public float battleDis;
    [Range(5, 10)]
    public float detectDis;
    [Range(4, 9)]
    public float confrontTime;

    private float confrontTimeCount;
    private string attackName;


    public GameObject player;
    public Animator anim;
    public NavMeshAgent nav;

    private AnimatorStateInfo animStateInfoZero;
    private delegate void AttackEvent();
    private List<AttackEvent> attacks = new List<AttackEvent>();

    // Start is called before the first frame update
    void Start()
    {
        this.nav.speed = 1.0f;
        this.attacks.Add(()=>{Attack("Attack1"); });
        this.attacks.Add(()=>{Attack("Attack2"); });
        this.attacks.Add(()=>{Attack("Attack3"); });

        
    }

    // Update is called once per frame
    void Update()
    {
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


    private bool PursuitPlayer()
    {
        if (Vector3.Distance(this.transform.position, this.player.transform.position) > this.battleDis)
        {
            this.nav.SetDestination(player.transform.position);
            Debug.Log("Pursuit");
            return false;

        }
        return true;
    }

    private bool ConfrontPlayer()
    {
        if(this.confrontTimeCount < this.confrontTime)
        {
            Debug.Log("Battle");
            this.confrontTimeCount += Time.deltaTime;
            return true;
        }
        else
        {
            return false;
        }

    }

    private bool Portal()
    {
        if (Vector3.Distance(this.transform.position, this.player.transform.position) > this.detectDis)
        {
            Debug.Log("Portal");
            return true;

        }
        return false;

    }
    private bool RandomAttack()
    {
        this.animStateInfoZero = this.anim.GetCurrentAnimatorStateInfo(0);
        if (this.animStateInfoZero.IsName(this.attackName) && this.animStateInfoZero.normalizedTime < 1.0f)
        {
            this.anim.ResetTrigger(this.attackName);
            return false;
        }
        else if (this.animStateInfoZero.IsName(this.attackName) && this.animStateInfoZero.normalizedTime > 1.0f)
        {
            return true;
        }
        else
        {
            int index = Random.Range(0, 2);
            this.attacks[index].Invoke();
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
        if(this.animStateInfoZero.IsName("Attack1")|| this.animStateInfoZero.IsName("Attack2")|| this.animStateInfoZero.IsName("Attack3"))
            this.anim.SetTrigger("Back");
        if (this.animStateInfoZero.IsName("Back") && this.animStateInfoZero.normalizedTime < 1.0f)
        {
            this.anim.ResetTrigger("Back");
        }
        else
        {
            this.confrontTimeCount = 0.0f;
        }
    }



}
