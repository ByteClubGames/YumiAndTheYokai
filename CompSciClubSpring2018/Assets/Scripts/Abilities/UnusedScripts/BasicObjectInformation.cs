/* BasicObjectInformation.cs
 * Date Created: 3/7/18
 * Last Edited: 3/7/18
 * Programmer: Jack Bruce
 * Description: Class made to hold all basic information objects have. Cuts 
 * down on code a lil.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicObjectInformation {
    private string name;
    private string description;
    private Sprite icon;

    public BasicObjectInformation(string oName) {
        name = oName;
    }

    public BasicObjectInformation(string oName, string oDescription) {
        name = oName;
        description = oDescription;
    }

    public BasicObjectInformation(string oName, string oDescription, Sprite oIcon)
    {
        name = oName;
        description = oDescription;
        icon = oIcon;
    }

    public string ObjectName
    {
        get { return name; }
    }

    public string ObjectDescription {
        get { return description; }
    }

    public Sprite ObjectIcon
    {
        get { return icon; }
    }

    
}
