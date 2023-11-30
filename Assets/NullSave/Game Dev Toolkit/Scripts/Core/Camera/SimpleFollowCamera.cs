//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDoc("Updates transform position and rotation to follow another object")]
    [AutoDocLocation("miscellaneous")]
    public class SimpleFollowCamera : MonoBehaviour
    {

        #region Fields

        [Tooltip("Object to follow")] public Transform target;
        [Tooltip("Offset to apply to position")] public Vector3 positionOffset;
        [Tooltip("Offset to apply to rotation")] public Vector3 lookAtOffset;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if(target == null)
            {
                FindPlayer();
                if (!enabled) return;
            }
        }

        private void Update()
        {
            transform.position = target.transform.position + positionOffset;
            transform.LookAt(transform.position + lookAtOffset);
        }

        #endregion

        #region Private Methods

        private void FindPlayer()
        {
            GameObject go = GameObject.FindGameObjectWithTag("Player");
            if (go != null)
            {
                target = go.gameObject.transform;
            }
            else
            {
                StringExtensions.Log(gameObject, "Simple3rdPersonCamera", "No Player tagged object.");
                enabled = false;
            }
        }

        #endregion

    }
}