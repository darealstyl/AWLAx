//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System;
using System.IO;
using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocLocation("universal-plugin-system/classes")]
    [AutoDoc("Specialized wrapper for SortAndFilterPlugins")]
    [Serializable]
    public class SAFPluginWrapper : ISerializationCallbackReceiver
    {

        #region Fields

        [AutoDoc("Plugin")] [NonSerialized] public SortAndFilterPlugin plugin;
        [AutoDoc("Plugin type")] public string serializationType;
        [AutoDoc("Serialized plugin data")] public string serializationData;
        [AutoDoc("Plugin namespace")] public string serializationNamespace;

        private string id;

#if UNITY_EDITOR
        [SerializeField] private bool z_expanded;
#endif

        #endregion

        #region Properties

        [AutoDoc("Id of plugin instance")]
        public string instanceId
        {
            get
            {
                if (string.IsNullOrEmpty(id))
                {
                    id = Guid.NewGuid().ToString();
                }
                return id;
            }
        }

        #endregion

        #region Constructors

        public SAFPluginWrapper() { }

        public SAFPluginWrapper(SortAndFilterPlugin plugin)
        {
            this.plugin = plugin;
            Type t = plugin.GetType();
            serializationData = SimpleJson.ToJSON(plugin);
            serializationType = t.ToString();
            serializationNamespace = t.Assembly.GetName().Name;
        }

        public SAFPluginWrapper(SortAndFilterPlugin plugin, string data)
        {
            this.plugin = plugin;
            serializationData = data;
            Type t = plugin.GetType();
            serializationType = t.ToString();
            serializationNamespace = t.Assembly.GetName().Name;
        }

        #endregion

        #region Public Methods

        [AutoDoc("Create a clone of this object")]
        public SAFPluginWrapper Clone()
        {
            SAFPluginWrapper result = new SAFPluginWrapper();

            if (string.IsNullOrEmpty(serializationNamespace))
            {
                result.plugin = (SortAndFilterPlugin)SimpleJson.FromJSON(serializationData, Type.GetType(serializationType));
            }
            else
            {
                result.plugin = (SortAndFilterPlugin)SimpleJson.FromJSON(serializationData, Type.GetType(serializationType + "," + serializationNamespace));
            }

            result.serializationData = serializationData;
            result.serializationNamespace = serializationNamespace;
            result.serializationType = serializationType;

            return result;
        }

        [AutoDoc("Load data from stream")]
        [AutoDocParameter("Stream to use")]
        public void DataLoad(Stream stream)
        {
            serializationType = stream.ReadStringPacket();
            serializationData = stream.ReadStringPacket();
            serializationNamespace = stream.ReadStringPacket();
        }

        [AutoDoc("Save data to stream")]
        [AutoDocParameter("Stream to use")]
        public void DataSave(Stream stream)
        {
            stream.WriteStringPacket(serializationType);
            stream.WriteStringPacket(serializationData);
            stream.WriteStringPacket(serializationNamespace);
        }

        #endregion

        #region Serialization

        [AutoDocSuppress]
        public void OnAfterDeserialize()
        {
            if (string.IsNullOrEmpty(serializationNamespace))
            {
                plugin = (SortAndFilterPlugin)SimpleJson.FromJSON(serializationData, Type.GetType(serializationType));
            }
            else
            {
                plugin = (SortAndFilterPlugin)SimpleJson.FromJSON(serializationData, Type.GetType(serializationType + "," + serializationNamespace));
            }
        }

        [AutoDocSuppress]
        public void OnBeforeSerialize() { }

        #endregion

    }
}