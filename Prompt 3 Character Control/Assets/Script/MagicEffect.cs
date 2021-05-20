using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicEffect : MonoBehaviour
{
    


    public GameObject dirFire;
    public GameObject[] firePoints;
    public GameObject fireModel;
    public int indexFire = 0;
    public List<GameObject> fireballs;



    public GameObject stonePoint;
    public GameObject stoneModel;


    public BoxCollider attackObj1;
    public BoxCollider attackObj2Left;
    public BoxCollider attackObj2Right;
    public BoxCollider attackObj3;
    private bool isAttack1 = false;
    private bool isAttack2Left = false;
    private bool isAttack2Right = false;
    private bool isAttack3 = false;
    private void CreateFireBall()
    {
        dirFire.transform.LookAt(this.gameObject.GetComponent<AIController>().player.transform.position);
        GameObject temp = Instantiate(this.fireModel, this.firePoints[indexFire].transform);
        temp.SetActive(true);
        temp.transform.localPosition = new Vector3(0, 0, 0);
        temp.transform.localEulerAngles = new Vector3(0, 0, 0);
        temp.transform.forward = this.firePoints[indexFire].transform.forward;
        temp.transform.parent = null;
        fireballs.Add(temp);
        indexFire++;
        if(firePoints.Length == indexFire)
        {
            indexFire = 0;
        }
    }

    private void ShootFireBalls()
    {
        for(int i = 0; i < this.fireballs.Count; i++)
        {
            this.fireballs[i].GetComponent<Bullet>().isFire = true;
        }
        this.fireballs = new List<GameObject>();
    }
    private void ThrowStone()
    {
        GameObject temp = Instantiate(this.stoneModel, this.stonePoint.transform);
        temp.SetActive(true);
        temp.transform.localPosition = new Vector3(0, 0, 0);
        temp.transform.localEulerAngles = new Vector3(0, 0, 0);
        temp.transform.parent = null;
        temp.transform.localScale = new Vector3(8, 8, 8);
        temp.GetComponent<Bullet>().isFire = true;
    }



    private void AttackA()
    {
        this.attackObj1.enabled = (true);
        this.isAttack1 = true;
    }

    private void AttackBLeft()
    {
        this.isAttack2Left = true;
        this.attackObj2Left.enabled = true;
    }
    private void AttackBRight()
    {
        this.isAttack2Right = true;
        this.attackObj2Right.enabled = true;
    }
    private void AttackC()
    {
        this.isAttack3 = true;
        this.attackObj3.enabled = true;
    }

    private void ResetAttackTrigger()
    {
        this.isAttack1 = false;
        this.isAttack2Left = false;
        this.isAttack2Right = false;
        this.isAttack3 = false;
        this.attackObj1.enabled = false;
        this.attackObj2Left.enabled = false;
        this.attackObj2Right.enabled = false;
        this.attackObj3.enabled = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerController>() != null && !other.isTrigger)
        {
            Debug.LogError("进入攻击区");
            if (this.isAttack1)
            {
                Debug.LogError("打中了！！！！！！！！！！！！！！！！！！！！！！");
                if (other.gameObject.GetComponent<PlayerController>().isDefend)
                    other.gameObject.GetComponent<PlayerController>().healthPoint -= 2.0f;
                else
                    other.gameObject.GetComponent<PlayerController>().healthPoint -= 10.0f;
                other.gameObject.GetComponent<PlayerController>().Hit();

                ResetAttackTrigger();
            }
            else if (this.isAttack2Left|| this.isAttack2Right)
            {
                Debug.LogError("打中了！！！！！！！！！！！！！！！！！！！！！！");
                if (other.gameObject.GetComponent<PlayerController>().isDefend)
                    other.gameObject.GetComponent<PlayerController>().healthPoint -= 5.0f;
                else
                    other.gameObject.GetComponent<PlayerController>().healthPoint -= 20.0f;
                other.gameObject.GetComponent<PlayerController>().Hit();
                ResetAttackTrigger();
            }
            else if (this.isAttack3)
            {
                Debug.LogError("打中了！！！！！！！！！！！！！！！！！！！！！！");
                other.gameObject.GetComponent<PlayerController>().healthPoint -= 40.0f;
                other.gameObject.GetComponent<PlayerController>().HitFly();
                ResetAttackTrigger();
            }
        }
    }
}
