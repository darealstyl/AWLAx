//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NullSave.GDTK
{
    [AutoDoc("Item that can be added to a FlexList")]
    [AutoDocLocation("ui")]
    public class FlexListItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {

        #region Fields

        [Tooltip("Image component")] public Image image;
        [Tooltip("Label component")] public Label label;

        [Tooltip("Background graphic")] public Graphic background;
        [Tooltip("Colors used on text component")] [SerializeField] private ColorBlock m_colors;

        [Tooltip("Event raised when item is selected")] public UnityEvent onSelected;
        [Tooltip("Event raised when item is deselected")] public UnityEvent onDeselected;
        [Tooltip("Event raised when item is clicked")] public UnityEvent onClick;

        private bool m_selected;

        #endregion

        #region Properties

        [AutoDoc("Get/Set the colors")]
        public ColorBlock colors
        {
            get { return m_colors; }
            set
            {
                m_colors = value;
                UpdateState();
            }
        }

        [AutoDoc("Gets if the pointer is over the item")]
        public bool hasPointer { get; private set; }

        [AutoDoc("Gets if the pointer is down over the item")]
        public bool pointerDown { get; private set; }

        [AutoDoc("Get/Set if the item is selected")]
        public virtual bool selected
        {
            get { return m_selected; }
            set
            {
                if (m_selected == value) return;
                m_selected = value;
                if (m_selected)
                {
                    onSelected?.Invoke();
                }
                else
                {
                    onDeselected?.Invoke();
                }
                UpdateState();
            }
        }

        #endregion

        #region Unity Methods

        [AutoDocSuppress]
        public void OnPointerClick(PointerEventData eventData)
        {
            onClick?.Invoke();
        }

        [AutoDocSuppress]
        public void OnPointerDown(PointerEventData eventData)
        {
            pointerDown = true;
            UpdateState();
        }

        [AutoDocSuppress]
        public void OnPointerEnter(PointerEventData eventData)
        {
            hasPointer = true;
            UpdateState();
        }

        [AutoDocSuppress]
        public void OnPointerExit(PointerEventData eventData)
        {
            hasPointer = false;
            UpdateState();
        }

        [AutoDocSuppress]
        public void OnPointerUp(PointerEventData eventData)
        {
            pointerDown = false;
            UpdateState();
        }

        private void Reset()
        {
            background = GetComponent<Graphic>();
            m_colors = new ColorBlock();
            m_colors.colorMultiplier = 1;
            m_colors.normalColor = Color.white;
            m_colors.highlightedColor = new Color(0.9607843f, 0.9607843f, 0.9607843f);
            m_colors.pressedColor = new Color(0.7843137f, 0.7843137f, 0.7843137f);
            m_colors.selectedColor = new Color(0, 0.4734311f, 1);
            m_colors.disabledColor = new Color(0.7843137f, 0.7843137f, 0.7843137f, 0.5019608f);
            m_colors.fadeDuration = 0.1f;
        }

        #endregion

        #region Private Methods

        protected virtual void DoStateTransition(UISelectionState state, bool instant)
        {
            if (colors == null || background == null) return;

            var tintColor = state switch
            {
                UISelectionState.Normal => colors.normalColor,
                UISelectionState.Highlighted => colors.highlightedColor,
                UISelectionState.Pressed => colors.pressedColor,
                UISelectionState.Disabled => colors.disabledColor,
                UISelectionState.Selected => colors.selectedColor,
                _ => Color.black,
            };

            background.CrossFadeColor(tintColor, instant ? 0f : colors.fadeDuration, true, true);
        }

        private void UpdateState()
        {
            DoStateTransition(pointerDown ? UISelectionState.Pressed : selected ? UISelectionState.Selected : hasPointer ? UISelectionState.Highlighted : UISelectionState.Normal, !Application.isPlaying);
        }

        #endregion

    }
}
