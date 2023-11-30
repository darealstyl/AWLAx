//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System;

namespace NullSave.GDTK
{
    [AutoDoc("Implements object as a selectable object")]
    [AutoDocLocation("miscellaneous/interfaces")]
    public interface ISelectableOption
    {

        #region Properties

        [AutoDoc("Gets information about the object")]
        public BasicInfo optionInfo { get; }

        [AutoDoc("Gets the type of the object")]
        public Type type { get; }

        #endregion

    }
}
