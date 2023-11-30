//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System;
using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocSuppress]
    [Serializable]
    public class TemplatedLabel
    {

        #region Fields

        [Tooltip("Label to target when setting formatted text")] public Label target;
        [Tooltip("Format to apply to text")] [TextArea(2, 5)] public string format;

        #endregion

    }
}
