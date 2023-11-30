//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDoc("Automatically rotates object to face main camera")]
    [AutoDocLocation("miscellaneous")]
    public class RotateToMainCamera : MonoBehaviour
    {

        #region Fields

        [Tooltip("Offset to apply to rotation")] public Vector3 offset;

        #endregion

        #region Unity Methods

        private void Update()
        {
            transform.LookAt(Camera.main.transform.position);
            transform.Rotate(offset);
        }

        #endregion

    }
}