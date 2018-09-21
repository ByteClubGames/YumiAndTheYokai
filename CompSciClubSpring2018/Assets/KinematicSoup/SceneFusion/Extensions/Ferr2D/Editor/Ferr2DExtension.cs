using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using KS.SceneFusion.API;

namespace KS.SceneFusion.Extensions.Editor
{
    /**
     * Makes Scene Fusion and Ferr2D play nice together.
     * 
     * NOTE: If you get compilation errors because Ferr2D_Path or Ferr2DT_PathTerrain are not defined, go to
     * Project Settings > Player > Scripting Define Symbols, remove SF_FERR2D and press enter.
     */
    [InitializeOnLoad]
    public class Ferr2DExtension
    {
#if SF_FERR2D
        private static float FERR2D_OVERLAY_HEIGHT = 16f;
        private static HashSet<Ferr2D_Path> m_rebuiltPaths = new HashSet<Ferr2D_Path>();
        private static float m_onlineStatusDeltaY = 0f;

        /**
         * Initialization
         */
        static Ferr2DExtension()
        {
            // Ferr2D does not respect NotEditable hide flags so we have to explicitly prevent edits to Ferr2D
            // components on locked objects.
            sfUtility.PreventEditsWhileLocked<Ferr2D_Path>();
            sfUtility.PreventEditsWhileLocked<Ferr2DT_PathTerrain>();

            // Register property change handlers
            sfObjectUtility.RegisterPropertyChangeHandler<Ferr2D_Path>(OnPropertyChange);
            sfObjectUtility.RegisterPropertyChangeHandler<Ferr2DT_PathTerrain>(OnPropertyChange);
            sfObjectUtility.RegisterPropertyChangeHandler<sfFerr2DAdaptor>(OnPropertyChange);

            // Register before spawn request handler
            sfObjectUtility.BeforeFirstSync += BeforeFirstSync;

            // Register new Ferr2D mesh callback
            sfFerr2DAdaptor.NewFerr2DMeshCallback = OnNewFerr2DMesh;

            // Sync hidden property m_SortingOrder on MeshRenderers
            sfUtility.SyncHiddenProperties<MeshRenderer>("m_SortingOrder");

            EditorApplication.update += Update;
        }

        /**
         * Called every frame.
         */
        private static void Update()
        {
            m_rebuiltPaths.Clear();

            // Move online status indicator down/up when Ferr2D overlay shows/hides.
            if (sfUtility.IsInitialized && sfUtility.IsLoggedIn)
            {
                Vector2 offset = sfUtility.GetOnlineStatusOffset();
                // OnChanged is set while the Ferr2D overlay is showing
                if (Ferr2D_PathEditor.OnChanged != null)
                {
                    if (offset.y < FERR2D_OVERLAY_HEIGHT)
                    {
                        m_onlineStatusDeltaY = FERR2D_OVERLAY_HEIGHT - offset.y;
                        offset.y = FERR2D_OVERLAY_HEIGHT;
                        // Change online status offset but do not save the new offset.
                        sfUtility.SetOnlineStatusOffset(offset, false);
                    }
                }
                else if (m_onlineStatusDeltaY != 0f)
                {
                    offset.y -= m_onlineStatusDeltaY;
                    m_onlineStatusDeltaY = 0f;
                    // Change online status offset but do not save the new offset.
                    sfUtility.SetOnlineStatusOffset(offset, false);
                }
            }
            else
            {
                m_onlineStatusDeltaY = 0f;
            }
        }

        /**
         * Called before syncing a game object for the first time. Adds our adaptor component to Ferr2D objects to sync
         * some additional data.
         * 
         * @param   GameObject gameObject a spawn request will be sent for.
         * @return  bool true - we always want to sync the object.
         */
        private static bool BeforeFirstSync(GameObject gameObject)
        {
            Ferr2D_Path path = gameObject.GetComponent<Ferr2D_Path>();
            if (path != null)
            {
                // This is a Ferr2D object. Add our adaptor component if there isn't one already.
                sfFerr2DAdaptor adaptor = gameObject.GetComponent<sfFerr2DAdaptor>();
                if (adaptor == null)
                {
                   adaptor = gameObject.AddComponent<sfFerr2DAdaptor>();
                }
                if (adaptor.HasControlledMesh)
                {
                    MeshFilter filter = gameObject.GetComponent<MeshFilter>();
                    if (filter != null && filter.sharedMesh != null)
                    {
                        // The game object has a mesh whose data is controlled by Ferr2D. Tell Scene Fusion not to
                        // upload the mesh data since Ferr2D will be generating it.
                        sfObjectUtility.DontUploadSceneAsset(filter.sharedMesh);
                    }
                }
            }
            return true;
        }

        /**
         * Called when a property on a Ferr2D component is changed by another user. Rebuilds the Ferr2D mesh.
         * 
         * @param   SerializedProperty property that changed.
         */
        private static void OnPropertyChange(SerializedProperty property)
        {
            Component component = property.serializedObject.targetObject as Component;
            if (component == null)
            {
                return;
            }

            Ferr2DT_PathTerrain terrain = component.GetComponent<Ferr2DT_PathTerrain>();
            if (terrain == null || terrain.Path == null)
            {
                return;
            }

            Ferr2D_Path path = terrain.Path;
            if (!m_rebuiltPaths.Add(path))
            {
                // We've already rebuilt the mesh this frame.
                return;
            }

            sfFerr2DAdaptor adaptor = path.GetComponent<sfFerr2DAdaptor>();
            if (adaptor != null && !adaptor.HasControlledMesh)
            {
                // The mesh on this object was not generated by the Ferr2D components on this object. If we
                // rebuild it will replace the existing mesh which we do not want.
                return;
            }

            MeshFilter filter = path.GetComponent<MeshFilter>();
            if (filter != null && filter.sharedMesh != null)
            {
                // Ensure the mesh name is what Ferr2D expects so Ferr2D will update the existing mesh instead of
                // creating a new one.
                filter.sharedMesh.name = terrain.GetMeshName();
            }

            // Rebuild the mesh
            path.UpdateDependants(true);

            // Rebuild the collider in play mode
            if (EditorApplication.isPlaying)
            {
                path.UpdateColliders();
            }
        }

        /**
         * Called when we detect a new Ferr2D mesh.
         * 
         * @param   Mesh mesh generated by Ferr2D.
         */
        private static void OnNewFerr2DMesh(Mesh mesh)
        {
            // Tell Scene Fusion not to upload this mesh since it will be generated by Ferr2D.
            sfObjectUtility.DontUploadSceneAsset(mesh);
        }

#else
        /**
         * Initialization
         */
        static Ferr2DExtension()
        {
            // Set SF_FERR2D define to compile Ferr2D-dependant code if Ferr2D is detected.
            if (DetectFerr2D())
            {
                sfUtility.SetDefineSymbol("SF_FERR2D");
            }
        }

        /**
         * Detects if Ferr2D is in the project.
         * 
         * @return  bool true if Ferr2D was detected.
         */
        private static bool DetectFerr2D()
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GetType("Ferr2D_Path") != null)
                {
                    return true;
                }
            }
            return false;
        }
#endif
    }
}
