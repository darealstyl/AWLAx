using UnityEditor;
using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocSuppress]
    public class EditorInfoItem
    {

        #region Fields

        public bool isDragging;
        public float currentY;
        public Editor editor;
        public bool isExpanded;
        public Rect rect;

        #endregion

    }
}