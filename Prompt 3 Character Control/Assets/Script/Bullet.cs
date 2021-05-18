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
    public float straightSpeed = 2.0f;
    public float detectDis = 2.0f;
    [HideInInspector]
    public bool isFire = false;



    public Transform target_trans;
    // �˶��ٶ�
    public float speed = 10;
    // ��С�ӽ�����, ��ֹͣ�˶�
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
        Collider[] colliderObject = Physics.OverlapSphere(this.transform.position, this.detectDis);

        for (int i = 0; i < colliderObject.Length; i++)
        {
            Debug.Log("''''''''''''''''''''" + colliderObject[i].gameObject.name);
            if (colliderObject[i].gameObject.tag == "Floor")
            {
                DestroyImmediate(this.gameObject);
            }
        }
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
            // ����Ŀ��, �Լ����˶�
            m_trans.LookAt(targetPos);
            // ���ݾ���˥�� �Ƕ�
            float angle = Mathf.Min(1, Vector3.Distance(m_trans.position, targetPos) / distanceToTarget) * 45;
            // ��ת��Ӧ�ĽǶȣ����Բ�ֵһ���Ƕȣ�Ȼ��ÿ֡��X����ת��
            m_trans.rotation = m_trans.rotation * Quaternion.Euler(Mathf.Clamp(-angle, -42, 42), 0, 0);
            // ��ǰ����Ŀ���
            float currentDist = Vector3.Distance(m_trans.position, pos);
            // �ܽӽ�Ŀ����, ׼������ѭ��
            if (currentDist < min_distance)
            {
                move_flag = false;
            }
            // ƽ�� (����Z���ƶ�)
            m_trans.Translate(Vector3.forward * Mathf.Min(speed * Time.deltaTime, currentDist));
            // ��ִͣ��, �ȴ���һ֡��ִ��while
            yield return null;
        }
        if (move_flag == false)
        {
            // ʹ�Լ���λ��, ��[Ŀ���]�غ�
            m_trans.position = pos;
            // [ֹͣ]��ǰЭ������,������Э�̷�����
            StopCoroutine(Parabola(pos));
            // ���ٽű�
            GameObject.Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            other.GetComponent<PlayerController>().HitFly();
        }
    }

}
