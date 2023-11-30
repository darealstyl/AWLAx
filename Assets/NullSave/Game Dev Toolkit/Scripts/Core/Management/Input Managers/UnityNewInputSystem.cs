//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

#if ENABLE_INPUT_SYSTEM

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NullSave.GDTK
{
    [AutoDocLocation("input-managers")]
    [AutoDocExcludeBase]
    [AutoDoc("This implementation of InputManager handles input from Unity's new Input System. To use this manager you need to supply the Input Action Asset associated with input for your project.")]
    [CreateAssetMenu(menuName = "Tools/GDTK/Settings/Unity New Input System", fileName = "Unity New Input System")]
    public class UnityNewInputSystem : InputManager
    {

        #region Fields

        [Tooltip("Input Action asset to use for mapping")] public InputActionAsset inputActions;
        [Tooltip("Use a specific map")] public string useMap;

        private Dictionary<string, InputAction> actions;

        #endregion

        #region Public Methods

        [AutoDocSuppress]
        public override float GetAxis(string axisName)
        {
            if (!actions.ContainsKey(axisName)) return 0;

            return actions[axisName].ReadValue<float>();
        }

        [AutoDocSuppress]
        public override bool GetButton(string buttonName)
        {
            if (!actions.ContainsKey(buttonName)) return false;

            return actions[buttonName].IsPressed();
        }

        [AutoDocSuppress]
        public override bool GetButtonDown(string buttonName)
        {
            if (!actions.ContainsKey(buttonName)) return false;

            return actions[buttonName].WasPressedThisFrame();
        }

        [AutoDocSuppress]
        public override bool GetButtonUp(string buttonName)
        {
            if (!actions.ContainsKey(buttonName)) return false;

            return actions[buttonName].WasReleasedThisFrame();
        }

        [AutoDocSuppress]
        public override bool GetKey(KeyCode key) { return Input.GetKey(key); }

        [AutoDocSuppress]
        public override bool GetKeyDown(KeyCode key) { return Input.GetKeyDown(key); }

        [AutoDocSuppress]
        public override bool GetKeyUp(KeyCode key) { return Input.GetKeyUp(key); }

        [AutoDocSuppress]
        public override void Initialize()
        {
            if (inputActions == null)
            {
                StringExtensions.LogError(this, "Initialize", "No Input Actions supplied");
                return;
            }

            inputActions.Enable();

            actions = new Dictionary<string, InputAction>();
            foreach (InputActionMap map in inputActions.actionMaps)
            {
                if (string.IsNullOrEmpty(useMap) || useMap == map.name)
                {
                    foreach (InputAction action in map.actions)
                    {
                        actions.Add(action.name, action);
                    }

                    break;
                }
            }
        }

        #endregion

    }
}

#endif