//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System;
using System.IO;

namespace NullSave.GDTK.JSON
{
    [AutoDocSuppress]
    [Serializable]
    public class jsonUniversalPluginWrapper
    {

        #region Fields

        public string serializationType;
        public string serializationData;
        public string serializationNamespace;
        public string id;

        #endregion

        #region Constructors

        public jsonUniversalPluginWrapper() { }

        public jsonUniversalPluginWrapper(Stream stream)
        {
            DataLoad(stream);
        }

        #endregion

        #region Public Methods

        public void DataLoad(Stream stream)
        {
            id = stream.ReadStringPacket();
            serializationNamespace = stream.ReadStringPacket();
            serializationType = stream.ReadStringPacket();
            serializationData = stream.ReadStringPacket();
        }

        public void DataSave(Stream stream)
        {
            stream.WriteStringPacket(id);
            stream.WriteStringPacket(serializationNamespace);
            stream.WriteStringPacket(serializationType);
            stream.WriteStringPacket(serializationData);
        }

        #endregion

    }
}
