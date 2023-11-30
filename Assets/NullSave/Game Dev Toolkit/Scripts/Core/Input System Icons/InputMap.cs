//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System;
using TMPro;

namespace NullSave.GDTK
{
    [Serializable]
    [AutoDocLocation("action-icons-system/support-classes")]
    [AutoDoc("Defines a relationship between an input name and a sprite index")]
    public class InputMap
    {

        #region Variables

        [AutoDoc("Name of the associated input")] public string inputName;
        [AutoDoc("Sprite index for the input")] public int tmpSpriteIndex;

        #endregion

        #region Properties

        [AutoDoc("Sprite asset for the input")] public TMP_SpriteAsset TMPSpriteAsset { get; set; }

        #endregion

    }
}