//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;
using UnityEngine.UI;

namespace NullSave.GDTK
{
    [AutoDocLocation("localization/components")]
    [AutoDoc("Provides localization to Unity Text objects")]
    [RequireComponent(typeof(Text))]
    public class LocalizedText : MonoBehaviour
    {

        #region Fields

        [Tooltip("Format to use for localizing text")] [TextArea(3, 10)] public string format = "[entry:entryId]";

        private Text target;

        #endregion

        #region Properties

        [AutoDoc("Gets localized text or sets raw text that is then localized")]
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
            target = GetComponent<Text>();

            Localize.onLanguageChanged.AddListener(LanguageChanged);
            if (Localize.Initialized)
            {
                LanguageChanged(Localize.CurrentLanguage);
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