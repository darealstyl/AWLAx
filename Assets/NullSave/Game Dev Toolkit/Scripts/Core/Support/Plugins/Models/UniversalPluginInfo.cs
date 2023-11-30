//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System;

namespace NullSave.GDTK
{
    [AutoDocLocation("universal-plugin-system/classes")]
    [AutoDoc("Information about a Universal Plugin entry")]
    public class UniversalPluginInfo<T>
    {

        #region Fields

        [AutoDoc("Plugin")] public T plugin;
        [AutoDoc("Type of plugin")] public Type type;

        #endregion

        #region Constructors

        public UniversalPluginInfo(T plugin, Type type)
        {
            this.plugin = plugin;
            this.type = type;
        }

        public UniversalPluginInfo(T plugin, string type)
        {
            this.plugin = plugin;
            this.type = Type.GetType(type);
        }

        #endregion

    }
}
