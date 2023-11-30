//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;
using UnityEngine.Events;

namespace NullSave.GDTK
{
    [AutoDocLocation("interaction/interactables")]
    [AutoDoc("The Interactable Object component enables an object to be targeted and interacted with.")]
    public class InteractableObject : MonoBehaviour
    {

        #region Fields

        [Tooltip("Determines if this object is interactable")] public bool interactable;
        [Tooltip("Text associated with this action")] public string actionText;
        [Tooltip("Image associated with the action")] public Sprite actionImage;

        [Tooltip("Display message when object is not interactable")] public bool showAltText;
        [Tooltip("Text associated with this action when not interactable")] public string alternateText;

        [Tooltip("Custom UI to use instead of the normal Interactor UI")] public InteractorUI customUI;

        [Tooltip("Name of the broadcaster channel to use with the audio ppol")] public string audioPoolChannel;
        [Tooltip("Sound to play when interacting")] public AudioClip actionSound;

        [Tooltip("Event raised when the object is interacted with")] public UnityEvent onInteract;

        #endregion

        #region Properties

        [AutoDoc("Currently active Interactor")]
        public virtual Interactor CurrentAgent { get; set; }

        [AutoDoc("Image to display in the UI")]
        public virtual Sprite InteractionImage
        {
            get { return actionImage; }
        }

        [AutoDoc("Text to display in the UI")]
        public virtual string InteractionText
        {
            get
            {
                if (!IsInteractable && showAltText)
                {
                    return alternateText;
                }
                return actionText;
            }
        }

        [AutoDoc("Is the object currently interactable")]
        public virtual bool IsInteractable
        {
            get
            {
                return interactable;
            }
            set
            {
                interactable = value;
            }
        }

        #endregion

        #region Unity Methods

        private void OnCollisionEnter(Collision collision)
        {
            Interactor target = collision.gameObject.GetComponentInChildren<Interactor>();
            if (target == null) return;
            target.ColliderOrTriggerEnter(this);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Interactor2D target = collision.gameObject.GetComponentInChildren<Interactor2D>();
            if (target == null) return;
            target.ColliderOrTriggerEnter(this);
        }

        private void OnCollisionExit(Collision collision)
        {
            Interactor target = collision.gameObject.GetComponentInChildren<Interactor>();
            if (target == null) return;
            target.ColliderOrTriggerExit(this);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            Interactor2D target = collision.gameObject.GetComponentInChildren<Interactor2D>();
            if (target == null) return;
            target.ColliderOrTriggerExit(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            Interactor target = other.gameObject.GetComponentInChildren<Interactor>();
            if (target == null) return;
            target.ColliderOrTriggerEnter(this);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Interactor2D target = collision.gameObject.GetComponentInChildren<Interactor2D>();
            if (target == null) return;
            target.ColliderOrTriggerEnter(this);
        }

        private void OnTriggerExit(Collider other)
        {
            Interactor target = other.gameObject.GetComponentInChildren<Interactor>();
            if (target == null) return;
            target.ColliderOrTriggerExit(this);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Interactor2D target = collision.gameObject.GetComponentInChildren<Interactor2D>();
            if (target == null) return;
            target.ColliderOrTriggerExit(this);
        }

        public virtual void Reset()
        {
            interactable = true;
        }

        #endregion

        #region Public Methods

        [AutoDoc("Interact with the object")]
        [AutoDocParameter("Interactor requesting action")]
        public virtual bool Interact(Interactor source)
        {
            if (!IsInteractable) return false;

            Broadcaster.Broadcast(audioPoolChannel, "Play", new object[] { actionSound, transform.position });
            onInteract?.Invoke();
            return true;
        }

        [AutoDoc("Set the interactable state of the object")]
        [AutoDocParameter("Interactable state")]
        public void SetInteractable(bool canInteract)
        {
            IsInteractable = canInteract;
        }

        #endregion

    }
}