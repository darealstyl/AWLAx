using UnityEditor;
using UnityEngine;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(ToolRegistryHelper))]
    public class ToolRegistryHelperEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SectionHeader("Register Items", "registerItems", typeof(Object));
            SimpleList("registerItems", true);

            MainContainerEnd();
        }

        #endregion

    }
}