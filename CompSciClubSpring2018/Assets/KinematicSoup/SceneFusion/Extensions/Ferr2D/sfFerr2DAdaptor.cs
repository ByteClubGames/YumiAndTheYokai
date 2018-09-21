using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KS.SceneFusion.Extensions
{
    /**
     * This script is automatically added to Ferr2D game objects when using Scene Fusion and syncs additional data
     * needed to prevent Ferr2D from creating new meshes when it shouldn't.
     */
    [AddComponentMenu("")] // Prevent users from adding this component
    [ExecuteInEditMode]
    public class sfFerr2DAdaptor : MonoBehaviour
    {
        /**
         * Handler for new Ferr2D mesh events.
         * 
         * @param   Mesh mesh - Ferr2D mesh for this object.
         */
        public delegate void NewFerr2DMeshHandler(Mesh mesh);

        /**
         * Called when we detect a Ferr2D mesh.
         */
        public static NewFerr2DMeshHandler NewFerr2DMeshCallback;

        /**
         * Is the mesh on this object controlled by the Ferr2D components on this object? When Ferr2D generates a mesh
         * it checks if the mesh name matches its expected name based on game object name and instance id. If the name
         * is different, Ferr2D will generate a new mesh rather than update the existing one. This is true if the mesh
         * name matches what Ferr2D is expecting.
         */
        public bool HasControlledMesh = false;

        /**
         * Checks if the mesh name matches Ferr2D's expected name on awake.
         */
        private void Awake()
        {
            CheckMesh();
        }

        /**
         * Checks if this object's mesh's name matches what Ferr2D is expecting and updates HasControlledMesh
         * accordingly.
         */
        public void CheckMesh()
        {
#if SF_FERR2D
            if ((gameObject.hideFlags & HideFlags.NotEditable) == HideFlags.NotEditable)
            {
                // Do nothing while the game object is locked.
                return;
            }

            Ferr2DT_PathTerrain terrain = GetComponent<Ferr2DT_PathTerrain>();
            if (terrain == null)
            {
                HasControlledMesh = false;
                return;
            }

            MeshFilter filter = GetComponent<MeshFilter>();
            if (filter == null)
            {
                HasControlledMesh = false;
                return;
            }

            bool oldValue = HasControlledMesh;
            HasControlledMesh = filter.sharedMesh != null && filter.sharedMesh.name == terrain.GetMeshName();
            if (!oldValue && HasControlledMesh && NewFerr2DMeshCallback != null)
            {
                // We detected a new mesh controlled by the Ferr2D components on this object. Call the callback to
                // prevent the mesh from being uploaded.
                NewFerr2DMeshCallback(filter.sharedMesh);
            }
#endif
        }
    }
}
