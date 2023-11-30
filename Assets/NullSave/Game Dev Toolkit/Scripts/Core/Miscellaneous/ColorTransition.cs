//
// Game Developers Toolkit � 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;
using UnityEngine.UI;

namespace NullSave.GDTK
{
    [AutoDoc("Provides the ability to update colors via selected and pointer states")]
    [AutoDocLocation("miscellaneous")]
    [RequireComponent(typeof(Graphic))]
    public class ColorTransition : MonoBehaviour
    {

        #region Fields

        [Tooltip("Colors used on text component")] [SerializeField] private ColorBlock m_colors;

        private Graphic target;
        private bool m_selected;
        private bool m_pointedDown;
        private bool m_hasPointer;

        #endregion

        #region Properties

        [AutoDoc("Gets/Sets the colors to use")]
        public ColorBlock colors
        {
            get { return m_colors; }
            set
            {
                if (m_colors.Equals(value)) return;
                m_colors = value;
                UpdateState();
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    UnityEditor.EditorUtility.SetDirty(this);
                }
#endif
            }
        }

        [AutoDoc("Gets/Sets if the pointer is over the object")]
        public bool hasPointer
        {
            get { return m_hasPointer; }
            set
            {
                if (m_hasPointer == value) return;
                m_hasPointer = value;
                UpdateState();
            }
        }

        [AutoDoc("Gets/Sets if the pointer is down over the object")]
        public bool pointerDown
        {
            get { return m_pointedDown; }
            set
            {
                if (m_pointedDown == value) return;
                m_pointedDown = value;
                UpdateState();
            }
        }

        [AutoDoc("Gets/Sets if the object is selected")]
        public bool selected
        {
            get { return m_selected; }
            set
            {
                if (m_selected == value) return;
                m_selected = value;
                UpdateState();
            }
        }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            target = GetComponent<Graphic>();
            UpdateState();
        }

        private void OnEnable()
        {
            UpdateState();
        }

        private void Reset()
        {
            m_colors.colorMultiplier = 1;
            m_colors.disabledColor = new Color(0.7843137f, 0.7843137f, 0.7843137f, 0.5019608f);
            m_colors.fadeDuration = 0.1f;
            m_colors.highlightedColor = new Color(0.9607843f, 0.9607843f, 0.9607843f, 1);
            m_colors.normalColor = Color.white;
            m_colors.pressedColor = new Color(0.7843137f, 0.7843137f, 0.7843137f, 1);
            m_colors.selectedColor = new Color(0.7843137f, 0.7843137f, 0.7843137f, 1);
        }

        #endregion

        #region Private Methods

        private void DoStateTransition(UISelectionState state, bool instant)
        {
            if (colors == null) return;
            var tintColor = state switch
            {
                UISelectionState.Normal => colors.normalColor,
                UISelectionState.Highlighted => colors.highlightedColor,
                UISelectionState.Pressed => colors.pressedColor,
                UISelectionState.Disabled => colors.disabledColor,
                UISelectionState.Selected => colors.selectedColor,
                _ => Color.black,
            };
            if (gameObject.activeInHierarchy)
            {
                target.CrossFadeColor(tintColor, instant ? 0f : colors.fadeDuration, true, true);
            }
        }

        private void UpdateState()
        {
            //if (!interactable)
            //{
            //    DoStateTransition(UISelectionState.Disabled, !Application.isPlaying);
            //}
            //else
            //{
                DoStateTransition(pointerDown ? UISelectionState.Pressed : selected ? UISelectionState.Selected : hasPointer ? UISelectionState.Highlighted : UISelectionState.Normal, !Application.isPlaying);
            //}
        }


        #endregion

    }
}