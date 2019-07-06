/* IceSpellLaser.cs
********************************************************************************
*Creator(s).........................................................Darrell Wong
*Created................................................................5/3/2019
*Last Modified...............................................@3:00PM on 5/3/2019
*Last Modified by...................................................Darrell Wong
*
*Description:   Handles behavior for the IceSpell Laser.
*      
*               This script should be attached to the IceSpellSpawner object.
*                   
*                   
*                   playerdetection on pig bomb and laser objects are temporarily given "player" layer, please change before pushing
*                   
********************************************************************************
*/

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;



public class IceSpellLaser : MonoBehaviour
{

    public float minLaserLength;

    [Tooltip("Laser length added to minLaserLength at full charge")]
    public float maxLaserLength;

    [Tooltip("In milliseconds(divide by 1000 for seconds)")]
    public float minChargeTime;

    [Tooltip("In milliseconds(divide by 1000 for seconds)")]
    public float maxChargeTime;

    public float laserSpeed;

    public LayerMask laserMask;

    private Vector3 mousePosDirection;

    private Stopwatch chargeTimer = new Stopwatch();
    private Plane referencePlane = new Plane(new Vector3(0, 0, -1), new Vector3(0, 0, 0));

    public GameObject laserProjectilePrefab;
    private GameObject laserProjectile;
    private bool isLaser;
    public float pointRadius; //make this private maybe 

    void Start()
    {

    }


    void Update()
    {
        if (!isLaser)
        {
            if (Input.GetMouseButtonDown(0))
            {

                //if (chargeTimer.IsRunning)
                //{
                //    chargeTimer.Reset();
                //}

                chargeTimer.Start();

                //drain mana here
            }

            if (Input.GetMouseButtonUp(0))  //OR if run out of mana
            {
                if ((chargeTimer.ElapsedMilliseconds) < minChargeTime)
                {
                    ShootLaser(0);
                }

                else if ((chargeTimer.ElapsedMilliseconds) > maxChargeTime)
                {
                    ShootLaser(maxChargeTime);
                }

                else
                {
                    ShootLaser(chargeTimer.ElapsedMilliseconds);
                }


                chargeTimer.Reset();
                chargeTimer.Stop();
            }
        }
       
    }

    void ShootLaser(float chargeTimer)
    {
        float scaleLaser = chargeTimer / maxChargeTime;

        float curLaserDistance = minLaserLength + (maxLaserLength * scaleLaser);

        Vector3 laserOrigin = GameObject.Find("SpellCaster").transform.position;
        Vector3 laserDirection = GetSpellClickTarget();

        Vector3[] points = new Vector3[20];
        int bounces = 0;


        RaycastHit hit;

        while (curLaserDistance > 0)
        {
            UnityEngine.Debug.DrawRay(laserOrigin, laserDirection.normalized * curLaserDistance, Color.cyan);
            //fire laser with laser origin and curLaserDistance and laserDirection
            if (Physics.Raycast(laserOrigin, laserDirection.normalized * curLaserDistance, out hit, curLaserDistance, laserMask))      //add a layer mask if you really like layermasks
            {
                //if laser hits see what laser hits
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Platforms"))
                {
                    //print("hit platform");

                    curLaserDistance = curLaserDistance - hit.distance;              //keep track of how much more laser range is left
                    laserOrigin = hit.point;                                          //save the point of hit for the next reflection raycast
                    laserDirection = Vector3.Reflect(laserDirection, hit.normal);       //save the new laser direction

                    points[bounces] = laserOrigin;

                    //bounces++;
                }

                else if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Platforms"))          //////WAS WORKING ON MAKING PLAYER DETECTION LAYER MASK
                {
                    //print("hit non reflective surface/nothing");
                    points[bounces] = hit.point;
                    curLaserDistance = -1;
                    //bounces++;
                }

            }
            else
            {
                //print("hit nothing");
                points[bounces] = laserOrigin + laserDirection.normalized * curLaserDistance;        //set the last length of the laser as the last waypoint
                curLaserDistance = -1;
                //bounces++;
            }
            bounces++;
        }

        //laserProjectile = Instantiate(laserProjectilePrefab, GameObject.Find("SpellCaster").transform.position, Quaternion.identity);
        StartCoroutine(MoveLaser(points, bounces));
        //debugPrintPoints(points, bounces);
    }



