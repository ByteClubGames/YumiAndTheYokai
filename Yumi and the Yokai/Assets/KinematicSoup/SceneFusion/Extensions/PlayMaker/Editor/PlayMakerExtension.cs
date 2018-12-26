using System;
using System.Reflection;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

using KS.SceneFusion.API;
using KS.Reactor;

#if SF_PLAYMAKER
using HutongGames.PlayMaker;
using HutongGames.PlayMakerEditor;
#endif

namespace KS.SceneFusion.Extensions.Editor
{
    /**
     * Makes Scene Fusion and PlayMaker play nice together.
     * 
     * NOTE: If you get compilation errors because PlayMakerGUI, PlayMakerFSM or HutongGames are not defined, go to
     * Project Settings > Player > Scripting Define Symbols, remove SF_PLAYMAKER and press enter.
     */
    [InitializeOnLoad]
    public class PlayMakerExtension : UnityEditor.AssetModificationProcessor
    {
#if SF_PLAYMAKER
        private const string SALT = "__SceneFusionLock";
        private const string LOCKED = "__LOCKED__";
        private const string UNLOCKED = "__UNLOCKED__";

        private static bool m_connected = false;
        private static bool m_fsmEditorDirty = false;
        // Store all PlayMakerFSMs, so we can unlock them before saving the scene.
        private static ksLinkedList<PlayMakerFSM> m_playMakerFSMs = new ksLinkedList<PlayMakerFSM>();
        // Store all PlayMakerFSMs we unlocked temporarily, so we can lock them after the scene is saved. 
        private static List<PlayMakerFSM> m_unlockedPlayMakerFSMs = new List<PlayMakerFSM>();
        // Store all PlayMakerFSMs that need to be locked because the game object is locked,
        // so we can lock them in the next frame because we want to apply property changes first.
        private static List<PlayMakerFSM> m_playMakerFSMsToBeLocked = new List<PlayMakerFSM>();
        // Store all PlayMakerFSMs whose properties were modified so we can reinitialize them.
        private static HashSet<PlayMakerFSM> m_dirtyPlayMakerFSMs = new HashSet<PlayMakerFSM>();

        /**
         * Initialization
         */
        static PlayMakerExtension()
        {
            // Don't sync PlayMakerGUI because PlayMaker can create it automatically.
            sfUtility.DontSyncObjectsWith<PlayMakerGUI>();

            // Register property change handlers
            sfObjectUtility.RegisterPropertyChangeHandler<PlayMakerFSM>(OnPropertyChange);

            // Register on connect/disconnect handler
            sfUtility.OnConnect += OnConnect;
            sfUtility.OnDisconnect += OnDisconnect;

            // Register on spawn request handler
            sfObjectUtility.OnSpawn += OnSpawn;

            // Register on lock/unlock handler
            sfObjectUtility.OnLock += OnLock;
            sfObjectUtility.OnUnlock += OnUnlock;

            // Register on add/remove component handler
            sfObjectUtility.OnAddComponent += OnAddComponent;
            sfObjectUtility.OnRemoveComponent += OnRemoveComponent;

            // Sync hidden property fsm on PlayMakerFSM
            sfUtility.SyncHiddenProperties<PlayMakerFSM>("fsm");

            EditorApplication.update += Update;
        }

        /**
         * Called before Unity assets are saved.
         */
        public static string[] OnWillSaveAssets(string[] paths)
        {
            if (m_connected)
            {
                foreach (string path in paths)
                {
                    if (path.EndsWith(".unity"))
                    {
                        UnlockAllFsms();
                        break;
                    }
                }
            }
            return paths;
        }

