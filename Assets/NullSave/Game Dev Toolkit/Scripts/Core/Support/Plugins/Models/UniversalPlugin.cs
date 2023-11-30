//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System;
using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocLocation("universal-plugin-system/classes")]
    [AutoDoc("Base class for all Universal Plugins")]
    [Serializable]
    public class UniversalPlugin
    {

        #region Properties

        [AutoDoc("Icon associated with the plugin")]
        public virtual Texture2D icon { get { return GetResourceImage(null); } }

        [AutoDoc("Title of the plugin")]
        public virtual string title { get { return "Untitled Plugin"; } }

        [AutoDoc("Text to display in the titlebar for the plugin")]
        public virtual string titlebarText { get { return title; } }

        [AutoDoc("Description of the plugin")]
        public virtual string description { get { return "This plugin has not been fully completed."; } }

        #endregion

        #region Protected Methods

        protected Texture2D GetResourceImage(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return (Texture2D)Resources.Load("icons/plugin", typeof(Texture2D));
            }
            return (Texture2D)Resources.Load(path, typeof(Texture2D));
        }

        #endregion

    }
}
