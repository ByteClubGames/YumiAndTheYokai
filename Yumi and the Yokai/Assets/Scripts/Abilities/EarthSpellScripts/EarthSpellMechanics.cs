/*
 * ******************************************************************************************************
 * Creator(s).................................................................Daniel Jaffe & Darrell Wong
 * Created.....................................................................................03/15/2018
 * Last Modified..................................................................@ 10:59PM on 12/21/2018
 * Last Modified by..........................................................................Daniel Jaffe
 * 
 * 
 * Description:   Functionality of Earth Spell - Attach to the earthSpell object (cube):
 *                1. Earth spell is created from EarthSpellUse.cs where its properties are defined
 *                2. With line of sight, click will produce a pillar that grows normal to the surface
 *                3. Properties of growth speed/distance, maxSpells, and decay time are public variables
 *                4. Earth spell growth will stop on collision with specified objects with:
 *                   a)"Platforms" layer
 *                   b)"Earth" layer
 *                   c)"Earth Spell Object" tag
 *                5. EarthSpell starts with size 1,0,1. Grows to 1,1,1.
 * ******************************************************************************************************
 */

using System.Collections;
using UnityEngine;

public class EarthSpellMechanics : MonoBehaviour
{
    #region Global Variables
    private Vector2 firstPressPos;
    private Vector2 secondPressPos;
    private bool grow = false;
    private Collision col;
    private IEnumerator co;
    public int maxSpells = 3;
    public float extendFactor = 25f;
    public float extendSpeed = .1f;
    public int destroyTime = 5;
    bool firstCollision;
    #region LegacyCode
    //bool sizeNotSet = true;
    //private float deltaX = 0f, deltaY = 0f;
    #endregion
    #endregion

    private void Start()
    {
        firstCollision = false;
        firstPressPos = Input.mousePosition;
        co = Grow(extendSpeed);
        StartCoroutine(co);
        Destroy(transform.parent.gameObject, destroyTime); //Destroy timer starts on creation
    }

    private void Update()
    {

        if (Input.GetMouseButtonUp(0))
        {
            grow = true;
            #region LegacyCode
            //secondPressPos = Input.mousePosition;
            //calculate the difference between firstposition and second position
            //deltaX = secondPressPos.x - firstPressPos.x;
            //deltaY = secondPressPos.y - firstPressPos.y;
            //print("First: " + firstPressPos.x + " " + firstPressPos.y + "\n");
            //print("Second: " + secondPressPos.x + " " + secondPressPos.y + "\n");
            #endregion
        }

        //destroy the earliest spell when there are too many
        if (GameObject.FindGameObjectsWithTag("Earth Spell Object").Length > maxSpells)
        {
            Destroy(GameObject.FindGameObjectsWithTag("Earth Spell Object")[0]);
        }
    }

    //Called at start and is a slow progression loop
    private IEnumerator Grow(float extendSpeed)
    {
        //Waits until IsGrowing returns true;
        yield return new WaitUntil(IsGrowing);

        #region LegacyCode
        //if (sizeNotSet)
        //{
        //    gameObject.transform.localScale = new Vector3(1, 1, 0F);
        //    sizeNotSet = false;
        //}
        #endregion

        for (int i = 0; i < extendFactor; i++)
        {
            //stretches object in the Y (Z in terms of blender) direction
            gameObject.transform.localScale += new Vector3(0, 0, extendSpeed);
            //main_mat.mainTextureScale = new Vector2(original_scale.x, original_scale.y + extendSpeed);   //  <<----- HAVE KROSS LOOK AT THIS
            #region LegacyCode
            //moves the object along to accomidate for equal stretching on both sides
            //gameObject.transform.Translate(0, (extendSpeed / 3f), 0);
            #endregion
            yield return null;
        //yield return new WaitForSeconds(extendSpeed / 10);
        }
        #region LegacyCode
        //if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY))
        //{
        //    if (sizeNotSet)
        //    {
        //        gameObject.transform.localScale = new Vector3(0, 1, 1);
        //        sizeNotSet = false;
        //    }

        //    if (deltaX > 0)
        //    {
        //        for (int i = 0; i < extendFactor; i++)
        //        {
        //            //stretches object in the X direction
        //            gameObject.transform.localScale += new Vector3(extendSpeed, 0, 0);
        //            //moves the object along to accomidate for equal stretching on both sides
        //            gameObject.transform.Translate((extendSpeed) / 2, 0, 0);            
        //            yield return new WaitForSeconds(extendSpeed / 10);
        //        }
        //    }
        //    if (deltaX < 0)
        //    {
        //        for (int i = 0; i < extendFactor; i++)
        //        {
        //            //stretches object in the X direction
        //            gameObject.transform.localScale += new Vector3(extendSpeed, 0, 0);
        //            //moves the object along to accomidate for equal stretching on both sides
        //            gameObject.transform.Translate((-extendSpeed) / 2, 0, 0);
        //            yield return new WaitForSeconds(extendSpeed / 10);
        //        }
        //    }
        //}
        //else if (Mathf.Abs(deltaX) < Mathf.Abs(deltaY))
        //{
        //    if (sizeNotSet)
        //    {
        //        gameObject.transform.localScale = new Vector3(1, 0F, 1);
        //        sizeNotSet = false;
        //    }


        //    if (deltaY > 0)
        //    {
        //        for (int i = 0; i < extendFactor; i++)
        //        {
        //            //stretches object in the Y direction

        //            gameObject.transform.localScale += new Vector3(0, extendSpeed, 0);
        //            //moves the object along to accomidate for equal stretching on both sides
        //            gameObject.transform.Translate(0, (extendSpeed / 2), 0);
        //            yield return new WaitForSeconds(extendSpeed / 10);
        //        }
        //    }
        //    if (deltaY < 0)
        //    {
        //        for (int i = 0; i < extendFactor; i++)
        //        {
        //            //stretches object in the Y direction
        //            gameObject.transform.localScale += new Vector3(0, extendSpeed, 0);
        //            //moves the object along to accomidate for equal stretching on both sides
        //            gameObject.transform.Translate(0, (-extendSpeed / 2), 0);
        //            yield return new WaitForSeconds(extendSpeed / 10);
        //        }
        //    }
        //}
        #endregion
    }

    //Returns true or false based on if the object should be growing or not
    //Seems silly, and it is, but this is needed for the coroutine 
    public bool IsGrowing()
    {
        if (grow)
        {
            return true;
        }
        else return false;
    }

    #region Collision Check to stop growth - ***Currently incomplete***
    private void OnCollisionEnter(Collision col)
    {
        //The earth spell growth can be stopped by adding more tags to this if statement
        if (col.collider.gameObject.layer == LayerMask.NameToLayer("Platforms")
            || col.collider.gameObject.CompareTag("Earth Spell Object"))
        {

            // bool firstCollision is instantiated in the Start() function
            //This is used to negate the initial collision of instantiating the earth spell inside of an earth block
            if (!firstCollision)
            {
                firstCollision = true;
            }
            else
            {
                //print("Stop Cooroutine");
                StopCoroutine(co);
            }

        }
    }
    #endregion

}