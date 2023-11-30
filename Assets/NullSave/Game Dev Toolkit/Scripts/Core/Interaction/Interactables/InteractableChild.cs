//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocLocation("interaction/interactables")]
    [AutoDoc("The Interactable Child component enables multiple interactable objects to be linked or the child of an interactable object to trigger its parent.")]
    [RequireComponent(typeof(Collider))]
    public class InteractableChild : MonoBehaviour
    {

        #region Fields

        [Tooltip("Parent interactable obejct")] public InteractableObject parentInteractable;

        #endregion

        #region Properties

        [AutoDoc("Returns the OcclusionPortal (if any) associated with the object")]
        public OcclusionPortal OcclusionPortal { get; private set; }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            OcclusionPortal = GetComponent<OcclusionPortal>();
        }

        #endregion

    }
}