//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System.Collections.Generic;
using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocLocation("ui/data-list")]
    [AutoDoc("This component provides a DataList in a single line using TextMeshPro")]
    [DefaultExecutionOrder(-170)]
    [RequireComponent(typeof(InlineList))]
    public class DataList_InlineList : DataList
    {

        #region Fields

        private InlineList target;
        private List<string> keys;

        #endregion

        #region Properties

        [AutoDoc("Gets the selected index")]
        public override int selectedIndex
        {
            get { return target.selectedIndex; }
        }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            keys = new List<string>();
            target = GetComponent<InlineList>();
            target.onSelectionChanged.AddListener(() => onSelectionChanged?.Invoke());
        }

        #endregion

        #region Public Methods

        [AutoDoc("Add an option to the list")]
        [AutoDocParameter("Value of the option")]
        public override void AddOption(string value)
        {
            EnsureStartup();
            target.AddOption(value);
            keys.Add(value);
        }

        [AutoDoc("Add an option to the list")]
        [AutoDocParameter("Key of the option")]
        [AutoDocParameter("Value of the option")]
        public override void AddOption(string key, string value)
        {
            EnsureStartup();
            target.AddOption(value);
            keys.Add(key);
        }

        [AutoDoc("Remove all entries")]
        public override void Clear()
        {
            EnsureStartup();
            target.Clear();
        }

        [AutoDoc("Get the selected index value")]
        public override int GetSelectedIndex()
        {
            return target.selectedIndex;
        }

        [AutoDoc("Get the selected item text")]
        public override string GetSelectedItem()
        {
            return target.selectedText;
        }

        [AutoDoc("Add a list of options")]
        [AutoDocParameter("Options to add")]
        public override void AddOptions(List<string> options)
        {
            EnsureStartup();
            target.AddOptions(options);
            keys.AddRange(options);
        }

        [AutoDoc("Add a dictionary of options")]
        [AutoDocParameter("Options to add")]
        public override void AddOptions(Dictionary<string, string> options)
        {
            EnsureStartup();
            foreach (var entry in options)
            {
                target.AddOption(entry.Value);
                keys.Add(entry.Key);
            }
        }

        [AutoDoc("Set the selection")]
        [AutoDocParameter("Text of option to select")]
        public override void SetSelection(string option)
        {
            EnsureStartup();
            for (int i = 0; i < target.options.Count; i++)
            {
                if(target.options[i] == option)
                {
                    target.selectedIndex = i;
                    return;
                }
            }
        }

        #endregion

        #region Private Methods

        private void EnsureStartup()
        {
            if (target == null)
            {
                target = GetComponent<InlineList>();
                keys = new List<string>();
            }
        }

        #endregion

    }
}