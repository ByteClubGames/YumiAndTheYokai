using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using KS.SceneFusion.API;

#if SF_TERRAINCOMPOSER2
using TerrainComposer2;
#endif

namespace KS.SceneFusion.Extensions
{
    /**
     * Makes Scene Fusion and Terrain Composer 2 play nice together. 
     * 
     * NOTE:
     * - All terrains must use the same settings defined by the TC_TerrainArea.
     * - Enabling/disabling a node will not sync if that node is not selected.
     * - Changing the blending method between two nodes will not sync if the right node is not selected.
     * - The source code for TerrainComposer 2 must be modified to avoid invalid argument exceptions. Open
     *   TC_Compute.cs. There are three methods where a ComputeBuffer is constructed with a length taken from an array
     *   of an item group: RunColorCompute, RunSplatCompute, and RunItemCompute. In these methods, check if the array
     *   length is zero before constructing the ComputeBuffer and return if it is.
     * - If you get compilation errors because TC_terrainArea or other TC classes are not defined, go to
     *   Project Settings > Player > Scripting Define Symbols, remove SF_TERRAINCOMPOSER2 and press enter.
     */
    [InitializeOnLoad]
    class TerrainComposer2Extension
    {
#if SF_TERRAINCOMPOSER2
        private static HashSet<TC_TerrainArea> m_rebuiltTerrains = new HashSet<TC_TerrainArea>();
        private static bool m_generateTerrain = false;
        private static float m_seed = 0f;

        /**
         * Initialization
         */
        static TerrainComposer2Extension()
        {
            // Register property change handlers.
            sfObjectUtility.RegisterPropertyChangeHandler<TC_TerrainArea>(OnTerrainAreaChange);
            sfObjectUtility.RegisterPropertyChangeHandler<TC_ItemBehaviour>(OnItemBehaviourChange);
            sfObjectUtility.RegisterPropertyChangeHandler<TC_Settings>(OnSettingsChange);
            // Register before first sync handler.
            sfObjectUtility.BeforeFirstSync += BeforeFirstSync;

            // Don't sync these properties. '%' prefix means match only the name of the property instead of the full 
            // path.
            sfUtility.DontSyncProperties<TC_TerrainArea>("%objectsParent");
            sfUtility.DontSyncProperties<TC_Generate>("stackEntry");
            sfUtility.DontSyncProperties<TC_Settings>("scale", "scrollOffset", "selectionOld", "lastPath",
                "exportPath");

            // Suppress warning for texture scene assets on these components
            sfUtility.SuppressAssetDatabaseWarningFor<Texture2D, TC_SelectItemGroup>();
            sfUtility.SuppressAssetDatabaseWarningFor<Texture2D, TC_SelectItem>();
            sfUtility.SuppressAssetDatabaseWarningFor<Texture2D, TC_TerrainArea>();
            sfUtility.SuppressAssetDatabaseWarningFor<Texture2D, TC_Compute>();
            sfUtility.SuppressAssetDatabaseWarningFor<RenderTexture, TC_TerrainArea>();
            sfUtility.SuppressAssetDatabaseWarningFor<RenderTexture, TC_Compute>();
            sfUtility.SuppressAssetDatabaseWarningFor<RenderTexture, TC_ItemBehaviour>();
            EditorApplication.update += Update;
        }

