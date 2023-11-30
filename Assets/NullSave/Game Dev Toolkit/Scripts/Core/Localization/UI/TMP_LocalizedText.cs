//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using TMPro;
using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocLocation("localization/components")]
    [AutoDoc("Provides localization to TextMeshProGUI objects")]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TMP_LocalizedText : MonoBehaviour
    {

        #region Fields

        [Tooltip("Format to use for localizing text")] [TextArea(3,10)] public string format = "[entry:entryId]";

        private TextMeshProUGUI target;

        #endregion

        #region Properties

        [AutoDoc("Get localized text or Set raw text to be localized")]
        public string Text
        {
            get { return target.text; }
            set
            {
                target.text = Localize.GetFormattedString(value);
            }
        }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            target = GetComponent<TextMeshProUGUI>();

            Localize.onLanguageChanged.AddListener(LanguageChanged);
            if(Localize.Initialized)
            {
                LanguageChanged(Localize.CurrentLanguage);
            }
            else
            {
                Localize.Initialize();
            }
        }

        #endregion

        #region Private Methods

        private void LanguageChanged(string newLanguage)
        {
            target.text = Localize.GetFormattedString(format);
        }

        #endregion

    }
}