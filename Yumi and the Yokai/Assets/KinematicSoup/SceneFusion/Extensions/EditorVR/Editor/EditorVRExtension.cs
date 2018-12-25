using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using UnityEngine;
using UnityEditor;
#if SF_EDITORVR
using UnityEditor.Experimental.EditorVR.Core;
using UnityEditor.Experimental.EditorVR.Tools;
#endif

using KS.SceneFusion.API;
using KS.Reactor;

namespace KS.SceneFusion.Extensions.Editor
{
    /**
     * Syncs EditorVR's annotations' materials and meshes.
     * 
     * NOTE: If you get compilation errors because AnnotationTool is not defined, go to
     * Project Settings > Player > Scripting Define Symbols, remove SF_EDITORVR and press enter.
     */
    [InitializeOnLoad]
    public class EditorVRExtension
    {
        private const string CHANNEL = "EditorVRExtension";
#if SF_EDITORVR
        private const string PREFIX = "SceneFusion_EditorVR_Annotations_";
        private const string MATERIAL = "Material_";
        private const string MESH = "MESH_";
        private static ksLinkedList<MeshFilter> m_pendingMeshes = new ksLinkedList<MeshFilter>();
        private static ksLinkedList<string> m_pendingServerChanges = new ksLinkedList<string>();

        private static Dictionary<MeshRenderer, string> m_materials = new Dictionary<MeshRenderer, string>();
        private static Dictionary<MeshFilter, string> m_meshes = new Dictionary<MeshFilter, string>();

        private static Material m_annotationMaterial = null;

        static readonly List<MeshFilter> m_MeshFiltersToRemove = new List<MeshFilter>();

        private static TransformTool m_transformTool = null;
        private static HashSet<UnityEngine.Object> m_grabbedObjects = new HashSet<UnityEngine.Object>();

        /**
         * Initialization
         */
        static EditorVRExtension()
        {
            EditorApplication.update += Update;

            AnnotationTool.AnnotationUpdated += UploadMesh;
            AnnotationTool.AnnotationFinished += UploadMeshAndSendChanges;

            sfCustomProperties.OnCreateCustomProperty += OnCreateCustomProperty;
            sfCustomProperties.OnChangeCustomProperty += OnChangeCustomProperty;
            sfCustomProperties.OnChangeCustomPropertyFailed += OnChangeCustomProperty;
            sfUtility.OnConnect += OnConnect;
            sfUtility.OnDisconnect += OnDisconnect;

            sfUtility.SuppressAssetDatabaseWarningFor<Material, MeshRenderer>();
        }

        /**
         * On connect
         */
        private static void OnConnect()
        {
            RegisterObjectEventHandlers();
        }

        /**
         * Clean up on disconnect.
         */
        private static void OnDisconnect()
        {
            m_materials.Clear();
            m_meshes.Clear();

            m_pendingMeshes.Clear();
            m_pendingServerChanges.Clear();

            UnregisterObjectEventHandlers();
        }

        /**
         * Removes properties on destroyed annotation game objects. Uploads pending mesh changes. 
         * Processes pending property changes from server.
         */
        private static void Update()
        {
            if (m_transformTool == null && VRView.viewerCamera != null)
            {
                RegisterObjectEventHandlers();
            }
            else if (m_transformTool != null && VRView.viewerCamera == null)
            {
                UnregisterObjectEventHandlers();
            }

            RemoveDestroyedProperties();

            m_MeshFiltersToRemove.Clear();
            foreach (MeshFilter meshFilter in m_pendingMeshes)
            {
                if (!meshFilter)
                {
                    m_MeshFiltersToRemove.Add(meshFilter);
                    continue;
                }

                UploadMesh(meshFilter);
            }

            foreach (var meshFilter in m_MeshFiltersToRemove)
            {
                m_pendingMeshes.Remove(meshFilter);
            }

            foreach (string id in m_pendingServerChanges)
            {
                if (id.StartsWith(PREFIX + MATERIAL))
                {
                    CreateMaterial(id);
                }
                else if (id.StartsWith(PREFIX + MESH))
                {
                    UpdateMesh(id);
                }
            }
        }

