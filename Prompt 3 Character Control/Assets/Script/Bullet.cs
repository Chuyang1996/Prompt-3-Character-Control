using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    Straight = 0,
    Cursor = 1,
}
public class Bullet : MonoBehaviour
{
    [Range(20,40)]
    public int bulletHurt = 40;

    public float straightSpeed = 2.0f;
    public float detectDis = 2.0f;
    [HideInInspector]
    public bool isFire = false;



    public Transform target_trans;
    // 运动速度
    public float speed = 10;
    // 最小接近距离, 以停止运动
    public float min_distance = 0.5f;
    private float distanceToTarget;
    private bool move_flag = true;
    private bool isThrow = false;
    private Transform m_trans;


    public BulletType bulletType;
    private void Update()
    {
        if (!isFire)
            return;
        if(bulletType == BulletType.Straight)
        {
            this.StraightShoot();
        }else if(bulletType == BulletType.Cursor)
        {
            
            this.Cursor();
        }

    }
    void StraightShoot()
    {
        this.transform.Translate(new Vector3(0, 0, 1) * this.straightSpeed * Time.deltaTime);

    }

    void Cursor()
    {
        if (!this.isThrow)
        {
            m_trans = this.transform;
            distanceToTarget = Vector3.Distance(m_trans.position, target_trans.position);
            this.isThrow = true;
            StartCoroutine(Parabola(target_trans.position));
        }
    }



    IEnumerator Parabola(Vector3 pos)
    {
        while (move_flag)
        {
            Vector3 targetPos = pos;
            // 朝向目标, 以计算运动
            m_trans.LookAt(targetPos);
            // 根据距离衰减 角度
            float angle = Mathf.Min(1, Vector3.Distance(m_trans.position, targetPos) / distanceToTarget) * 45;
            // 旋转对应的角度（线性插值一定角度，然后每帧绕X轴旋转）
            m_trans.rotation = m_trans.rotation * Quaternion.Euler(Mathf.Clamp(-angle, -42, 42), 0, 0);
            // 当前距离目标点
            float currentDist = Vector3.Distance(m_trans.position, pos);
            // 很接近目标了, 准备结束循环
            if (currentDist < min_distance)
            {
                move_flag = false;
            }
            // 平移 (朝向Z轴移动)
            m_trans.Translate(Vector3.forward * Mathf.Min(speed * Time.deltaTime, currentDist));
            // 暂停执行, 等待下一帧再执行while
            yield return null;
        }
        if (move_flag == false)
        {
            // 使自己的位置, 跟[目标点]重合
            m_trans.position = pos;
            // [停止]当前协程任务,参数是协程方法名
            StopCoroutine(Parabola(pos));
            // 销毁脚本
            AudioManager.Instance.PlayAudio(this.transform, "ThrowExplosion");
            GameObject.Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag =="Floor" && this.bulletType == BulletType.Straight)
        {
            AudioManager.Instance.PlayAudio(this.transform, "FireExplosion");
            DestroyImmediate(this.gameObject);
        }
        if (other.GetComponent<PlayerController>() != null)
        {
            other.GetComponent<PlayerController>().HitFly();
            other.GetComponent<PlayerController>().healthPoint-=this.bulletHurt;
        }
    }

}
