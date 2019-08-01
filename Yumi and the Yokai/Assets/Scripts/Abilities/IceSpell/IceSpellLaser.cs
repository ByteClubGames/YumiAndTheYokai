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


    GameObject Yumi;

    void Start()
    {
        Yumi = GameObject.Find("Yumi");
    }


    void Update()
    {
        if (!isLaser)                                                        //isLaser prevents shooting lasers while there is already a laser bouncing around
        {

            if (Input.GetMouseButtonDown(0))                                 //when the shoot button is pressed
            {

                chargeTimer.Start();                                         //start the charging timer
            }

            if (Input.GetMouseButtonUp(0))                                   //when the shoot button is released
            {
                if ((chargeTimer.ElapsedMilliseconds) < minChargeTime)       //if the time charged is very short (quick tap)
                {
                    ShootLaser(0);                                         //shoot the minimum laser length (0)
                }

                else if ((chargeTimer.ElapsedMilliseconds) > maxChargeTime)   //if charged for the maximum time
                {
                    ShootLaser(maxChargeTime);                      //shoot max laser length
                }

                else                                                           //if charge time is somewhere inbetween minimum and maximum
                {
                    ShootLaser(chargeTimer.ElapsedMilliseconds);            //shoot with the laser length based on the charge time. (shoot laser takes in milliseconds
                }


                chargeTimer.Reset();                            //reset timers
                chargeTimer.Stop();
            }
        }
       
    }

    void ShootLaser(float chargeTimer)                                //shooting of the laser based on the milliseconds held down
    {
        isLaser = true;                                              //isLaser is true meaning that there is currently a laser out right now and can not shoot another one until this is false
        float scaleLaser = chargeTimer / maxChargeTime;             //the length of the laser is changed based on how much it is charged with scaleLaser

        float curLaserDistance = minLaserLength + (maxLaserLength * scaleLaser);

        //Yumi.GetComponent<YumiManaSystem>().SpellIsActive();                          //Hunter's mana system is used to deplete mana

        Vector3 laserOrigin = GameObject.Find("SpellCaster").transform.position;        
        Vector3 laserDirection = GetSpellClickTarget();

        Vector3[] points = new Vector3[20];                             //a vector 3 of "points" are used to set waypoints for the laser to move between. points are either bounce points or the end point of the laser
        int bounces = 0;


        RaycastHit hit;

        while (curLaserDistance > 0)            //while there is still laser length left, the laser will continue to bounce
        {
            UnityEngine.Debug.DrawRay(laserOrigin, laserDirection.normalized * curLaserDistance, Color.cyan);

            //fire laser with laser origin and curLaserDistance and laserDirection
            if (Physics.Raycast(laserOrigin, laserDirection.normalized * curLaserDistance, out hit, curLaserDistance, laserMask))      
            {
                //if laser hits, see what laser hits
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Platforms"))
                {
                    //print("hit platform");

                    curLaserDistance = curLaserDistance - hit.distance;              //keep track of how much more laser range is left
                    laserOrigin = hit.point;                                          //save the point of hit for the next reflection raycast
                    laserDirection = Vector3.Reflect(laserDirection, hit.normal);       //save the new laser direction

                    points[bounces] = laserOrigin;

                    //bounces++;
                }

                else if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Platforms"))          //if the laser hits a non-reflective surface or enemy, stop the laser from continuing
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

    void debugPrintPoints(Vector3[] points, int bounces)        //for testing purposes, okay to delete
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


    IEnumerator MoveLaser(Vector3[] points, int bounces)        //coroutine to move the laser object between the array of Vector3's "points" created in ShootLaser()
    {

        laserProjectile = Instantiate(laserProjectilePrefab, GameObject.Find("SpellCaster").transform.position, Quaternion.identity);  


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


        laserProjectile.transform.FindChild("TrailSpawner").parent = null;

        Destroy(laserProjectile);

        isLaser = false;            //allow a new laser to be shot when this one is deleted
    }
}