        /**
         * Called every frame.
         */
        private static void Update()
        {
            if (!sfUtility.IsConnected)
            {
                return;
            }
            m_rebuiltTerrains.Clear();

            // Set hideflags to none on TC2 objects so SF will sync them.
            if (TC_Reporter.instance != null && TC_Reporter.instance.hideFlags == HideFlags.HideInHierarchy)
            {
                TC_Reporter.instance.gameObject.hideFlags = HideFlags.None;
            }
            if (TC_Generate.instance != null && TC_Generate.instance.hideFlags == HideFlags.HideInHierarchy)
            {
                TC_Generate.instance.gameObject.hideFlags = HideFlags.None;
            }
            if (TC_Settings.instance != null && TC_Settings.instance.hideTerrainGroup)
            {
                TC_Settings.instance.hideTerrainGroup = false;
                if (TC_Area2D.current != null && TC_Area2D.current.terrainLayer != null)
                {
                    TC_Area2D.current.terrainLayer.hideFlags = HideFlags.None;
                }
            }

            // Sync the seed if it changed.
            if (TC_Settings.instance != null && TC_Settings.instance.seed != m_seed)
            {
                m_seed = TC_Settings.instance.seed;
                sfObjectUtility.SendChanges(TC_Settings.instance.gameObject);
            }

            // The TC_SelectItemGroup inspector allows you to edit the TC_SelectItem children without selecting them or
            // registering the changes on the Undo stack, so Scene Fusion misses the changes. We tell Scene Fusion to
            // check for changes in the child TC_SelectItems while the TC_SelectItemGroup is selected.
            if (Selection.activeGameObject != null && Selection.activeGameObject.hideFlags != HideFlags.NotEditable &&
                Selection.activeGameObject.GetComponent<TC_SelectItemGroup>() != null)
            {
                foreach (Component component in Selection.activeGameObject.GetComponentsInChildren<TC_SelectItem>())
                {
                    sfObjectUtility.SendChanges(component.gameObject);
                }
            }

            // Regenerate terrain if it is stale
            if (m_generateTerrain && TC_Area2D.current != null &&
                TC_Area2D.current.terrainLayer.layerGroups[TC.heightOutput] != null)
            {
                m_generateTerrain = false;
                TC_Generate.instance.Generate(true);
                SceneView.RepaintAll();
            }
        }

        /**
         * Called before syncing a game object for the first time. Checks if the game object is the parent for
         * generated TC2 objects and prevents it from syncing if it is.
         * 
         * @param   GameObject gameObject that will be synced.
         * @param   bool false if the game object is the parent for generated TC2 objects. This will stop it from syncing.
         */
        private static bool BeforeFirstSync(GameObject gameObject)
        {
            return gameObject.name != "TerrainComposer Objects" || gameObject.transform.parent == null ||
                gameObject.transform.parent.GetComponent<Terrain>() == null;
        }

        /**
         * Called when a TC_Settings property is changed by another user.
         * 
         * @param   SerializedProperty property that changed.
         */
        private static void OnSettingsChange(SerializedProperty property)
        {
            // Regenerate terrain if the seed changed.
            if (property.propertyPath == "seed")
            {
                m_generateTerrain = true;
            }
        }

        /**
         * Called when a TC_TerrainArea property is changed by another user. Applies the terrain area settings to all
         * terrain and regenerates terrain.
         * 
         * @param   SerializedProperty property that changed.
         */
        private static void OnTerrainAreaChange(SerializedProperty property)
        {
            TC_TerrainArea terrainArea = property.serializedObject.targetObject as TC_TerrainArea;
            if (terrainArea != null && m_rebuiltTerrains.Add(terrainArea))
            {
                terrainArea.ApplySize();
                terrainArea.ApplyResolution();
                foreach (TCUnityTerrain terrain in terrainArea.terrains)
                {
                    terrainArea.ApplySplatTextures(terrain);
                }
                terrainArea.ApplyTrees();
                terrainArea.ApplyGrass();

                if (TC_Generate.instance != null && TC_Generate.instance.autoGenerate)
                {
                    m_generateTerrain = true;
                }
            }
        }

        /**
         * Called when a TC_ItemBehaviour property is changed by another user.
         * 
         * @param   SerializedProperty property that changed.
         */
        private static void OnItemBehaviourChange(SerializedProperty property)
        {
            m_generateTerrain = true;

            TC_SelectItem selectItem = property.serializedObject.targetObject as TC_SelectItem;
            if (selectItem != null && selectItem.parentItem != null)
            {
                selectItem.Refresh();
            }
        }
#else
        /**
         * Initialization
         */
        static TerrainComposer2Extension()
        {
            // Set SF_TERRAINCOMPOSER2 define to compile TerrainComposer2-dependant code if TerrainComposer2 is detected.
            if (DetectTerrainComposer2())
            {
                sfUtility.SetDefineSymbol("SF_TERRAINCOMPOSER2");
            }
        }

        /**
         * Detects if TerrainComposer2 is in the project.
         * 
         * @return  bool true if TerrainComposer2 was detected.
         */
        private static bool DetectTerrainComposer2()
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GetType("TerrainComposer2.TC") != null)
                {
                    return true;
                }
            }
            return false;
        }
#endif
    }
}
