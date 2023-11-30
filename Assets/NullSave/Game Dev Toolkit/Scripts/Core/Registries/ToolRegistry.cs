//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace NullSave.GDTK
{
    [AutoDoc("The Tool Registry is a class that allows you to easily find registered objects. Using this system provides much faster results that FindObject methods and is ideal for components that need to be referenced by multiple GameObjects.")]
    [AutoDocLocation("tool-registry/classes")]
    /// <summary>
    /// Static registry for storing and retrieving components
    /// </summary>
    public class ToolRegistry
    {

        #region Fields

        private static List<ToolRegistryEntry> entries;
        private static bool subscribed;

        #endregion

        #region Public Methods

        [AutoDoc("Clear all registered components")]
        public static void Clear()
        {
            if (entries == null)
            {
                entries = new List<ToolRegistryEntry>();
            }
            else
            {
                entries.Clear();
            }
        }

        [AutoDoc("Get registered component")]
        public static T GetComponent<T>()
        {
            EnsureSceneAndList();

            foreach (ToolRegistryEntry entry in entries)
            {
                if (entry.Object is T t)
                {
                    return t;
                }
            }

            return default;
        }

        [AutoDoc("Get registered component with key")]
        [AutoDocParameter("Key to find")]
        public static T GetComponent<T>(string key)
        {
            if (string.IsNullOrEmpty(key)) return GetComponent<T>();

            EnsureSceneAndList();

            foreach (ToolRegistryEntry entry in entries)
            {
                if (entry.Object is T t && entry.Key == key)
                {
                    return t;
                }
            }

            return default;
        }

        [AutoDoc("Get list of registered components")]
        public static List<T> GetComponents<T>()
        {
            EnsureSceneAndList();

            List<T> results = new List<T>();
            foreach (ToolRegistryEntry entry in entries)
            {
                if (entry.Object is T t)
                {
                    results.Add(t);
                }
            }

            return results;
        }

        [AutoDoc("Add a component to registry")]
        [AutoDocParameter("Component to register")]
        [AutoDocParameter("Keep component registered between scenes")]
        public static void RegisterComponent(object component, bool persistBetweenScenes = false)
        {
            RegisterComponent(component, null, persistBetweenScenes);
        }

        [AutoDoc("Add a component to registry")]
        [AutoDocParameter("Component to register")]
        [AutoDocParameter("Key to associate with component")]
        [AutoDocParameter("Keep component registered between scenes")]
        public static void RegisterComponent(object component, string key, bool persistBetweenScenes = false)
        {
            EnsureSceneAndList();

            foreach (ToolRegistryEntry entry in entries)
            {
                if (entry.Object == component && entry.Key == key) return;
            }

            entries.Add(new ToolRegistryEntry { Object = component, Key = key, Persist = persistBetweenScenes, Scene = SceneManager.GetActiveScene() });
        }

        [AutoDoc("Remove a component from registry")]
        [AutoDocParameter("Component to remove")]
        public static void RemoveComponent(object component)
        {
            EnsureSceneAndList();

            foreach (ToolRegistryEntry entry in entries)
            {
                if (entry.Object == component)
                {
                    entries.Remove(entry);
                    return;
                }
            }
        }

        /// <summary>
        /// Remove all components using the specified key
        /// </summary>
        [AutoDoc("Remove a component from registry")]
        [AutoDocParameter("Key associated with object(s) to remove")]
        public static void RemoveComponentsByKey(string key)
        {
            List<ToolRegistryEntry> removeList = new List<ToolRegistryEntry>();
            foreach(ToolRegistryEntry entry in entries)
            {
                if(entry.Key == key)
                {
                    removeList.Add(entry);
                }
            }

            foreach(ToolRegistryEntry entry in removeList)
            {
                entries.Remove(entry);
            }
        }

        #endregion

        #region Private Methods

        private static void ClearNonPersist(Scene scene, LoadSceneMode mode)
        {
            if (mode == LoadSceneMode.Additive) return;

            Scene currentSceen = SceneManager.GetActiveScene();
            List<ToolRegistryEntry> persistedEntries = new List<ToolRegistryEntry>();
            foreach (ToolRegistryEntry entry in entries)
            {
                if (entry.Persist || entry.Scene == currentSceen)
                {
                    persistedEntries.Add(entry);
                }
            }

            entries.Clear();
            entries.AddRange(persistedEntries);
        }

        private static void EnsureSceneAndList()
        {
            if (entries == null) entries = new List<ToolRegistryEntry>();

            if (!subscribed)
            {
                subscribed = true;
                SceneManager.sceneLoaded += ClearNonPersist;
            }
        }

        #endregion

    }
}