        /**
         * Called every frame.
         */
        private static void Update()
        {
            // Reinitialize modified fsms and repaint all if some fsms were modified.
            foreach (PlayMakerFSM playMakerFSM in m_dirtyPlayMakerFSMs)
            {
                playMakerFSM.Fsm.Reinitialize();
            }
            m_dirtyPlayMakerFSMs.Clear();
            if (m_fsmEditorDirty)
            {
                m_fsmEditorDirty = false;
                EditorCommands.UpdateGraphView();
                FsmEditor.SetFsmDirty();
                FsmEditor.RepaintAll();
            }

            // Lock all fsms that were temporarily unlocked before saving the scene.
            if (m_unlockedPlayMakerFSMs.Count > 0)
            {
                LockAllFsms();
            }

            // Lock all fsms that are on a locked game object.
            foreach (PlayMakerFSM playMakerFSM in m_playMakerFSMsToBeLocked)
            {
                LockFsm(playMakerFSM);
            }
            m_playMakerFSMsToBeLocked.Clear();
        }

        /**
         * Called when the client connects to a Scene Fusion session.
         */
        private static void OnConnect()
        {
            m_connected = true;
        }

        /**
         * Called when the client disconnects from a Scene Fusion session.
         */
        private static void OnDisconnect()
        {
            m_connected = false;
            m_playMakerFSMs.Clear();
            m_unlockedPlayMakerFSMs.Clear();
            m_playMakerFSMsToBeLocked.Clear();
        }

        /**
         * Called when spawning a game object. Records all PlayMakerFSM components.
         * 
         * @param   GameObject gameObject that was spawned.
         */
        private static void OnSpawn(GameObject gameObject)
        {
            if (gameObject == null)
            {
                return;
            }
            PlayMakerFSM playMakerFSM = gameObject.GetComponent<PlayMakerFSM>();
            if (playMakerFSM != null)
            {
                m_playMakerFSMs.Add(playMakerFSM);
            }
        }

        /**
         * Called when a game object is locked by another user.
         * 
         * @param   GameObject gameObject that is locked.
         */
        private static void OnLock(GameObject gameObject)
        {
            if (gameObject == null)
            {
                return;
            }
            PlayMakerFSM playMakerFSM = gameObject.GetComponent<PlayMakerFSM>();
            if (playMakerFSM != null)
            {
                m_playMakerFSMsToBeLocked.Add(playMakerFSM);
            }
        }

        /**
         * Called when a game object is unlocked by another user.
         * 
         * @param   GameObject gameObject that is unlocked.
         */
        private static void OnUnlock(GameObject gameObject)
        {
            if (gameObject == null)
            {
                return;
            }
            PlayMakerFSM playMakerFSM = gameObject.GetComponent<PlayMakerFSM>();
            if (playMakerFSM != null)
            {
                UnlockFsm(playMakerFSM);
            }
        }

        /**
         * Called when a component is added.
         * 
         * @param   Component component that is added.
         */
        private static void OnAddComponent(Component component)
        {
            PlayMakerFSM playMakerFSM = component as PlayMakerFSM;
            if (playMakerFSM != null)
            {
                m_playMakerFSMs.Add(playMakerFSM);
                if (playMakerFSM.hideFlags == HideFlags.NotEditable)
                {
                    m_playMakerFSMsToBeLocked.Add(playMakerFSM);
                }
            }
        }

        /**
         * Called when a component is removed.
         * 
         * @param   Component component that is removed.
         */
        private static void OnRemoveComponent(Component component)
        {
            PlayMakerFSM playMakerFSM = component as PlayMakerFSM;
            if ((object)playMakerFSM != null)
            {
                m_playMakerFSMs.Remove(playMakerFSM);
            }
        }

