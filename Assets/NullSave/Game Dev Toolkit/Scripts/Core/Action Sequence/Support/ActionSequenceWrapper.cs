//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System;
using System.IO;
using UnityEngine;

namespace NullSave.GDTK
{
    [Serializable]
    [AutoDocLocation("action-sequence/support-classes")]
    [AutoDoc("Wraper class for ActionSequencePlugin to allow for proper serializliation and deserialization in Unity.")]
    public class ActionSequenceWrapper : ISerializationCallbackReceiver
    {

        #region Fields

        [NonSerialized][Tooltip("Instance copy of plugin")] public ActionSequencePlugin plugin;
        [Tooltip("Serialized plugin type")] public string serializationType;
        [Tooltip("Serialized plugin data")] public string serializationData;
        [Tooltip("Serialized plugin namespace")] public string serializationNamespace;

        private string id;

#if UNITY_EDITOR
        [SerializeField] private bool z_expanded;
#endif

        #endregion

        #region Properties

        [AutoDoc("Gets the unique id for the current instance of the plugin wrapper")]
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

        public ActionSequenceWrapper() { }

        public ActionSequenceWrapper(ActionSequencePlugin plugin)
        {
            this.plugin = plugin;
            Type t = plugin.GetType();
            serializationData = SimpleJson.ToJSON(plugin);
            serializationType = t.ToString();
            serializationNamespace = t.Assembly.GetName().Name;
        }

        public ActionSequenceWrapper(ActionSequencePlugin plugin, string data)
        {
            this.plugin = plugin;
            serializationData = data;
            Type t = plugin.GetType();
            serializationType = t.ToString();
            serializationNamespace = t.Assembly.GetName().Name;
        }

        #endregion

        #region Public Methods

        [AutoDoc("Clones the plugin wrapper")]
        public ActionSequenceWrapper Clone()
        {
            ActionSequenceWrapper result = new ActionSequenceWrapper();

            if (string.IsNullOrEmpty(serializationNamespace))
            {
                result.plugin = (ActionSequencePlugin)SimpleJson.FromJSON(serializationData, Type.GetType(serializationType));
            }
            else
            {
                result.plugin = (ActionSequencePlugin)SimpleJson.FromJSON(serializationData, Type.GetType(serializationType + "," + serializationNamespace));
            }

            result.serializationData = serializationData;
            result.serializationNamespace = serializationNamespace;
            result.serializationType = serializationType;

            return result;
        }

        [AutoDoc("Load wrapper from stream")]
        [AutoDocParameter("Stream to use")]
        public void DataLoad(Stream stream)
        {
            serializationType = stream.ReadStringPacket();
            serializationData = stream.ReadStringPacket();
            serializationNamespace = stream.ReadStringPacket();
        }

        [AutoDoc("Save wrapper to stream")]
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
                plugin = (ActionSequencePlugin)SimpleJson.FromJSON(serializationData, Type.GetType(serializationType));
            }
            else
            {
                plugin = (ActionSequencePlugin)SimpleJson.FromJSON(serializationData, Type.GetType(serializationType + "," + serializationNamespace));
            }
        }

        [AutoDocSuppress]
        public void OnBeforeSerialize() { }

        #endregion

    }
}