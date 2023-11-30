//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System.Collections.Generic;
using UnityEngine;
using static NullSave.GDTK.FlexList;

namespace NullSave.GDTK
{
    [AutoDocLocation("universal-plugin-system/plugins")]
    [AutoDoc("Sort and Filter plugin compatible with FlexLists")]
    public class FlexListSortPlugin : SortAndFilterPlugin
    {

        #region Fields

        [Tooltip("Enable sort and filter")] public bool enabled = true;
        [Tooltip("Sort mode")] public SortMode sortMode;

        private readonly string[] compat = new string[] { typeof(FlexListOption).ToString() };

        #endregion

        #region Properties

        [AutoDoc("Get an array of compatible list types")]
        public override string[] compatibleListTypes => compat;

        [AutoDoc("Get/Set sort by")]
        public override object sortBy
        {
            get { return sortMode; }
            set
            {
                if (value is SortMode useValue)
                {
                    sortMode = useValue;
                    requiresUpdate?.Invoke();
                }
            }
        }

        [AutoDoc("Get/Set plugin enabled")]
        public override bool isEnabled => enabled;

        [AutoDoc("Title of the plugin")]
        public override string title { get { return "FlexList Sort"; } }

        [AutoDoc("Description of the plugin")]
        public override string description { get { return "Sort items in FlexList."; } }

        #endregion

        #region Public Methods

        [AutoDoc("Sort and Filter a list based on settings")]
        [AutoDocParameter("List to sort and filter")]
        public override void SortAndFilter<T>(List<T> list)
        {
            if (list is List<FlexListOption> typedList)
            {
                switch (sortMode)
                {
                    case SortMode.StringAsc:
                        typedList.Sort(new StringSortAsc());
                        break;
                    case SortMode.StringDesc:
                        typedList.Sort(new StringSortDesc());
                        break;
                    case SortMode.HasImageAsc:
                        typedList.Sort(new HasImageSortAsc());
                        break;
                    case SortMode.HasImageDesc:
                        typedList.Sort(new HasImageSortDesc());
                        break;
                }

            }

        }

        #endregion

        #region Comparer Classes

        private class HasImageSortAsc : IComparer<FlexListOption>
        {
            public int Compare(FlexListOption c1, FlexListOption c2) { return (c2.image != null).CompareTo(c1.image != null); }
        }

        private class HasImageSortDesc : IComparer<FlexListOption>
        {
            public int Compare(FlexListOption c1, FlexListOption c2) { return (c1.image != null).CompareTo(c2.image != null); }
        }

        private class StringSortAsc : IComparer<FlexListOption>
        {
            public int Compare(FlexListOption c1, FlexListOption c2) { return c2.text.CompareTo(c1.text); }
        }

        private class StringSortDesc : IComparer<FlexListOption>
        {
            public int Compare(FlexListOption c1, FlexListOption c2) { return c1.text.CompareTo(c2.text); }
        }

        #endregion


    }
}