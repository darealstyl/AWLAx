//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocLocation("ui/interfaces")]
    [AutoDoc("Makes an item as a tab stop")]
    public interface ITabStop
    {

        #region Fields

        [AutoDoc("Gets the attached object")]
        GameObject attachedObject { get; }

        [AutoDoc("Get/Set parent tab stop id")]
        int parentStopId { get; set; }

        [AutoDoc("Get/Set tab stop id")]
        int tabStopId { get; set; }

        #endregion

    }
}