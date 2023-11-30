//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;
using UnityEngine.Events;

namespace NullSave.GDTK
{
    [AutoDocLocation("interaction/interactables")]
    [AutoDoc("The Interactable Switch component enables a switch to be turned on/off.")]
    public class InteractableSwitch : InteractableObject
    {

        #region Fields

        [Tooltip("Boolean parameter to set on the animator")] public string onBoolAnim;
        [Tooltip("Determines if the switch is on")] [SerializeField] private bool isOn;

        [Tooltip("Text to display when the switch can be activated")] public string activateText;
        [Tooltip("Text to displayw hen the switch can be deactivated")] public string deactivateText;

        [Tooltip("Event raised when the switch is activated")] public UnityEvent onSwitchOn;
        [Tooltip("Event raised when the switch is deactivated")] public UnityEvent onSwitchOff;

        #endregion

        #region Properties

        [AutoDoc("Attached Animator")]
        public Animator Animator { get; private set; }

        [AutoDoc("Text to display in the UI")]
        public override string InteractionText
        {
            get
            {
                if (isOn)
                {
                    return deactivateText;
                }
                return activateText;
            }
        }

        [AutoDoc("Is the switch active")]
        public bool IsOn
        {
            get { return isOn; }
            set
            {
                if (IsOn == value) return;

                isOn = value;
                if (Animator != null)
                {
                    Animator.SetBool(onBoolAnim, value);
                }

                RaiseEvents();
            }
        }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            Animator = GetComponent<Animator>();
        }

        public override void Reset()
        {
            base.Reset();
            onBoolAnim = "On";
        }

        private void Start()
        {
            RaiseEvents(false);
        }

        #endregion

        #region Public Methods

        [AutoDoc("Interact with the object")]
        [AutoDocParameter("Interactor requesting action")]
        public override bool Interact(Interactor source)
        {
            if (!IsInteractable) return false;
            IsOn = !isOn;
            onInteract?.Invoke();
            return true;
        }

        #endregion

        #region Private Methods

        private void RaiseEvents(bool includeSounds = true)
        {
            if (IsOn)
            {
                onSwitchOn?.Invoke();
            }
            else
            {
                onSwitchOff?.Invoke();
            }

            if (includeSounds)
            {
                Broadcaster.Broadcast(audioPoolChannel, "Play", new object[] { actionSound, transform.position });
            }
        }

        #endregion

    }
}