    public Vector3 GetSpellClickTarget()  //taken from Michael's "IceProjectile.cs" legacy script
    {
        // take mouse input as Vector3 coord. in the far and near plane
        Vector3 mousePosFar = new Vector3(Input.mousePosition.x,
                                          Input.mousePosition.y,
                                          Camera.main.farClipPlane);
        Vector3 mousePosNear = new Vector3(Input.mousePosition.x,
                                           Input.mousePosition.y,
                                           Camera.main.nearClipPlane);
        Vector3 mousePosF = Camera.main.ScreenToWorldPoint(mousePosFar);
        Vector3 mousePosN = Camera.main.ScreenToWorldPoint(mousePosNear);

        // shoot a ray from nearClipPlane -> farClipPlane
        Ray screenPoint = new Ray(mousePosN, mousePosF - mousePosN);

        // get worldCursorPosition/vector3 of spell click target on reference plane
        float distanceCamToPlane = 0f;
        referencePlane.Raycast(screenPoint, out distanceCamToPlane);
        Vector3 worldCursorPosition = screenPoint.GetPoint(distanceCamToPlane);

        // final spell path direction = projected vector from (mousePosF - mousePosN) onto reference plane
        worldCursorPosition = Vector3.ProjectOnPlane(worldCursorPosition, new Vector3(0, 0, 0));

        // get SpellCaster current position
        Vector3 castPos = GameObject.Find("SpellCaster").transform.position;

        // unity debug rays. blue: yumi -> spell click point, red: yumi -> (mousePosF - mousePosN)
        //UnityEngine.Debug.DrawRay(castPos, worldCursorPosition - castPos, Color.blue);
        //UnityEngine.Debug.DrawRay(mousePosN, (mousePosF - mousePosN) * 100, Color.red);

        // fix z-coordinate
        Vector3 direction = worldCursorPosition - castPos;
        direction.z = 0;

        // final spellClickTarget is (world position - yumi's position)

        return direction;
    }

    void debugPrintPoints(Vector3[] points, int bounces)
    {
        //print("bounces: " + bounces);

        //for (int bounceIterator = 0; bounceIterator < bounces; bounceIterator++)
        //{
        //    //print(bounceIterator + " " + points[bounceIterator]);
        //    laserProjectile = Instantiate(laserProjectilePrefab, points[bounceIterator], Quaternion.identity);
        //}

        int bounceIterator = 0;
        while (bounceIterator < bounces)
        {
            laserProjectile = Instantiate(laserProjectilePrefab, points[bounceIterator], Quaternion.identity);
            bounceIterator++;
        }
    }

    IEnumerator MoveLaser(Vector3[] points, int bounces)
    {
        isLaser = true;

        laserProjectile = Instantiate(laserProjectilePrefab, GameObject.Find("SpellCaster").transform.position, Quaternion.identity);   ////////THE PROJECTILE IS NOT MOVING FIXXXXXXXXXXXX


        int bounceIterator = 0;


        while (bounceIterator < bounces)
        {
            laserProjectile.transform.position = Vector3.MoveTowards(laserProjectile.transform.position, points[bounceIterator], laserSpeed * Time.deltaTime);
            //laserProjectile.transform.position = Vector3.Lerp(laserProjectile.transform.position, points[bounceIterator], laserSpeed * Time.deltaTime);
            //yield return new WaitForSeconds(.5f);
            yield return null;
            if (Vector3.Distance(points[bounceIterator], laserProjectile.transform.position) < pointRadius)
            {
                bounceIterator++;

            }  
        }

        Destroy(laserProjectile);

        isLaser = false;
    }
}
