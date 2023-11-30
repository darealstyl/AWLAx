//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocLocation("object-pull")]
    [AutoDoc("This component allows the attached GameObject to be targeted by an Object Pull Source.")]
    public class ObjectPullTarget : MonoBehaviour
    {

        #region Fields

        [Tooltip("Additional seconds to delay before pull starts")] public float additionalDelay;
        [Tooltip("Additional seconds to add to pull duration")] public float additionalDuration;

        #endregion

        #region Properties

        [AutoDoc("Seconds of delay already elapsed.")]
        public float ElapsedDelay { get; set; }

        [AutoDoc("Seconds spent pulling object.")]
        public float ElapsedPull { get; set; }

        [AutoDoc("Position of object before pull started.")]
        public Vector3 StartPosition { get; set; }

        #endregion

    }
}