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
    [AutoDocExcludeBase]
    [AutoDocLocation("ui")]
    [AutoDoc("This component allows the attached object to be treated as a tabstop")]
    [DefaultExecutionOrder(-160)]
    public class TabStop : Selectable, ITabStop
    {

        #region Fields

        [SerializeField] private int m_parentStopId;
        [SerializeField] private int m_tabStopId;

        [Tooltip("Event raised when the object is selected")] public UnityEvent onSelected;
        [Tooltip("Event raised when the object is deselected")] public UnityEvent onDeselected;
        private bool selected;

        #endregion

        #region Properties

        [AutoDoc("Gets the attached object")]
        public GameObject attachedObject
        {
            get { return gameObject; }
        }

        [AutoDoc("Get/Set parent tab stop id")]
        public int parentStopId
        {
            get { return m_parentStopId; }
            set { m_parentStopId = value; }
        }

        [AutoDoc("Get/Set tab stop id")]
        public int tabStopId
        {
            get { return m_tabStopId; }
            set { m_tabStopId = value; }
        }

        #endregion

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();

            ToolRegistry.RegisterComponent(this);
            if (EventSystem.current != null)
            {
                if (EventSystem.current.currentSelectedGameObject == this)
                {
                    selected = true;
                    onSelected?.Invoke();
                }
                else
                {
                    selected = false;
                    onDeselected?.Invoke();
                }
            }
        }

        private void Update()
        {
            if (!Application.isPlaying) return;

            if (EventSystem.current.currentSelectedGameObject == gameObject)
            {
                if (!selected)
                {
                    selected = true;
                    onSelected?.Invoke();
                }
            }
            else if (selected)
            {
                selected = false;
                onDeselected?.Invoke();
            }
        }

        #endregion

    }
}