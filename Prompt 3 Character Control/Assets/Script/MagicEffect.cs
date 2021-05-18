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
}
