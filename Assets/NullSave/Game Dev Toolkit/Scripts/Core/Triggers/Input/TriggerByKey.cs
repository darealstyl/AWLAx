//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;
using UnityEngine.Events;

namespace NullSave.GDTK
{
    [AutoDocLocation("triggers")]
    [AutoDoc("This component raises an event whenever the specified key conditions are met.")]
    public class TriggerByKey : MonoBehaviour
    {

        #region Fields

        [Tooltip("KeyCode to listen to")] public KeyCode key;
        [Tooltip("Raise event on key")] public bool onKey;
        [Tooltip("Raise event on key down")] public bool onKeyDown;
        [Tooltip("Raise event on key up")] public bool onKeyUp;

        [Tooltip("Event raised when conditions are met")] public UnityEvent onTrigger;

        #endregion

        #region Unity Methods

        private void Update()
        {
            if (onKey && InterfaceManager.Input.GetKey(key)) onTrigger?.Invoke();
            if (onKeyDown && InterfaceManager.Input.GetKeyDown(key)) onTrigger?.Invoke();
            if (onKeyUp && InterfaceManager.Input.GetKeyUp(key)) onTrigger?.Invoke();
        }

        #endregion

    }
}