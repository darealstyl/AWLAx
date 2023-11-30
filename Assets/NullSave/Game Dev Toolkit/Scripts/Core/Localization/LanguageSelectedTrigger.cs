//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;
using UnityEngine.Events;

namespace NullSave.GDTK
{
    [AutoDocLocation("localization/components")]
    [AutoDoc("This component raises events when the language changes")]
    public class LanguageSelectedTrigger : MonoBehaviour
    {

        #region Fields

        [Tooltip("Language to wait for")] public string language;
        [Tooltip("Event raised when the current languages matches the 'language' parameter")] public UnityEvent onMatch;
        [Tooltip("Event raised when the current languages does not match the 'language' parameter")] public UnityEvent onNoMatch;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            Localize.onLanguageChanged.AddListener(LanguageChanged);
            LanguageChanged(Localize.CurrentLanguage);
        }

        #endregion

        #region Private Methods

        private void LanguageChanged(string newLanguage)
        {
            if (newLanguage == language)
            {
                onMatch?.Invoke();
            }
            else
            {
                onNoMatch?.Invoke();
            }
        }

        #endregion

    }
}