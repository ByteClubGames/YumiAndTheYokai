/*
***************************************************************************************
*Creator(s).........................................Karim Dabboussi & Keiran Glynn
*Created..............................................................3/17/2018
*Last Modified............................................@ 11:46 PM on 2/9/2019
*Last Modified by...................................................Karim Dabboussi
*
*Description:  controls the projectile.
*
*               
***************************************************************************************
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform target;
    public float speed = 15f;
    public Vector3 targetPos = Vector3.zero;

    public void getTargetPosition()
    {
        targetPos = target.position;
    }

    public void Seek(Transform _target)
    {
        target = _target;
        StartCoroutine(WaitToReturn(5)); //this calls a method that deletes the object after 5 seconds
        //can pass on damage amount
    }

    // Update is called once per frame
    void Update()
    {
        if (targetPos == Vector3.zero)
        {
            getTargetPosition();
        }

        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = targetPos - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;
        if (dir.magnitude <= distanceThisFrame)
        {
            //if target gets hit
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }
    void HitTarget()
    {
        Debug.Log("Target Hit");
        Destroy(gameObject);
        //HUNTER ADD HEALTH DAMAGE FUNCTION CALL HERE
    }

    private IEnumerator WaitToReturn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime); // after a set amount of time the object will be deleted.
        Destroy(gameObject);
    }
}
