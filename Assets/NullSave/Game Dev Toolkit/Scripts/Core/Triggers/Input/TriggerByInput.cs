//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;
using UnityEngine.Events;

namespace NullSave.GDTK
{
    [AutoDoc("Raises events based on input")]
    [AutoDocLocation("triggers")]
    public class TriggerByInput : MonoBehaviour
    {

        #region Fields

        [Tooltip("Button to listen to (if any)")] public string buttonName;
        [Tooltip("Key to listen to")] public KeyCode keyCode;
        [Tooltip("Ignore modal windows when listening")] public bool ignoreModals;

        [Tooltip("Event raised when input is pressed")] public UnityEvent onTriggered;
        
        private bool badButton;

        #endregion

        #region Unity Methods

        private void Update()
        {
            if (!ignoreModals && InterfaceManager.IsBlockedByModal(gameObject)) return;
            if (InterfaceManager.Input.GetKeyDown(keyCode)) onTriggered?.Invoke();

            if (!badButton && !string.IsNullOrEmpty(buttonName))
            {
                try
                {
                    if (InterfaceManager.Input.GetButtonDown(buttonName))
                    {
                        onTriggered?.Invoke();
                    }
                }
                catch
                {
                    StringExtensions.LogError(gameObject, "Invoked", "Invalid button name: " + buttonName + "; disabling checks");
                    badButton = true;
                }
            }
        }

        #endregion

    }
}