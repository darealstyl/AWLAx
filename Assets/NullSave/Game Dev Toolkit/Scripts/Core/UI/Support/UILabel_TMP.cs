//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace NullSave.GDTK
{
    [AutoDocLocation("ui")]
    [AutoDoc("Component used to display text that can automatically be localized.")]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UILabel_TMP : UILabel
    {

        #region Enumerations

        public enum AutoSizeMode
        {
            None,
            Vertically,
            Horizontally
        }

        #endregion

        #region Fields

        [SerializeField, TextArea(2, 6)][Tooltip("Text to display")] private string m_Text;
        [Tooltip("Auto-size mode")] public AutoSizeMode autoSize;
        [Tooltip("Localize text")] public bool localize;

        [Tooltip("Event raised whenever control is resized")] public UnityEvent onResized;

        [Tooltip("Component used to display text.")] public TextMeshProUGUI target;
        private RectTransform RectTransform;

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
            get { return textMeshPro.text; }
            set
            {
                if (m_Text == value) return;
                m_Text = value;

                if (localize)
                {
                    textMeshPro.text = Localize.GetFormattedString(value);
                    onTextChanged?.Invoke();
                }
                else
                {
                    textMeshPro.text = m_Text;
                    onTextChanged?.Invoke();
                }

                UpdateSizing();
            }
        }

        [AutoDoc("Component used to display text.")]
        public TextMeshProUGUI textMeshPro
        {
            get
            {
                if (target == null)
                {
                    target = GetComponent<TextMeshProUGUI>();
                }
                return target;
            }
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

        private void Start()
        {
            UpdateSizing();
        }

        private void Reset()
        {
            target = GetComponent<TextMeshProUGUI>();
        }

        #endregion

        #region Private Methods

        private void AutoSizeH()
        {
            if (RectTransform == null) RectTransform = GetComponent<RectTransform>();

            float width = Mathf.Max(RectTransform.rect.width, RectTransform.sizeDelta.x);
            if (width == 0)
            {
                StartCoroutine(DeferredAutoSize());
                return;
            }
            float height = Mathf.Max(RectTransform.rect.height, RectTransform.sizeDelta.y);

            Vector2 sizeTo = target.GetPreferredValues(float.PositiveInfinity, height);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sizeTo.x);

            onResized?.Invoke();
        }

        private void AutoSizeV()
        {
            if (RectTransform == null) RectTransform = GetComponent<RectTransform>();

            float width = Mathf.Max(RectTransform.rect.width, RectTransform.sizeDelta.x);
            if (width == 0)
            {
                StartCoroutine(DeferredAutoSize());
                return;
            }
            Vector2 sizeTo = target.GetPreferredValues(text, width, float.PositiveInfinity);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sizeTo.y);

            onResized?.Invoke();
        }

        private IEnumerator DeferredAutoSize()
        {
            float width = 0;
            while (width == 0)
            {
                width = Mathf.Max(RectTransform.rect.width, RectTransform.sizeDelta.x);
                yield return null;
            }

            UpdateSizing();
        }

        private void LanguageChanged(string newLanguage)
        {
            if (!localize) return;

            textMeshPro.text = Localize.GetFormattedString(m_Text);
        }

        private void UpdateSizing()
        {
            switch (autoSize)
            {
                case AutoSizeMode.Horizontally:
                    AutoSizeH();
                    break;
                case AutoSizeMode.Vertically:
                    AutoSizeV();
                    break;

            }
        }

        #endregion

    }
}