//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocLocation("ui/data-list")]
    [AutoDoc("This component provides a DataList dropdown using TextMeshPro")]
    [DefaultExecutionOrder(-170)]
    [RequireComponent(typeof(TMP_Dropdown))]
    public class DataList_DropdownTMP : DataList
    {

        #region Fields

        private TMP_Dropdown target;
        private List<string> keys;

        #endregion

        #region Properties

        [AutoDoc("Gets the selected index")]
        public override int selectedIndex
        {
            get { return target.value; }
        }

        [AutoDoc("Gets the selected key")]
        public override string selectedKey
        {
            get
            {
                if (target.value >= keys.Count) return string.Empty;
                return keys[target.value];
            }
        }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            keys = new List<string>();
            target = GetComponent<TMP_Dropdown>();
            target.onValueChanged.AddListener((int value) => onSelectionChanged?.Invoke());
        }

        #endregion

        #region Public Methods

        [AutoDoc("Add an option to the list")]
        [AutoDocParameter("Value of the option")]
        public override void AddOption(string value)
        {
            EnsureStartup();
            target.options.Add(new TMP_Dropdown.OptionData { text = value });
            keys.Add(value);
        }

        [AutoDoc("Add an option to the list")]
        [AutoDocParameter("Key of the option")]
        [AutoDocParameter("Value of the option")]
        public override void AddOption(string key, string value)
        {
            EnsureStartup();
            target.options.Add(new TMP_Dropdown.OptionData { text = value });
            keys.Add(key);
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
                target.options.Add(new TMP_Dropdown.OptionData { text = entry.Value });
                keys.Add(entry.Key);
            }
        }

        [AutoDoc("Remove all entries")]
        public override void Clear()
        {
            EnsureStartup();
            target.ClearOptions();
        }

        [AutoDoc("Get the selected index value")]
        public override int GetSelectedIndex()
        {
            return target.value;
        }

        [AutoDoc("Get the selected item text")]
        public override string GetSelectedItem()
        {
            return target.options[target.value].text;
        }

        [AutoDoc("Set the selection")]
        [AutoDocParameter("Text of option to select")]
        public override void SetSelection(string option)
        {
            EnsureStartup();
            for (int i = 0; i < target.options.Count; i++)
            {
                if (target.options[i].text == option)
                {
                    target.value = i;
                    return;
                }
            }
        }

        #endregion

        #region Private Methods

        private void EnsureStartup()
        {
            if(target == null)
            {
                target = GetComponent<TMP_Dropdown>();
                keys = new List<string>();
            }
        }

        #endregion

    }
}