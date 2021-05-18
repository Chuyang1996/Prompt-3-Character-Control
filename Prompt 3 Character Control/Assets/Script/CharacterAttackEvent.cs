using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttackEvent : MonoBehaviour
{
    [Range(0,1000)]
    public float attackApoint = 10;
    [Range(0, 1000)]
    public float attackBpoint = 20;
    [Range(0, 1000)]
    public float attackCpoint = 30;

    public BoxCollider attackAObj;
    public BoxCollider attackBObj;
    public BoxCollider attackCObj;

    private bool isAttackA;
    private bool isAttackB;
    private bool isAttackC;
    public void AttackA()
    {
        this.isAttackA = true;
        this.attackAObj.enabled = true;
    }
    public void AttackB()
    {
        this.isAttackB = true;
        this.attackBObj.enabled = true;
    }
    public void AttackC()
    {
        this.isAttackC = true;
        this.attackCObj.enabled = true;
    }
    public void ResetAttack()
    {
        this.isAttackA = false;
        this.isAttackB = false;
        this.isAttackC = false;
        this.attackAObj.enabled = false;
        this.attackBObj.enabled = false;
        this.attackCObj.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<AIController>() != null && !other.isTrigger)
        {
            Debug.LogError("进入攻击区");
            if (this.isAttackA)
            {
                Debug.LogError("打中了！！！！！！！！！！！！！！！！！！！！！！");
                other.gameObject.GetComponent<AIController>().healthPoint -= this.attackApoint;
                //other.gameObject.GetComponent<AIController>().Hit();

                ResetAttack();
            }
            else if (this.isAttackB)
            {
                Debug.LogError("打中了！！！！！！！！！！！！！！！！！！！！！！");
                other.gameObject.GetComponent<AIController>().healthPoint -= this.attackBpoint;
                //other.gameObject.GetComponent<AIController>().Hit();
                ResetAttack();
            }
            else if (this.isAttackC)
            {
                Debug.LogError("打中了！！！！！！！！！！！！！！！！！！！！！！");
                other.gameObject.GetComponent<AIController>().healthPoint -= this.attackCpoint;
                //other.gameObject.GetComponent<AIController>().HitFly();
                ResetAttack();
            }
        }
    }
}
