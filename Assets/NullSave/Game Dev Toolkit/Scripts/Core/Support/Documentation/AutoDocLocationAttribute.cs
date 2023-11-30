//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System;

namespace NullSave.GDTK
{
    [AutoDocExcludeBase]
    [AutoDocLocation("auto-documentation/attributes")]
    [AutoDoc("Sets a specific location under the output directory for this documentation")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class AutoDocLocation : Attribute
    {

        #region Fields

        [AutoDoc("Relative location to place documentation")] public string Location;

        #endregion

        #region Constructor

        public AutoDocLocation(string location)
        {
            Location = location;
        }

        #endregion

    }
}
