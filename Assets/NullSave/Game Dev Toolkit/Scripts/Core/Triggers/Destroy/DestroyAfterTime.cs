//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System.Collections;
using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocLocation("triggers")]
    [AutoDoc("This component waits for a specified number of seconds before destroying the attached GameObject.")]
    public class DestroyAfterTime : MonoBehaviour
    {

        #region Fields

        [Tooltip("How long should we wait before destroying the GameObject?")] public float secondsToLive;

        #endregion

        #region Unity Methods

        private void Start()
        {
            StartCoroutine(WaitAndDestroy());
        }

        #endregion

        #region Private Methods

        private IEnumerator WaitAndDestroy()
        {
            yield return new WaitForSecondsRealtime(secondsToLive);
            InterfaceManager.ObjectManagement.DestroyObject(gameObject);
        }

        #endregion

    }
}