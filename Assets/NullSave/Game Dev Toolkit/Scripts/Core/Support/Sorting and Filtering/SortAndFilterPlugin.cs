//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System.Collections.Generic;
using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocLocation("universal-plugin-system/classes")]
    [AutoDoc("Specialized plugin for sorting and filtering")]
    public class SortAndFilterPlugin : UniversalPlugin
    {

        #region Fields

        [AutoDoc("Event raised when a display update is needed")] [HideInInspector] public SimpleEvent requiresUpdate;

        #endregion

        #region Properties

        [AutoDoc("Get an array of compatible list types")]
        public virtual string[] compatibleListTypes { get; }

        [AutoDoc("Get/Set filter by")]
        public virtual object filterBy { get; set; }

        [AutoDoc("Gets the enabled state of the plugin")]
        public virtual bool isEnabled { get; }

        [AutoDoc("Get/Set sort by")]
        public virtual object sortBy { get; set; }

        #endregion

        #region Public Methods

        [AutoDoc("Sort and Filter a list based on settings")]
        [AutoDocParameter("List to sort and filter")]
        public virtual void SortAndFilter<T>(List<T> list) { }

        #endregion

    }
}