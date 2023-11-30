//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDoc("Provides settings to use for localization")]
    [AutoDocLocation("localization/scriptable-objects")]
    [AutoDocExcludeBase]
    [CreateAssetMenu(menuName = "Tools/GDTK/Settings/Localization Settings", fileName = "Localization Settings")]
    public class LocalizationSettings : ScriptableObject
    {

        #region Fields

        [Tooltip("Source of localization data")] public DictionarySource sourceType;
        [Tooltip("Method used to lookup localization data")] public DictionaryLookupMode lookupMode;
        [Tooltip("Text encoding used on source data")] public TextEncoding encoding;
        [Tooltip("Default language to use")] public string defaultLanguage = "English";
        [Tooltip("Name of the asset inside of the bundle")] public string bundleAssetName = "Localize";
        [Tooltip("Name of the associated asset bundle")] public string bundleName = "Localize";
        [Tooltip("Relative file path")] public string filename = "Localize";
        [Tooltip("URL containing data")] public string bundleUrl = "https://www.nullsave.com/assetbundles/localization";

        #endregion

    }
}