        /**
         * Called when a property on a PlayMakerFSM component is changed by another user.
         * 
         * @param   SerializedProperty property that changed.
         */
        private static void OnPropertyChange(SerializedProperty property)
        {
            PlayMakerFSM playMakerFSM = property.serializedObject.targetObject as PlayMakerFSM;
            if (playMakerFSM == null)
            {
                return;
            }
            m_dirtyPlayMakerFSMs.Add(playMakerFSM);
            m_fsmEditorDirty = true;

            if (property.propertyPath == "fsm.locked")
            {
                Fsm fsm = playMakerFSM.Fsm;
                string password = fsm.Password;
                if (fsm.Locked)
                {
                    if (password.StartsWith(SALT + UNLOCKED))
                    {
                        fsm.Unlock(password);
                        password = password.Substring((SALT + UNLOCKED).Length);
                        fsm.Lock(SALT + LOCKED + password);
                    }
                }
                else if (password.StartsWith(SALT + LOCKED))
                {
                    fsm.Unlock(password);
                    password = password.Substring((SALT + LOCKED).Length);
                    fsm.Lock(SALT + UNLOCKED + password);
                }
                return;
            }

            if (property.propertyPath == "fsm.password")
            {
                if (playMakerFSM.hideFlags != HideFlags.NotEditable)
                {
                    return;
                }
                Fsm fsm = playMakerFSM.Fsm;
                if (fsm.Locked)
                {
                    string password = fsm.Password;
                    fsm.Unlock(password);
                    fsm.Lock(SALT + LOCKED + password);
                }
                return;
            }
        }

        /**
         * Locks fsm.
         * 
         * @param   PlayMakerFSM playMakerFSM to be locked.
         */
        private static void LockFsm(PlayMakerFSM playMakerFSM)
        {
            if (playMakerFSM != null)
            {
                m_fsmEditorDirty = true;
                string lockInfo = UNLOCKED;
                Fsm fsm = playMakerFSM.Fsm;
                string password = fsm.Password;
                if (fsm.Locked)
                {
                    lockInfo = LOCKED;
                    if (!password.StartsWith(SALT))
                    {
                        fsm.Unlock(password);
                    }
                }
                fsm.Lock(SALT + lockInfo + password);
            }
        }

        /**
         * Removes salt from password. Unlocks fsm if it was locked beacuse the game object was locked,
         * or just removes the salt but keeps the object locked if it was locked by user.
         * 
         * @param   PlayMakerFSM playMakerFSM to be unlocked.
         */
        private static void UnlockFsm(PlayMakerFSM playMakerFSM)
        {
            if (playMakerFSM != null)
            {
                m_fsmEditorDirty = true;
                Fsm fsm = playMakerFSM.Fsm;
                string password = fsm.Password;
                fsm.Unlock(password);
                if (password.StartsWith(SALT + LOCKED))
                {
                    fsm.Lock(password.Substring((SALT + LOCKED).Length));
                }
                else if (password.StartsWith(SALT + UNLOCKED))
                {
                    fsm.Lock(password.Substring((SALT + UNLOCKED).Length));
                    fsm.Unlock(fsm.Password);
                }
            }
        }

        /**
         * Locks all fsms that were temporarily unlocked because the scene was saved.
         */
        private static void LockAllFsms()
        {
            foreach (PlayMakerFSM playMakerFSM in m_unlockedPlayMakerFSMs)
            {
                if (playMakerFSM != null)
                {
                    LockFsm(playMakerFSM);
                }
            }
            m_unlockedPlayMakerFSMs.Clear();
        }

        /**
         * Unlocks all fsms that were locked by us so they won't be saved locked.
         */
        private static void UnlockAllFsms()
        {
            foreach (PlayMakerFSM playMakerFSM in m_playMakerFSMs)
            {
                if (playMakerFSM == null)
                {
                    m_playMakerFSMs.Remove(playMakerFSM);
                }
                else if (playMakerFSM.Fsm.Password.StartsWith(SALT))
                {
                    UnlockFsm(playMakerFSM);
                    m_unlockedPlayMakerFSMs.Add(playMakerFSM);
                }
            }
        }
#else
        /**
         * Initialization
         */
        static PlayMakerExtension()
        {
            // Set SF_PLAYMAKER define to compile PlayMaker-dependant code if PlayMaker is detected.
            if (DetectPlayMaker())
            {
                sfUtility.SetDefineSymbol("SF_PLAYMAKER");
            }
        }

        /**
         * Detects if PlayMaker is in the project.
         * 
         * @return  bool true if PlayMaker was detected.
         */
        private static bool DetectPlayMaker()
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GetType("PlayMakerFSM") != null)
                {
                    return true;
                }
            }
            return false;
        }
#endif
    }
}
