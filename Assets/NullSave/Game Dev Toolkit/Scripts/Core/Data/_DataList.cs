//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NullSave.GDTK
{
    [AutoDocSuppress]
    public class DataList : MonoBehaviour
    {

        #region Fields

        [Tooltip("Event raised whenever selection changes")] public UnityEvent onSelectionChanged;

        #endregion

        #region Properties

        [AutoDoc("Gets the selected index")]
        public virtual int selectedIndex { get { throw new System.NotImplementedException(); } }

        [AutoDoc("Gets the selected key")]
        public virtual string selectedKey { get { throw new System.NotImplementedException(); } }

        #endregion

        #region Public Methods

        [AutoDoc("Add an option to the list")]
        [AutoDocParameter("Value of the option")]
        public virtual void AddOption(string value) { throw new System.NotImplementedException(); }

        [AutoDoc("Add an option to the list")]
        [AutoDocParameter("Key of the option")]
        [AutoDocParameter("Value of the option")]
        public virtual void AddOption(string key, string value) { throw new System.NotImplementedException(); }

        [AutoDoc("Add a list of options")]
        [AutoDocParameter("Options to add")]
        public virtual void AddOptions(List<string> options) { throw new System.NotImplementedException(); }

        [AutoDoc("Add a dictionary of options")]
        [AutoDocParameter("Options to add")]
        public virtual void AddOptions(Dictionary<string, string> options) { throw new System.NotImplementedException(); }

        [AutoDoc("Remove all entries")]
        public virtual void Clear() { throw new System.NotImplementedException(); }

        [AutoDoc("Get the selected index value")]
        public virtual int GetSelectedIndex() { throw new System.NotImplementedException(); }

        [AutoDoc("Get the selected item text")]
        public virtual string GetSelectedItem() { throw new System.NotImplementedException(); }

        [AutoDoc("Set the selection")]
        [AutoDocParameter("Text of option to select")]
        public virtual void SetSelection(string option) { throw new System.NotImplementedException(); }

        #endregion

    }
}