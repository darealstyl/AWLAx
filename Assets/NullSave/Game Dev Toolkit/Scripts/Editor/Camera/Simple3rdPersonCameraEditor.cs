using UnityEditor;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(Simple3rdPersonCamera))]
    public class Simple3rdPersonCameraEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SimpleProperty("defaultDistance");
            SimpleProperty("maxDistance");
            SimpleProperty("minDistance");
            SimpleProperty("height");
            SimpleProperty("smoothFollow");
            SimpleProperty("smoothRotation");
            SimpleProperty("xMouseSensitivity");
            SimpleProperty("yMouseSensitivity");
            SimpleProperty("cullingHeight");
            SimpleProperty("cullingMinDist");
            SimpleProperty("stateSmoothing");
            SimpleProperty("target");
            SimpleProperty("inputHoriz");
            SimpleProperty("inputVert");
            SimpleProperty("cullingLayer");
            SimpleProperty("clipPlaneMargin");
            SimpleProperty("checkHeightRadius");

            MainContainerEnd();
        }

        #endregion

    }
}
