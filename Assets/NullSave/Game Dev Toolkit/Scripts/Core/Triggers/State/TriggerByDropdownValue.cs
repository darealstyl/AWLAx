//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace NullSave.GDTK
{
    [AutoDocLocation("triggers")]
    [AutoDoc("This component raises an event when the attached `TMP_Dropdown` changes its value.")]
    public class TriggerByDropdownValue : MonoBehaviour
    {

        #region Fields

        [Tooltip("Object to listen to")] public TMP_Dropdown target;
        [Tooltip("Desired values to listen for")] public List<int> desiredValues;

        [Tooltip("Event raised when the new value is in the desiredValues list")] public UnityEvent onValueMatch;
        [Tooltip("Event raised when the new value is not in the desiredValues list")] public UnityEvent onValueMismatch;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (target == null)
            {
                target = GetComponent<TMP_Dropdown>();
            }
            target.onValueChanged.AddListener(ValueChanged);
            ValueChanged(target.value);
        }

        private void Reset()
        {
            target = GetComponent<TMP_Dropdown>();
        }

        #endregion

        #region Private Methods

        private void ValueChanged(int value)
        {
            if(desiredValues.Contains(value))
            {
                onValueMatch?.Invoke();
            }
            else
            {
                onValueMismatch?.Invoke();
            }
        }

        #endregion

    }
}