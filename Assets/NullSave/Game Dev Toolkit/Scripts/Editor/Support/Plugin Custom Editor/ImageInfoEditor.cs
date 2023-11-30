using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace NullSave.GDTK
{
    [CustomUniversalPluginEditor(typeof(ImageInfo))]
    public class ImageInfoEditor : UniversalPluginEditor
    {

        #region Public Methods

        public override void OnEnable()
        {
            ((ImageInfo)referencedObject).LoadImage();
        }

        public override void OnInspectorGUI()
        {
            Sprite sprite = (Sprite)PropertyObjectValue("sprite");
            PropertyField("sprite");
            Sprite checkGo = (Sprite)PropertyObjectValue("sprite");
            if (sprite != checkGo)
                if (sprite != checkGo)
                {
                    if (checkGo == null)
                    {
                        PropertyIntValue("source", (int)ImageSource.Resources);
                        PropertyStringValue("bundleName", null);
                        PropertyStringValue("path", null);
                        PropertyStringValue("assetName", null);
                        PropertyBoolValue("z_imageError", false);
                    }
                    else
                    {
                        string assetPath = AssetDatabase.GetAssetPath(checkGo);
                        string bundlePath = string.IsNullOrEmpty(assetPath) ? string.Empty : AssetDatabase.GetImplicitAssetBundleName(assetPath);
                        if (!string.IsNullOrEmpty(bundlePath))
                        {
                            PropertyIntValue("source", (int)ImageSource.AssetBundle);
                            PropertyStringValue("bundleName", bundlePath);
                            PropertyStringValue("path", assetPath);
                            PropertyStringValue("assetName", checkGo.name);
                            PropertyBoolValue("z_imageError", false);
                        }
                        else if (!string.IsNullOrEmpty(assetPath))
                        {
                            if (assetPath.IndexOf("resources", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                PropertyIntValue("source", (int)ImageSource.Resources);
                                PropertyStringValue("bundleName", null);
                                PropertyStringValue("path", Path.Combine(Path.GetDirectoryName(assetPath.Substring(assetPath.LastIndexOf("resources", StringComparison.OrdinalIgnoreCase) + 10)), checkGo.name));
                                PropertyStringValue("assetName", null);
                                PropertyBoolValue("z_imageError", false);
                            }
                            else
                            {
                                PropertyBoolValue("z_imageError", true);
                            }
                        }
                        else if (checkGo == null)
                        {
                            PropertyIntValue("source", (int)ImageSource.Resources);
                            PropertyStringValue("bundleName", null);
                            PropertyStringValue("path", null);
                            PropertyStringValue("assetName", null);
                            PropertyBoolValue("z_imageError", false);
                        }
                        else
                        {
                            PropertyBoolValue("z_imageError", true);
                        }
                    }
                }

            if (PropertyBoolValue("z_imageError"))
            {
                GUILayout.Label("Must be in Resources or an Asset Bundle", GDTKEditor.Styles.ErrorTextStyle);
            }
        }

        #endregion

    }
}