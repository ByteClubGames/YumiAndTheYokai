/*
 * Programmer:   Hunter Goodin 
 * Date Created: 02/16/2018 @  9:40 PM 
 * Last Updated: 02/16/2018 @ 11:35 PM 
 * File Name:    PlayerJumper.cs 
 * Description:  This script will be responsible for the player's movements. 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumper : MonoBehaviour
{
    public Rigidbody playerRB;
    public float jumpSpeed; 

    public Vector2 firstPressPos = new Vector2(0, 0);
    public Vector2 secondPressPos;
    public Vector2 currentSwipe; 

    private void Update()
    {
        Swiper();
    }

    private void Swiper()
    {
        //get user input
        if (Input.GetMouseButtonDown(0))
        {
            //get first mouse position
            firstPressPos.x = Input.mousePosition.x;
            firstPressPos.y = Input.mousePosition.y;
        }
        if (Input.GetMouseButtonUp(0))
        {
            secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
        }

        if (currentSwipe.y > 50 && currentSwipe.y != 0)
        {
            playerRB.transform.Translate(transform.up * Time.deltaTime * jumpSpeed);

            firstPressPos = new Vector2(0, 0);
            secondPressPos = new Vector2(0, 0);
            currentSwipe = new Vector2(0, 0);
        }
    }
}
