using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using KS.SceneFusion.API;

namespace KS.SceneFusion.Menus
{
    /**
     * Menu functions and hotkeys for managing persistent locks on objects. Anyone can add or remove peristent locks
     * to or from objects that prevent them from being edited. These locks are saved in the scene so they persist
     * between sessions.
     */
    public class LockMenu
    {
        private const int PRIORITY = 30;
        private const string MENU_PATH = "GameObject/Lock/";

        /**
         * Adds a persistent lock to all selected game objects.
         */
        [MenuItem(MENU_PATH + "Lock Objects %l", false, PRIORITY)]
        private static void Lock()
        {
            sfObjectUtility.Lock(Selection.gameObjects);
        }

        /**
         * Adds a persitent lock to all selected game objects and their descendants.
         */
        [MenuItem(MENU_PATH + "Lock Objects and Children %#l", false, PRIORITY)]
        private static void RecursiveLock()
        {
            sfObjectUtility.RecursiveLock(Selection.gameObjects);
        }

        /**
         * Removes persistent locks on all selected game objects.
         */
        [MenuItem(MENU_PATH + "Unlock Objects %u", false, PRIORITY)]
        private static void Unlock()
        {
            sfObjectUtility.Unlock(Selection.gameObjects);
        }

        /**
         * Removes persistent locks on all selected game Objects and their descendants.
         */
        [MenuItem(MENU_PATH + "Unlock Objects and Children %#u", false, PRIORITY)]
        private static void RecursiveUnlock()
        {
            sfObjectUtility.RecursiveUnlock(Selection.gameObjects);
        }
    }
}
