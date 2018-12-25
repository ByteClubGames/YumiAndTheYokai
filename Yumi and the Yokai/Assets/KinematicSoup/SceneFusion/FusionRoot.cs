using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KS.SceneFusion.Client.Editor
{
    /**
     * This class must be located in the Scene Fusion root folder. We query Unity's asset database for the location of
     * this script at start up to find the root folder.
     */
    internal class FusionRoot : ScriptableObject
    {

    }
}
