//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;
using UnityEngine.Events;

namespace NullSave.GDTK
{
    [AutoDocLocation("ui")]
    [AutoDoc("Provides tooltip support to object")]
    public class TooltipClient : MonoBehaviour
    {

        #region Fields

        [Tooltip("Display static text")] public bool dynamicText;
        [Tooltip("Display static text")] public Label textSource;
        [Tooltip("Tooltip text to display.")] [TextArea(2, 5)] public string tooltip;
        [Tooltip("Number of seconds to modify the display delay for this time.")] public float modifyDelay;
        [Tooltip("Custom tooltip prefab")] public TooltipDisplay customTooltip;

        [Tooltip("Event raised just prior to displaying tooltip")] public UnityEvent onPreDisplay;
        [Tooltip("Event raised when custom tooltip is initialized")] public UnityEvent onInitCustomTip;
        [Tooltip("Event raised after initialization")] public TooltipEvent onPostInit;

        [Tooltip("Event raised when tooltip is displayed for this object")] public UnityEvent onDisplay;
        [Tooltip("Event raised when tooltip is hidden for this object")] public UnityEvent onHide;

        #endregion

        #region Properties

        [AutoDoc("Gets the tooltip text")]
        public string tipText
        {
            get
            {
                if (!dynamicText) return tooltip;
                return textSource.text;
            }
        }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            // Perform check to make sure an interface manager is created if one doesn't already exist.
            _ = InterfaceManager.Current;
        }

        #endregion

    }
}