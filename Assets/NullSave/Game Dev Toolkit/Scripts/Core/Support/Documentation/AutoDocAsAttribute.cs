//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System;

namespace NullSave.GDTK
{
    [AutoDocExcludeBase]
    [AutoDocLocation("auto-documentation/attributes")]
    [AutoDoc("Document field using a different name in the output")]
    [AttributeUsage(AttributeTargets.Field)]
    public class AutoDocAs : Attribute
    {

        #region Fields

        [AutoDoc("Description")] public string Description;
        [AutoDoc("Name to use")] public string Name;

        #endregion

        #region Constructor

        public AutoDocAs(string name)
        {
            Name = name;
        }

        public AutoDocAs(string name, string description)
        {
            Description = description;
            Name = name;
        }

        #endregion

    }
}
