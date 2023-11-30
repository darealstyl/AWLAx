//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System.IO;
using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocLocation("action-sequence/built-in-plugins")]
    [AutoDoc("This plug-in allows you `Instantiate` a prefab.")]
    public class SpawnPrefabPlugin : ActionSequencePlugin
    {

        #region Fields

        [AutoDocAs("Prefab", "Reference to the `Prefab` to instantiate.")] [Tooltip("Object to spawn")] public GameObject prefab;
        [AutoDocAs("Offset", "Offset from current `GameObject`'s position to use on spawned prefab.")] [Tooltip("Offset to use when spawning")] public Vector3 offset;
        [AutoDocAs("Use Rotation", "If checked the spawned prefab's rotation will be set to match the current `GameObject`'s rotation.")] [Tooltip("Use current rotation")] public bool useRotation;
        [AutoDocAs("Parent", "If checked the spawned prefab's rotation will parented `GameObject`'s transform.")] [Tooltip("Parent object to current transfrom")] public bool parent;

        [AutoDocSuppress] public string path;
        [AutoDocSuppress] public string bundleName;
        [AutoDocSuppress] public string assetName;
        [AutoDocSuppress] public bool z_spawnError;

        #endregion

        #region Properties

        [AutoDocSuppress] public override Texture2D icon { get { return GetResourceImage("icons/object"); } }

        [AutoDocSuppress] public override string title { get { return "Spawn prefab"; } }

        [AutoDocSuppress]
        public override string titlebarText
        {
            get
            {
                if (parent)
                {
                    return "Spawn and parent " + GetObjectName() + " at " + offset;
                }
                return "Spawn " + GetObjectName() + " at " + offset;
            }
        }

        [AutoDocSuppress] public override string description { get { return "Spawns an object at designated location."; } }

        #endregion

        #region Plugin Methods

        [AutoDocSuppress]
        public override void StartAction(ActionSequence host)
        {
            isComplete = false;
            isStarted = true;
            GameObject go;
            if (string.IsNullOrEmpty(bundleName))
            {
                // Resource
                go = InterfaceManager.ObjectManagement.InstantiateObject(Resources.Load<GameObject>(path), host.transform.parent.transform);
            }
            else
            {
                // Bundle
                go = InterfaceManager.ObjectManagement.InstantiateObject(GetBundle().LoadAsset<GameObject>(assetName), host.transform.parent.transform);
            }

            go.transform.localPosition = offset;
            if (parent)
            {
                if (useRotation)
                {
                    go.transform.rotation = Quaternion.Euler(Vector3.zero);
                }
            }
            else
            {
                go.transform.SetParent(null);
                if (useRotation)
                {
                    go.transform.rotation = host.transform.parent.transform.rotation;
                }
            }

            isComplete = true;
        }

        #endregion

        #region Private Methods

        private AssetBundle GetBundle()
        {
            foreach (AssetBundle bundle in AssetBundle.GetAllLoadedAssetBundles())
            {
                if (bundle.name == bundleName) return bundle;
            }

            return AssetBundle.LoadFromFile(Path.Combine(Application.persistentDataPath, assetName));
        }

        private string GetObjectName()
        {
            if (prefab == null) return "(null)";
            return prefab.name;
        }

        #endregion

    }
}