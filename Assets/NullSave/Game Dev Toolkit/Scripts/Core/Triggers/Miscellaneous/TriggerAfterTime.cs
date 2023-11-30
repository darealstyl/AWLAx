//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace NullSave.GDTK
{
    [AutoDoc("Raises events after a set number of seconds")]
    [AutoDocLocation("triggers")]
    public class TriggerAfterTime : MonoBehaviour
    {

        #region Fields

        [Tooltip("Seconds to wait before triggering event")] public float secondsToWait;
        [Tooltip("Repeat event every X seconds")] public bool repeat;
        [Tooltip("Event raised when time elapses")] public UnityEvent onTimeElapsed;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            StartCoroutine(WaitAndInvoke());
        }

        #endregion

        #region Private Methods

        private IEnumerator WaitAndInvoke()
        {
            yield return new WaitForSecondsRealtime(secondsToWait);
            if (enabled)
            {
                onTimeElapsed?.Invoke();
                if(repeat)
                {
                    StartCoroutine(WaitAndInvoke());
                }
            }
        }

        #endregion

    }
}