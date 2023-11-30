//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NullSave.GDTK
{
    [AutoDocLocation("triggers")]
    [AutoDoc("This component raises events when the value of the attached `Toggle` changes.")]
    [RequireComponent(typeof(Toggle))]
    public class TriggerByToggleState : MonoBehaviour
    {

        #region Fields

        [Tooltip("Event raised when the toggle value becomes true")] public UnityEvent onValueTrue;
        [Tooltip("Event raised when the toggle value becomes false")] public UnityEvent onValueFalse;

        private Toggle target;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            target = GetComponent<Toggle>();
            target.onValueChanged.AddListener(Trigger);
            Trigger(target.isOn);
        }

        #endregion

        #region Private Methods

        private void Trigger(bool value)
        {
            if (value)
            {
                onValueTrue?.Invoke();
            }
            else
            {
                onValueFalse?.Invoke();
            }
        }

        #endregion

    }
}