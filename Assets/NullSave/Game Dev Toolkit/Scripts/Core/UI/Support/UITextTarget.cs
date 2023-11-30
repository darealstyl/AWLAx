//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;
using UnityEngine.Events;

namespace NullSave.GDTK
{
    [AutoDocLocation("ui")]
    [AutoDoc("Base class for Labels")]
    public class UILabel : MonoBehaviour
    {

        #region Fields

        [Tooltip("Event raised whenever the text changes")] public UnityEvent onTextChanged;

        #endregion

        #region Properties

        [AutoDoc("Gets/Sets the text")]
        public virtual string text { get; set; }

        [AutoDoc("Gets/Sets the color")]
        public virtual Color color { get; set; }

        #endregion

    }
}