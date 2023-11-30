using UnityEditor;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(LabelParentResizer))]
    [CanEditMultipleObjects]
    public class LabelParentResizerEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SimpleProperty("label");
            SimpleProperty("padding");

            MainContainerEnd();
        }

        #endregion

    }
}