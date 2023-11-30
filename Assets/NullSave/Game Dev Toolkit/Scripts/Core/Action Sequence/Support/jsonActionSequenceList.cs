//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System;
using System.Collections.Generic;

namespace NullSave.GDTK.JSON
{
    [Serializable]
    [AutoDocSuppress]
    public class jsonActionSequenceList
    {

        #region Fields

        public string id;
        public List<jsonUniversalPlugin> actions;

        #endregion

        #region Constructor

        public jsonActionSequenceList()
        {
            actions = new List<jsonUniversalPlugin>();
        }

        #endregion

    }
}
