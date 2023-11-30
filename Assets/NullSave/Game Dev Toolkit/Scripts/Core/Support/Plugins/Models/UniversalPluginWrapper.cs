//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using NullSave.GDTK.JSON;
using System;
using System.IO;
using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocLocation("universal-plugin-system/classes")]
    [AutoDoc("Wrapper for Universal Plugins")]
    [Serializable]
    public class UniversalPluginWrapper<T> where T : UniversalPlugin
    {

        #region Fields

        [AutoDoc("Plugin")] [JsonDoNotSerialize] [NonSerialized] public T plugin;
        [AutoDoc("Plugin type")] public string serializationType;
        [AutoDoc("Serialized plugin data")] public string serializationData;
        [AutoDoc("Plugin namespace")] public string serializationNamespace;

        [SerializeField] private string id;

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

        public UniversalPluginWrapper() { }

        public UniversalPluginWrapper(T plugin)
        {
            this.plugin = plugin;
            Type t = plugin.GetType();
            serializationData = SimpleJson.ToJSON(plugin);
            serializationType = t.ToString();
            serializationNamespace = t.Assembly.GetName().Name;
        }

        public UniversalPluginWrapper(T plugin, string data)
        {
            this.plugin = plugin;
            serializationData = data;
            Type t = plugin.GetType();
            serializationType = t.ToString();
            serializationNamespace = t.Assembly.GetName().Name;
        }

        public UniversalPluginWrapper(string id, string jsonNamespace, string jsonType, string jsonData)
        {
            this.id = id;
            serializationData = jsonData;
            serializationNamespace = jsonNamespace;
            serializationType = jsonType;

            try
            {
                if (string.IsNullOrEmpty(serializationNamespace))
                {
                    plugin = (T)SimpleJson.FromJSON(serializationData, Type.GetType(serializationType));
                }
                else
                {
                    plugin = (T)SimpleJson.FromJSON(serializationData, Type.GetType(serializationType + "," + serializationNamespace));
                }
            }
            catch(Exception ex)
            {
                StringExtensions.LogError("UniversalPluginWrapper", "Ctor", "Unable to deserialize: " + serializationNamespace + ", " + serializationType + " :: error " + ex.Message);
            }
        }

        #endregion

        #region Public Methods

        [AutoDoc("Create a clone of this object")]
        public UniversalPluginWrapper<T> Clone()
        {
            UniversalPluginWrapper<T> result = new UniversalPluginWrapper<T>();

            if (string.IsNullOrEmpty(serializationNamespace))
            {
                result.plugin = (T)SimpleJson.FromJSON(serializationData, Type.GetType(serializationType));
            }
            else
            {
                result.plugin = (T)SimpleJson.FromJSON(serializationData, Type.GetType(serializationType + "," + serializationNamespace));
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
            id = stream.ReadStringPacket();
        }

        [AutoDoc("Get a JSON compatible representation from data loaded via stream")]
        [AutoDocParameter("Stream to use")]
        public static jsonUniversalPluginWrapper JSONFromDataLoad(Stream stream)
        {
            jsonUniversalPluginWrapper result = new jsonUniversalPluginWrapper();

            result.serializationType = stream.ReadStringPacket();
            result.serializationData = stream.ReadStringPacket();
            result.serializationNamespace = stream.ReadStringPacket();
            result.id = stream.ReadStringPacket();

            return result;
        }

        [AutoDoc("Save data to stream")]
        [AutoDocParameter("Stream to use")]
        public void DataSave(Stream stream)
        {
            stream.WriteStringPacket(serializationType);
            stream.WriteStringPacket(serializationData);
            stream.WriteStringPacket(serializationNamespace);
            stream.WriteStringPacket(id);
        }

        [AutoDoc("Get a JSON compatible representation of this object")]
        public jsonUniversalPluginWrapper ToJSON()
        {
            jsonUniversalPluginWrapper result = new jsonUniversalPluginWrapper();

            result.serializationData = serializationData;
            result.serializationNamespace = serializationNamespace;
            result.serializationType = serializationType;
            result.id = id;

            return result;
        }

        #endregion

        #region Serialization

        [AutoDocSuppress]
        [JsonAfterDeserialization]
        public void OnAfterDeserialize()
        {
            try
            {
                if (string.IsNullOrEmpty(serializationData)) return;

                if (string.IsNullOrEmpty(serializationNamespace))
                {
                    plugin = (T)SimpleJson.FromJSON(serializationData, Type.GetType(serializationType));
                }
                else
                {
                    plugin = (T)SimpleJson.FromJSON(serializationData, Type.GetType(serializationType + "," + serializationNamespace));
                }
            }
            catch { }
        }

        [AutoDocSuppress]
        public void OnBeforeSerialize() { }

        #endregion

    }
}