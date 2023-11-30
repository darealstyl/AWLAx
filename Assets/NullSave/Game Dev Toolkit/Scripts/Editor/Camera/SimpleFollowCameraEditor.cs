using UnityEditor;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(SimpleFollowCamera))]
    public class SimpleFollowCameraEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SimpleProperty("target");
            SimpleProperty("positionOffset");
            SimpleProperty("lookAtOffset");

            MainContainerEnd();
        }

        #endregion

    }
}