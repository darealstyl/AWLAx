//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System;

namespace NullSave.GDTK.JSON
{
    [AutoDocSuppress]
    [Serializable]
    public class jsonUniversalPlugin
    {

        #region Fields

        public string pluginType;
        public string pluginData;
        public string pluginNamespace;

        #endregion

        #region Public Methods

        public static jsonUniversalPlugin FromModel<T>(UniversalPluginWrapper<T> source) where T : UniversalPlugin
        {
            jsonUniversalPlugin result = new jsonUniversalPlugin();

            result.pluginData = source.serializationData;
            result.pluginType = source.serializationType;
            result.pluginNamespace = source.serializationNamespace;

            return result;
        }

        #endregion

    }
}
