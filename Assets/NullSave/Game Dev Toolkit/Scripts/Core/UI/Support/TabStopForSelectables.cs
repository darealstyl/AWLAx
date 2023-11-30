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
    [AutoDocLocation("ui")]
    [AutoDoc("This component allows the attached selectable object to be treated as a tabstop")]
    [RequireComponent(typeof(Selectable))]
    [DefaultExecutionOrder(-160)]
    public class TabStopForSelectables : MonoBehaviour, ITabStop
    {

        #region Fields

        [SerializeField] private int m_parentStopId;
        [SerializeField] private int m_tabStopId;

        [Tooltip("Event raised when the object is selected")] public UnityEvent onSelected;
        [Tooltip("Event raised when the object is deselected")] public UnityEvent onDeselected;

        private bool selected;
        private Selectable target;

        #endregion

        #region Properties

        [AutoDoc("Gets the attached object")]
        public GameObject attachedObject
        {
            get
            {
                if (gameObject == null) return null;
                return gameObject;
            }
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

        protected void Awake()
        {
            target = GetComponent<Selectable>();
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

        private void OnDisable()
        {
            ToolRegistry.RemoveComponent(this);
        }

        private void OnEnable()
        {
            ToolRegistry.RegisterComponent(this);
        }

        protected void Reset()
        {
            m_parentStopId = 0;
            m_tabStopId = -1;

            ITabStop tabStop;
            int highestIndex = -1;
            foreach (Component go in FindObjectsOfType<Component>())
            {
                if (go is ITabStop stop && go != this)
                {
                    tabStop = stop;
                    if (tabStop.parentStopId == parentStopId)
                    {
                        if (tabStop.tabStopId > highestIndex)
                        {
                            highestIndex = tabStop.tabStopId;
                        }
                    }
                }
            }

            tabStopId = highestIndex + 1;
        }

        private void Update()
        {
            if (!Application.isPlaying) return;

            if (EventSystem.current.currentSelectedGameObject == target.gameObject)
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