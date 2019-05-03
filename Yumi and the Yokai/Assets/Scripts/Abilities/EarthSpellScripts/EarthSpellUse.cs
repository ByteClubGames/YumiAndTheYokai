/*
* ************************************************************************************************
* Creator(s)...........................................................Daniel Jaffe & Darrell Wong
* Created...............................................................................03/15/2018
* Last Modified............................................................@ 11:33PM on 04/19/2018
* Last Modified by....................................................................Daniel Jaffe
* 
* 
* Description:   Spawn in the earthSpell object - Attach to the Earth Spell Spawner object:
*                   1. Uses a OverlapSphere to check eSpell overlap
*                   2. Spawns a eSpell object at the point of click oriented normal to the surface
* ************************************************************************************************
*/

using UnityEngine;

public class EarthSpellUse : MonoBehaviour
{
    #region Global Variables
    private Plane zPlane = new Plane(new Vector3(0, 0, -1), new Vector3(0, 0, 0)); //plane to see where ray cast from camera hits
    public float overlapRadius = 0.65F; // shpere to cover 1,1,1, cube
    public bool spellOverlap = false;
    public GameObject eSpell;
    public Vector3 playerinput = new Vector3(1, 1, 0); //gets player input
    public Vector3 normalVector;
    public float scalar;
    private LayerMask layerMask;
    #endregion

    // Update is called once per frame
    void Update()
    {
        //execute on mouse click
        if (Input.GetMouseButtonDown(0))
        {
            ClickLocation();
            CheckOverlap();

        }
    }

    //Struct that holds a Vector and a RaycastHit
    public struct TargetVector
    {
        Ray ray;
        Raycast hit;

        public TargetVector(Ray ray, Raycast hit)
        {
            this.ray = ray;
            this.hit = hit;
        }
    }

    public TargetVector[] ClickLocation()
    {
        //raycast from the camera and the player - if both rays hit the same object with the tag EarthSpellSurface, then spawn eSpell
        //***Should eventually be moved into Spell Casting script as this is used with the Ice Spell as well and a similar version for wind***

        //Ray from camera to world position
        Vector3 mousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
        Vector3 mousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
        Vector3 mousePosF = Camera.main.ScreenToWorldPoint(mousePosFar);
        Vector3 mousePosN = Camera.main.ScreenToWorldPoint(mousePosNear);
        Ray camRay = new Ray(mousePosN, mousePosF - mousePosN); //this is the ray shot from the camera to to world position clicked at
        Debug.DrawRay(mousePosN, mousePosF - mousePosN, Color.green); //this is a visual representation of that camera ray for debugging purposes 
        RaycastHit camHit; //our camRay hit variable

        //Ray from Yumi to the location the camera ray hits when z=0
        float distanceCamToPlane = 0f;
        zPlane.Raycast(camRay, out distanceCamToPlane);
        Vector3 worldCursorPosition = camRay.GetPoint(distanceCamToPlane);
        Vector3 yumiPosition = GameObject.Find("SpellCaster").transform.position;
        worldCursorPosition = Vector3.ProjectOnPlane(worldCursorPosition, new Vector3(0, 0, 0)); //FORGET WHAT THIS DOES
        Ray yumiRay = new Ray(yumiPosition, worldCursorPosition - yumiPosition); //this is the ray shot from yumi to the world position
        Debug.DrawRay(yumiPosition, worldCursorPosition - yumiPosition, Color.blue); //this is a visual representation of that yumi ray for debugging purposes 
        RaycastHit yumiHit; //our yumiRay hit variable

        //Creates and returns an array of the struct TargetVector which contains both the vector and its hit.
        TargetVecotor[] myTarget = new TargetVector[2];
        myTarget[0] = new TargetVector(camRay, camHit);
        myTarget[1] = new TargetVector(yumiRay, yumiHit);

        return myTarget;
    }

    void CheckOverlap()
    {
        if (Input.GetMouseButtonDown(0))
        {

            if (Physics.Raycast(camRay, out camHit) && Physics.Raycast(yumiRay, out yumiHit) && (yumiHit.collider.gameObject == camHit.collider.gameObject) && yumiHit.collider.CompareTag("EarthSpellSurface"))
            {
                //get the location of the player click
                Vector3 playerinput = new Vector3(yumiHit.point.x, yumiHit.point.y, 0);
                normalVector = yumiHit.normal;
                normalVector.z = 0;

                //Checking for overlapping eSpell spawns
                Collider[] hitColliders = Physics.OverlapSphere(playerinput, overlapRadius);
                spellOverlap = false;
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    if (hitColliders[i].tag == "_Earth Spell Object")
                    {
                        print("Overlap");
                        spellOverlap = true;
                        break;
                    }
                }

                //Assuming no overlap, spawns the eSpell object into play in the direction of the normal vector
                if (!spellOverlap)
                {
                    ScaleAndSpawn(yumiHit); //Function to spawn the earth spell and "grow" it to the correct length.
                }
            }
        }
    }


    //Function to spawn the pilar in and scale it correctly
    void ScaleAndSpawn(RaycastHit yumiHit)
    {
        int layerMask = 1 << 9; //turns the layermask on for the player layer

        //Set up of rays to check distance pillar should grow to as well as if the pillar hits the player
        Ray heightRay = new Ray(playerinput, 4 * yumiHit.normal);
        RaycastHit heightHit;

        //Scales the length the earth spell will spawn / stretch to based on collision with anything except the player layer. Scaler of 0 = max length while scaler of 1 = no growth.
        if (Physics.Raycast(heightRay, out heightHit, 4f, ~layerMask))
        {
            scalar = 1 - Mathf.Abs(heightHit.distance / 4);
        }
        else scalar = 0;

        var earthPillar = Instantiate(eSpell, playerinput, Quaternion.FromToRotation(Vector3.up, yumiHit.normal)); //Creates the earth spell
        earthPillar.GetComponentInChildren<EarthSpellMechanics>().Initialize(scalar); //Shrinks the earth spell max length by the scalar value * max length (This is instant and so it just makes the pillar grow to a shorter max length)

        //Check for collision with player and if so, call function to apply a velcity away
        if (Physics.Raycast(heightRay, out heightHit, 4f, layerMask))
        {
            ApplyVelocity(heightHit.transform.gameObject);
        }

    }



    void ApplyVelocity(GameObject player)
    {

    }

}


