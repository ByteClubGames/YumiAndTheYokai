using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace KS.SceneFusion.Extensions.Editor
{
    /**
     * Custom inspector for sfFerr2DAdaptor, a component that syncs additional data needed to make Scene Fusion and
     * Ferr2D work well together.
     */
    [CustomEditor(typeof(sfFerr2DAdaptor))]
    class sfFerr2DAdaptorEditor : UnityEditor.Editor
    {
        /**
         * Called to draw the inspector GUI.
         */
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("This component syncs additional data needed to make Scene Fusion and Ferr2D work well together. " +
                "Do not remove it while using Scene Fusion.", MessageType.Info);

            // While the object is selected, check if its mesh is controlled by Ferr2D.
            sfFerr2DAdaptor adaptor = target as sfFerr2DAdaptor;
            if (adaptor != null)
            {
                adaptor.CheckMesh();
            }
        }

        /**
         * Called to draw the scene GUI.
         */
        private void OnSceneGUI()
        {
            // Ferr2D can change the mesh from OnSceneGUI so we need to check for a new mesh here.
            sfFerr2DAdaptor adaptor = target as sfFerr2DAdaptor;
            if (adaptor != null)
            {
                adaptor.CheckMesh();
            }
        }
    }
}
