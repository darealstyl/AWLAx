//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System.Collections.Generic;
using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocLocation("universal-plugin-system/plugins")]
    [AutoDoc("String Filter plugin compatible with FlexLists")]
    public class StringFilterPlugin : SortAndFilterPlugin
    {

        #region Fields

        [Tooltip("Enable sort and filter")] public bool enabled = true;
        [Tooltip("Filter")] public string filter;

        private readonly string[] compat = new string[] { typeof(string).ToString(), typeof(FlexListOption).ToString() };

        #endregion

        #region Properties

        [AutoDoc("Get an array of compatible list types")]
        public override string[] compatibleListTypes => compat;

        [AutoDoc("Get/Set filter by")]
        public override object filterBy
        {
            get { return filter; }
            set
            {
                if(value is string stringValue)
                {
                    filter = stringValue;
                    requiresUpdate?.Invoke();
                }
            }
        }

        [AutoDoc("Get/Set plugin enabled")]
        public override bool isEnabled => enabled;

        [AutoDoc("Icon associated with the plugin")]
        public override Texture2D icon { get { return GetResourceImage("icons/filter"); } }

        [AutoDoc("Title of the plugin")]
        public override string title { get { return "String Filter"; } }

        [AutoDoc("Description of the plugin")]
        public override string description { get { return "Filter string results."; } }

        #endregion

        #region Public Methods

        [AutoDoc("Sort and Filter a list based on settings")]
        [AutoDocParameter("List to sort and filter")]
        public override void SortAndFilter<T>(List<T> list)
        {
            if (string.IsNullOrEmpty(filter)) return;

            List<int> removeIndexList = new List<int>();

            if (list is List<string> stringList)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (stringList[i].IndexOf(filter, System.StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        removeIndexList.Add(i);
                    }
                }
            }
            else if (list is List<FlexListOption> flexList)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (flexList[i].text.IndexOf(filter, System.StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        removeIndexList.Add(i);
                    }
                }
            }

            // Remove last to first
            for(int i = removeIndexList.Count -1; i >= 0; i--)
            {
                list.RemoveAt(removeIndexList[i]);
            }
        }

        #endregion

    }
}