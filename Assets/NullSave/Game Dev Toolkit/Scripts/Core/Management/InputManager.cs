//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocExcludeBase]
    [AutoDocLocation("input-managers")]
    [AutoDoc("InputManager is a ScriptableObject that is used as a base class to define different managers for handling input for your system. Two managers come built-in, UnityInput and RewiredInput. You can assign an Input Manager on the Interface Manager component.")]
    public class InputManager : ScriptableObject
    {

        #region Public Methods

        [AutoDoc("Returns true during the frame the user starts pressing down the key identified by name")]
        [AutoDocParameter("Name of axis to read")]
        public virtual float GetAxis(string axisName) { throw new System.NotImplementedException(); }

        [AutoDoc("Returns true during the frame the user starts pressing down the key identified by name")]
        [AutoDocParameter("Name of axis to read")]
        [AutoDocParameter("Id of player to check")]
        public virtual float GetAxis(string axisName, int playerId) { throw new System.NotImplementedException(); }

        [AutoDoc("True when an axis has been pressed and not released")]
        [AutoDocParameter("Name of button to read")]
        public virtual bool GetButton(string buttonName) { throw new System.NotImplementedException(); }

        [AutoDoc("True when an axis has been pressed and not released")]
        [AutoDocParameter("Name of button to read")]
        [AutoDocParameter("Id of player to check")]
        public virtual bool GetButton(string buttonName, int playerId) { throw new System.NotImplementedException(); }

        [AutoDoc("Returns true during the frame the user pressed down the virtual button identified by buttonName")]
        [AutoDocParameter("Name of button to read")]
        public virtual bool GetButtonDown(string buttonName) { throw new System.NotImplementedException(); }

        [AutoDoc("Returns true during the frame the user pressed down the virtual button identified by buttonName")]
        [AutoDocParameter("Name of button to read")]
        [AutoDocParameter("Id of player to check")]
        public virtual bool GetButtonDown(string buttonName, int playerId) { throw new System.NotImplementedException(); }

        [AutoDoc("Returns true the first frame the user releases the virtual button identified by buttonName")]
        [AutoDocParameter("Name of button to read")]
        public virtual bool GetButtonUp(string buttonName) { throw new System.NotImplementedException(); }

        [AutoDoc("Returns true the first frame the user releases the virtual button identified by buttonName")]
        [AutoDocParameter("Name of button to read")]
        [AutoDocParameter("Id of player to check")]
        public virtual bool GetButtonUp(string buttonName, int playerId) { throw new System.NotImplementedException(); }

        [AutoDoc("Returns true while the user holds down the key identified by name")]
        [AutoDocParameter("Key to read")]
        public virtual bool GetKey(KeyCode key) { throw new System.NotImplementedException(); }

        [AutoDoc("Returns true while the user holds down the key identified by name")]
        [AutoDocParameter("Key to read")]
        [AutoDocParameter("Id of player to check")]
        public virtual bool GetKey(string key, int playerId) { throw new System.NotImplementedException(); }

        [AutoDoc("Returns true during the frame the user starts pressing down the key identified by name")]
        [AutoDocParameter("Key to read")]
        public virtual bool GetKeyDown(KeyCode key) { throw new System.NotImplementedException(); }

        [AutoDoc("Returns true during the frame the user starts pressing down the key identified by name")]
        [AutoDocParameter("Key to read")]
        [AutoDocParameter("Id of player to check")]
        public virtual bool GetKeyDown(string key, int playerId) { throw new System.NotImplementedException(); }

        [AutoDoc("Returns true during the frame the user releases the key identified by name")]
        [AutoDocParameter("Key to read")]
        public virtual bool GetKeyUp(KeyCode key) { throw new System.NotImplementedException(); }

        [AutoDoc("Returns true during the frame the user releases the key identified by name")]
        [AutoDocParameter("Key to read")]
        [AutoDocParameter("Id of player to check")]
        public virtual bool GetKeyUp(string key, int playerId) { throw new System.NotImplementedException(); }

        [AutoDoc("Initialize the manager")]
        public virtual void Initialize() { }

        #endregion

    }
}