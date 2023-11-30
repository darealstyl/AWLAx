//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System;

namespace NullSave.GDTK
{
    [AutoDocExcludeBase]
    [AutoDocLocation("auto-documentation/attributes")]
    [AutoDoc("Method parameter documentation entry")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class AutoDocParameter : Attribute
    {

        #region Fields

        [AutoDoc("Description of the parameter")] public string Description;

        #endregion

        #region Constructor

        public AutoDocParameter(string description)
        {
            Description = description;
        }

        #endregion


    }
}
