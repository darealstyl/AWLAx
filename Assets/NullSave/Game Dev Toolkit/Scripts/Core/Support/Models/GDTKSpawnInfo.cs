//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System;
using System.IO;
using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDoc("Provides a JSON compatible reference to a Game Object")]
    [AutoDocLocation("miscellaneous/classes")]
    [Serializable]
    public class GDTKSpawnInfo
    {

        #region Fields

        [Tooltip("Source of object information")] public SpawnSource source;
        [Tooltip("Path to object")] public string path;
        [Tooltip("Bundle name")] public string bundleName;
        [Tooltip("Asset name")] public string assetName;
        [Tooltip("Parent spawned object to creator")] public bool parent;
        [Tooltip("Offset to apply to spawned object")] public Vector3 offset;

        [Tooltip("Object to spawn")] [JsonDoNotSerialize] public GameObject gameObject;

#if UNITY_EDITOR
        [SerializeField] private bool z_spawnError;
#endif

        private GameObject spawn;

        #endregion

        #region Public Methods

        [AutoDoc("Creates a clone of this object")]
        public GDTKSpawnInfo Clone()
        {
            GDTKSpawnInfo result = new GDTKSpawnInfo();

            result.gameObject = gameObject;
            result.source = source;
            result.path = path;
            result.bundleName = bundleName;
            result.parent = parent;
            result.offset = offset;

            return result;
        }

        [AutoDoc("Load data from a stream")]
        [AutoDocParameter("Stream to use")]
        [AutoDocParameter("Version")]
        public void DataLoad(Stream stream)
        {
            source = (SpawnSource)stream.ReadInt();
            path = stream.ReadStringPacket();
            bundleName = stream.ReadStringPacket();
            assetName = stream.ReadStringPacket();
            parent = stream.ReadBool();
            offset = stream.ReadVector3();
        }

        [AutoDoc("Save data to a stream")]
        [AutoDocParameter("Stream to use")]
        [AutoDocParameter("Version")]
        public void DataSave(Stream stream)
        {
            stream.WriteInt((int)source);
            stream.WriteStringPacket(path);
            stream.WriteStringPacket(bundleName);
            stream.WriteStringPacket(assetName);
            stream.WriteBool(parent);
            stream.WriteVector3(offset);
        }

        [AutoDoc("Destroys spawned instance")]
        public void DestroySpawn()
        {
            if (spawn != null)
            {
                InterfaceManager.ObjectManagement.DestroyObject(spawn);
                spawn = null;
            }
        }

        [AutoDoc("Gets GameObject from data, loading if needed")]
        public GameObject GetGameObject()
        {
            if (gameObject != null) return gameObject;
            LoadGameObject();
            return gameObject;
        }

        [AutoDoc("Loads GameObject from data")]
        public void LoadGameObject()
        {
            switch (source)
            {
                case SpawnSource.AssetBundle:
                    gameObject = GetBundle().LoadAsset<GameObject>(assetName);
                    break;
                case SpawnSource.Resources:
                    gameObject = Resources.Load<GameObject>(path);
                    break;
            }
        }

        [AutoDoc("Spawns the GameObject")]
        [AutoDocParameter("Transform to set as parent")]
        public void Spawn(Transform parent)
        {
            DestroySpawn();

            switch (source)
            {
                case SpawnSource.AssetBundle:
                    if (string.IsNullOrEmpty(path)) return;
                    if (string.IsNullOrEmpty(bundleName)) return;
                    if (string.IsNullOrEmpty(assetName)) return;
                    break;
                case SpawnSource.Resources:
                    if (string.IsNullOrEmpty(path)) return;
                    break;
            }

            LoadGameObject();

            if (gameObject != null)
            {
                spawn = InterfaceManager.ObjectManagement.InstantiateObject(gameObject);
                spawn.transform.position = parent.transform.position + offset;
                if (parent)
                {
                    spawn.transform.SetParent(parent);
                }
            }
        }

        #endregion

        #region Private Methods

        [JsonAfterDeserialization]
        private void AfterDeserialize()
        {
            // This cannot be done during run mode
            // Reason: Possible to happen outside main render thread and Unity does not like that
            // Keep this in for design-time editor
            if (System.Threading.Thread.CurrentThread.ManagedThreadId == 1)
            {
                LoadGameObject();
            }
        }

        private AssetBundle GetBundle()
        {
            foreach (AssetBundle bundle in AssetBundle.GetAllLoadedAssetBundles())
            {
                if (bundle.name == bundleName) return bundle;
            }

            return AssetBundle.LoadFromFile(Path.Combine(Application.persistentDataPath, assetName));
        }

        #endregion

    }
}