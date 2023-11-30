//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System;

namespace NullSave.GDTK
{
    [AutoDocExcludeBase]
    [AutoDocLocation("auto-documentation/attributes")]
    [AutoDoc("Automatically document a class, property, method, or field")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Interface)]
    public class AutoDoc : Attribute
    {

        #region Fields

        [AutoDoc("Description")] public string Description;

        #endregion

        #region Constructor

        public AutoDoc(string description)
        {
            Description = description;
        }

        #endregion

    }
}
