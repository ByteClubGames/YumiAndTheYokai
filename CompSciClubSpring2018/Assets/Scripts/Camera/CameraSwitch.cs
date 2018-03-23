/* 
 * 
 * Author: Karim Dabboussi
 * Date Created: 03/16/2018 @ 12:50 pm
 * Date Modified: 03/16/2018 @ 12:50 pm
 * Project: CompSciClubSpring2018
 * File: CameraSwitch.cs
 * Description: This script has the functions for switching between the main camera and the pan camera.
 *
 */
using UnityEngine;
using System.Collections;

public class CameraSwitch : MonoBehaviour
{

    public Camera mainCamera;
    public Camera panCamera;

    public void ShowMainCamera()
    {
        mainCamera.enabled = false;
        panCamera.enabled = true;
    }

    public void ShowPanCamera()
    {
        mainCamera.enabled = true;
        panCamera.enabled = false;
    }
}
