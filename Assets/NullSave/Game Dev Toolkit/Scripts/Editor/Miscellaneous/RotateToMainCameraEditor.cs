using UnityEditor;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(RotateToMainCamera))]
    public class RotateToMainCameraEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SimpleProperty("offset");

            MainContainerEnd();
        }

        #endregion

    }
}