        /**
         * If any annotation game object is destroyed, remove the material property and mesh property for it.
         */
        private static void RemoveDestroyedProperties()
        {
            List<MeshRenderer> renderers = new List<MeshRenderer>();
            foreach (KeyValuePair<MeshRenderer, string> pair in m_materials)
            {
                if (pair.Key != null)
                {
                    continue;
                }
                renderers.Add(pair.Key);
                if (sfCustomProperties.CanEdit(pair.Value))
                {
                    sfCustomProperties.RemoveCustomProperty(pair.Value);
                }
            }
            foreach (MeshRenderer renderer in renderers)
            {
                m_materials.Remove(renderer);
            }

            List<MeshFilter> filters = new List<MeshFilter>();
            foreach (KeyValuePair<MeshFilter, string> pair in m_meshes)
            {
                if (pair.Key != null)
                {
                    continue;
                }
                filters.Add(pair.Key);
                if (sfCustomProperties.CanEdit(pair.Value))
                {
                    sfCustomProperties.RemoveCustomProperty(pair.Value);
                }
            }
            foreach (MeshFilter filter in filters)
            {
                m_meshes.Remove(filter);
            }
        }

        /**
         * Uploads mesh changes. If the game object is not synced yet, add this mesh to the pending list.
         * If there is no property created yet for this mesh, create a property for it.
         * 
         * @param   MeshFilter meshFilter
         */
        private static void UploadMesh(MeshFilter meshFilter)
        {
            GameObject go = meshFilter.gameObject;
            uint entityId = sfObjectUtility.GetGameObjectId(go);
            if (entityId != 0)
            {
                m_pendingMeshes.Remove(meshFilter);
                string meshPropertyId = PREFIX + MESH + entityId;
                if (!sfCustomProperties.HasProperty(meshPropertyId))
                {
                    if (meshFilter.sharedMesh == null && !m_pendingMeshes.Contains(meshFilter))
                    {
                        m_pendingMeshes.Add(meshFilter);
                        return;
                    }
                    string materialPropertyId = PREFIX + MATERIAL + entityId;
                    Material material = go.GetComponent<MeshRenderer>().sharedMaterial;
                    Color color = material.GetColor("_EmissionColor");
                    float[] colorArray = new float[] { color.r, color.g, color.b, color.a };
                    var byteArray = new byte[colorArray.Length * 4];
                    Buffer.BlockCopy(colorArray, 0, byteArray, 0, byteArray.Length);
                    sfCustomProperties.CreateCustomProperty(materialPropertyId, byteArray, false);
                    List<byte> meshBuffer = new List<byte>();
                    ksBitOStream output = new ksBitOStream(meshBuffer);
                    try
                    {
                        sfMeshSerializer.Serialize(output, meshFilter.sharedMesh);
                        output.Align();
                    }
                    catch (Exception e)
                    {
                        ksLog.Error(CHANNEL, "Error serializing " + meshFilter.sharedMesh, e);
                        return;
                    }
                    sfCustomProperties.CreateCustomProperty(meshPropertyId, meshBuffer, false);
                }
                else
                {
                    if (sfCustomProperties.IsPropertyCreationPending(meshPropertyId))
                    {
                        m_pendingMeshes.Add(meshFilter);
                        return;
                    }
                    List<byte> meshBuffer = new List<byte>();
                    ksBitOStream output = new ksBitOStream(meshBuffer);
                    try
                    {
                        sfMeshSerializer.Serialize(output, meshFilter.sharedMesh);
                        output.Align();
                    }
                    catch (Exception e)
                    {
                        ksLog.Error(CHANNEL, "Error serializing " + meshFilter.sharedMesh, e);
                        return;
                    }
                    sfCustomProperties.SetCustomProperty(meshPropertyId, meshBuffer);
                }
            }
            else if (!m_pendingMeshes.Contains(meshFilter))
            {
                m_pendingMeshes.Add(meshFilter);
            }
        }

