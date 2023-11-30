//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocLocation("input-managers")]
    [AutoDocExcludeBase]
    [AutoDoc("This implementation of InputManager handles input from Unity's classic Input system. This is the implementation used by default.")]
    [CreateAssetMenu(menuName = "Tools/GDTK/Settings/Unity Input Manager", fileName = "Unity Input Manager")]
    public class UnityInput : InputManager
    {

        #region Public Methods

        [AutoDocSuppress]
        public override float GetAxis(string axisName) { if (string.IsNullOrEmpty(axisName)) return 0; return Input.GetAxis(axisName); }

        [AutoDocSuppress]
        public override bool GetButton(string buttonName) { return Input.GetButton(buttonName); }

        [AutoDocSuppress]
        public override bool GetButtonDown(string buttonName) { return Input.GetButtonDown(buttonName); }

        [AutoDocSuppress]
        public override bool GetButtonUp(string buttonName) { return Input.GetButtonUp(buttonName); }

        [AutoDocSuppress]
        public override bool GetKey(KeyCode key) { return Input.GetKey(key); }

        [AutoDocSuppress]
        public override bool GetKeyDown(KeyCode key) { return Input.GetKeyDown(key); }

        [AutoDocSuppress]
        public override bool GetKeyUp(KeyCode key) { return Input.GetKeyUp(key); }

        #endregion

    }
}