//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace NullSave.GDTK
{
    [AutoDocLocation("ui")]
    [AutoDoc("Component used to display text that can automatically be localized.")]
    [RequireComponent(typeof(Text))]
    public class UILabel_UnityText : UILabel
    {

        #region Fields

        [SerializeField, TextArea(2, 6)][Tooltip("Text to display")] private string m_Text;
        [Tooltip("Localize text")] public bool localize;

        [Tooltip("Event raised whenever control is resized")] public UnityEvent onResized;

        [Tooltip("Component used to display text.")] public Text target;

        #endregion

        #region Properties

        [AutoDoc("Color to apply to text.")]
        public override Color color
        {
            get { return target.color; }
            set { target.color = value; }
        }

        [AutoDoc("Get/Set control text. If localize is selected text will automatically be localized.")]
        public override string text
        {
            get { return target.text; }
            set
            {
                if (m_Text == value) return;
                m_Text = value;

                if (localize)
                {
                    target.text = Localize.GetFormattedString(value);
                    onTextChanged?.Invoke();
                }
                else
                {
                    target.text = m_Text;
                    onTextChanged?.Invoke();
                }
            }
        }

        [AutoDoc("Component used to display text.")]
        public Text unityText
        {
            get { return target; }
        }

        [AutoDoc("Text supplied without localization.")]
        public string unlocalizedText
        {
            get { return m_Text; }
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
            else
            {
                Localize.Initialize();
            }
        }

        private void Reset()
        {
            target = GetComponent<Text>();
        }

        #endregion

        #region Private Methods

        private void LanguageChanged(string newLanguage)
        {
            if (!localize) return;

            target.text = Localize.GetFormattedString(m_Text);
        }

        #endregion

    }
}