        /**
         * Uploads a mesh and sends changes to the server for the mesh's game object and its parent and siblings. 
         * Modifying an annotation can change the positions of the parent and all sibling annotations, 
         * so this ensures the positions are all updated correctly.
         * 
         * @param   MeshFilter meshFilter
         */
        private static void UploadMeshAndSendChanges(MeshFilter meshFilter)
        {
            UploadMesh(meshFilter);
            SendChanges(meshFilter.gameObject);
        }

        /**
         * Sends changes to the server for a game object, and its parent and siblings.
         * 
         * @param   GameObject gameObject
         */
        private static void SendChanges(GameObject gameObject)
        {
            Transform group = gameObject.transform.parent;
            sfObjectUtility.SendChanges(group.gameObject);
            foreach (Transform child in group)
            {
                sfObjectUtility.SendChanges(child.gameObject);
            }
        }

        /**
         * Create custom property event handler. If it is an annotation material property, create material.
         * 
         * @param   string id of the custom property that was created
         */
        private static void OnCreateCustomProperty(string id)
        {
            if (id.StartsWith(PREFIX + MATERIAL))
            {
                CreateMaterial(id);
            }
            else if (id.StartsWith(PREFIX + MESH))
            {
                UpdateMesh(id);
            }
        }

        /**
         * Change custom property event handler. If an annotation mesh property was changed by another user or a local
         * change requests failed, update this mesh.
         * 
         * @param   string id of the custom property that changed
         */
        private static void OnChangeCustomProperty(string id)
        {
            if (id.StartsWith(PREFIX + MESH))
            {
                UpdateMesh(id);
            }
        }

        /**
         * Creates annotation material on create annotation property.
         * 
         * @param   string id of material property
         */
        private static void CreateMaterial(string id)
        {
            if (m_annotationMaterial == null)
            {
                m_annotationMaterial = AssetDatabase.LoadAssetAtPath<Material>(
                    "Assets/EditorVR/Tools/AnnotationTool/Materials/AnnotationMaterial.mat");
            }
            uint gameObjectId = 0;
            try
            {
                gameObjectId = Convert.ToUInt32(id.Substring((PREFIX + MATERIAL).Length));
            }
            catch (Exception ex)
            {
                ksLog.Error(CHANNEL, "Failed to parse game object id from property id " + id + ".", ex);
                m_pendingServerChanges.Remove(id);
                return;
            }
            GameObject gameObject = sfObjectUtility.GetGameObjectById(gameObjectId);
            if (gameObject != null)
            {
                MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    m_materials[renderer] = id;
                    renderer.sharedMaterial = Material.Instantiate<Material>(m_annotationMaterial);
                    byte[] colorData = null;
                    if (sfCustomProperties.TryGetCustomProperty(id, ref colorData) && colorData != null)
                    {
                        float[] colorArray = new float[4];
                        Buffer.BlockCopy(colorData, 0, colorArray, 0, colorData.Length);
                        Color color = new Color(colorArray[0], colorArray[1], colorArray[2], colorArray[3]);
                        renderer.sharedMaterial.SetColor("_EmissionColor", color);
                    }
                    m_pendingServerChanges.Remove(id);
                }
            }
            else if (!m_pendingServerChanges.Contains(id))
            {
                m_pendingServerChanges.Add(id);
            }
        }

        /**
         * Updates annotation meshes with server changes.
         * 
         * @param   string id of mesh property
         */
        private static void UpdateMesh(string id)
        {
            uint gameObjectId = 0;
            try
            {
                gameObjectId = Convert.ToUInt32(id.Substring((PREFIX + MESH).Length));
            }
            catch (Exception ex)
            {
                ksLog.Error(CHANNEL, "Failed to parse game object id from property id " + id + ".", ex);
                m_pendingServerChanges.Remove(id);
                return;
            }
            GameObject gameObject = sfObjectUtility.GetGameObjectById(gameObjectId);
            bool updated = false;
            if (gameObject != null)
            {
                MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
                if (meshFilter != null && meshFilter.sharedMesh != null
                    && meshFilter.sharedMesh.GetIndices(0).Length > 0)
                {
                    m_meshes[meshFilter] = id;
                    List<byte> meshData = null;
                    if (sfCustomProperties.TryGetCustomProperty(id, ref meshData) && meshData != null)
                    {
                        sfMeshSerializer.Deserialize(new ksBitIStream(meshData), meshFilter.sharedMesh);
                    }
                    m_pendingServerChanges.Remove(id);
                    updated = true;
                }
            }
            if (!updated && !m_pendingServerChanges.Contains(id))
            {
                m_pendingServerChanges.Add(id);
            }
        }

