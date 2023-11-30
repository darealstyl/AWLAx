//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System;

namespace NullSave.GDTK
{
    [AutoDocExcludeBase]
    [AutoDocLocation("auto-documentation/attributes")]
    [AutoDoc("Insert a YouTube video in the documentation")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class AutoDocYouTube : Attribute
    {

        #region Fields

        [AutoDoc("URL for YouTube Video (only the last section)")] public string source;
        [AutoDoc("Width of video (940 default)")] public int width;
        [AutoDoc("Height of video (529 default)")] public int height;

        #endregion

        #region Constructor

        public AutoDocYouTube(string source)
        {
            this.source = source;
            width = 940;
            height = 529;
        }

        public AutoDocYouTube(string source, int width, int height)
        {
            this.source = source;
            this.width = width;
            this.height = height;
        }

        #endregion

    }
}