        /**
         * Registers OnObjectsGrabbed and OnObjectsDropped event handlers.
         */
        private static void RegisterObjectEventHandlers()
        {
            m_transformTool = null;
            if (VRView.viewerCamera != null)
            {
                m_transformTool = VRView.viewerCamera.transform.root.gameObject.GetComponent<TransformTool>();
                if (m_transformTool == null)
                {
                    return;
                }
                m_transformTool.objectsGrabbed += OnObjectsGrabbed;
                m_transformTool.objectsDropped += OnObjectsDropped;
            }
        }

        /**
         * Unregisters OnObjectsGrabbed and OnObjectsDropped event handlers.
         */
        private static void UnregisterObjectEventHandlers()
        {
            if (m_transformTool != null)
            {
                m_transformTool.objectsGrabbed -= OnObjectsGrabbed;
                m_transformTool.objectsDropped -= OnObjectsDropped;
                m_transformTool = null;
                m_grabbedObjects.Clear();
            }
        }

        /**
         * OnObjectsGrabbed event handler. Adds all grabbed objects to selection.
         * 
         * @param   Transform rayOrigin
         * @param   HashSet<Transform> grabbedObjects
         */
        private static void OnObjectsGrabbed(Transform rayOrigin, HashSet<Transform> grabbedObjects)
        {
            foreach (Transform t in grabbedObjects)
            {
                m_grabbedObjects.Add(t.gameObject);
            }
            HashSet<UnityEngine.Object> selected = new HashSet<UnityEngine.Object>(Selection.objects);
            selected.UnionWith(m_grabbedObjects);
            Selection.objects = selected.ToArray();
        }

        /**
         * OnObjectsDropped event handler. Removes all dropped objects from selection.
         * 
         * @param   Transform rayorigin
         * @param   Transform[] droppedObjects
         */
        private static void OnObjectsDropped(Transform rayOrigin, Transform[] droppedObjects)
        {
            HashSet<UnityEngine.Object> selected = new HashSet<UnityEngine.Object>(Selection.objects);
            foreach (Transform t in droppedObjects)
            {
                m_grabbedObjects.Remove(t.gameObject);
                selected.Remove(t.gameObject);
            }
            Selection.objects = m_grabbedObjects.ToArray();
        }
#else
        /**
         * Initialization
         */
        static EditorVRExtension()
        {
            // Set DetectEditorVR define to compile EditorVR-dependant code if EditorVR is detected.
            if (DetectEditorVR())
            {
                sfUtility.SetDefineSymbol("SF_EDITORVR");
            }
        }

        /**
         * Detects if EditorVR is in the project.
         * 
         * @return  bool true if EditorVR was detected.
         */
        private static bool DetectEditorVR()
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type annotationTool = assembly.GetType("UnityEditor.Experimental.EditorVR.Tools.AnnotationTool");
                if (annotationTool == null)
                {
                    continue;
                }

                if (annotationTool.GetField("AnnotationUpdated", BindingFlags.Public | BindingFlags.Static) == null)
                {
                    EditorUtility.DisplayDialog(
                    "Scene Fusion",
                    "Scene fusion requires you to use an updated version of EditorVR for full featured VR editing. "
                    + "For more information go to https://www.kinematicsoup.com/scene-fusion/editorvr",
                    "OK");
                    return false;
                }

                return true;
            }
            return false;
        }
#endif
    }
}
