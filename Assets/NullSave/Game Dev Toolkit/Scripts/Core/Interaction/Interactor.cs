//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace NullSave.GDTK
{
    [AutoDocLocation("interaction/interactors")]
    [AutoDoc("The Interactor component provides 3D objects with the ability to interact with the environment via raycasting.")]
    public class Interactor : MonoBehaviour
    {

        #region Enumerations

        public enum DetectionMode
        {
            Boxcast = 0,
            Raycast = 1,
            CollisionOnly = 2,
        }

        #endregion

        #region Fields

        [Tooltip("Type of detection to use")] public DetectionMode detectionMode;
        [Tooltip("Raycast layermask")] public LayerMask interactionLayer;
        [Tooltip("Offset from the source to use when raycasting")] public Vector3 emissionOffset;
        [Tooltip("Source of the raycast")] public Transform emissionSource;
        [Tooltip("Height used for cubecast")] public Vector3 halfExtends;
        [Tooltip("Maximum allowed interaction distance")] public float maxDistance;
        [Tooltip("Determines if events should also be sent to the Broadcaster")] public bool broadcastEvents;
        [Tooltip("Name of the channel on the Broadcaster to use")] public string channelName;

        [Tooltip("Event raised when encountering a new interactable")] public InteractableChanged onInteractableFound;
        [Tooltip("Event raised when target interactable is no longer available")] public UnityEvent onInteractableLost;

        private InteractableObject target;
        private InteractorUI uiInstance;

        [SerializeField] private List<InteractorComponent> m_components;
        private List<InteractableObject> collisions;

        #endregion

        #region Properties

        [AutoDoc("Get list of interactor components associated with this object")]
        public List<InteractorComponent> interactorComponents
        {
            get
            {
                if (m_components == null) m_components = new List<InteractorComponent>();
                return m_components;
            }
        }

        [AutoDoc("Get/Set currently active interactable target")]
        public InteractableObject Target
        {
            get { return target; }
            set
            {
                if (value != null && !value.interactable && !value.showAltText) value = null;

                if (target == value)
                {
                    if (value == null && uiInstance != null)
                    {
                        RemoveInteractionPrompt();
                    }
                    else if (value != null && uiInstance != null)
                    {
                        uiInstance.SetText(value.InteractionText);
                    }
                    return;
                }
                target = value;

                if (value == null)
                {
                    RemoveInteractionPrompt();
                }
                else
                {
                    value.CurrentAgent = this;

                    if (uiInstance == null)
                    {
                        if (target.customUI != null)
                        {
                            uiInstance = Instantiate(target.customUI, InterfaceManager.UICanvas.transform);
                        }
                        else if (InterfaceManager.Current.interactorPrefab != null)
                        {
                            uiInstance = Instantiate(InterfaceManager.Current.interactorPrefab, InterfaceManager.UICanvas.transform);
                        }
                    }

                    if (uiInstance != null)
                    {
                        uiInstance.source = this;
                        uiInstance.target = value;
                        uiInstance.SetText(value.InteractionText);
                        uiInstance.gameObject.SetActive(true);
                        uiInstance.Initialize();
                    }

                    InterfaceManager.PromptOpen = true;
                    onInteractableFound?.Invoke(value);
                    if (broadcastEvents)
                    {
                        Broadcaster.Broadcast(channelName, "onInteractableFound", new object[] { value });
                    }
                }
            }
        }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            // Simply called to ensure interface manager is created if not already present
            _ = InterfaceManager.Current;
            if (emissionSource == null) emissionSource = transform;
            if(halfExtends == null || halfExtends == Vector3.zero)
            {
                halfExtends = new Vector3(0.25f, 0.46f, 0.1f);
            }
            collisions = new List<InteractableObject>();
        }

        public virtual void OnDrawGizmos()
        {
            if (emissionSource == null) emissionSource = transform;

            Vector3 start = emissionSource.position + emissionOffset;
            Gizmos.color = new Color(0.1876112f, 0.7025714f, 0.8773585f);

            switch (detectionMode)
            {
                case DetectionMode.Boxcast:
                    Matrix4x4 rotationMatrix = Matrix4x4.TRS(emissionSource.position, emissionSource.rotation, emissionSource.lossyScale);
                    Gizmos.matrix = rotationMatrix;

                    Vector3 v = halfExtends * 2 + new Vector3(0, 0, maxDistance);
                    Gizmos.DrawWireCube(emissionOffset + new Vector3(0,0,maxDistance/2), v);
                    break;
                case DetectionMode.Raycast:
                    Gizmos.DrawLine(start, start + (emissionSource.forward * maxDistance));
                    break;
            }
        }

        public virtual void Reset()
        {
            interactionLayer = 1;
            emissionOffset = new Vector3(0, 0.5f, 0);
            maxDistance = 1.5f;
            channelName = name + "Interactor";
            emissionSource = transform;
            halfExtends = new Vector3(0.25f, 0.46f, 0.1f);

            InteractorComponent[] components = GetComponentsInChildren<InteractorComponent>();
            foreach (InteractorComponent c in components)
            {
                RegisterInteractorComponent(c);
            }
        }

        public virtual void Update()
        {
            if (InterfaceManager.PreventInteractions || InterfaceManager.ActiveModal != null)
            {
                if (target != null) Target = null;
                return;
            }

            foreach(var obj in collisions.ToList())
            {
                if(obj == null || obj.gameObject == null || !obj.gameObject.activeInHierarchy)
                {
                    collisions.Remove(obj);
                }
            }

            if (collisions.Count > 0)
            {
                Target = collisions[0];
            }
            else
            {
                switch (detectionMode)
                {
                    case DetectionMode.Boxcast:
                        Detection_Cubecast();
                        break;
                    case DetectionMode.Raycast:
                        Detection_Raycast();
                        break;
                }
            }

            if (Target != null)
            {
                CheckInteractionInput();
            }
        }

        #endregion

        #region Public Methods

        [AutoDoc("Register collider/trigger enter event")]
        [AutoDocParameter("Object registering event")]
        public void ColliderOrTriggerEnter(InteractableObject source)
        {
            collisions.Add(source);
        }

        [AutoDoc("Register collider/trigger exit event")]
        [AutoDocParameter("Object registering event")]
        public void ColliderOrTriggerExit(InteractableObject source)
        {
            collisions.Remove(source);
        }

        [AutoDoc("Get a interactor component of a specific type")]
        public T GetInteractorComponent<T>()
        {
            foreach (object entry in interactorComponents)
            {
                if (entry is T t)
                {
                    return t;
                }
            }

            return GetComponentInChildren<T>();
        }

        [AutoDoc("Interact with the current Target")]
        public void InteractWithTarget()
        {
            if (uiInstance != null)
            {
                uiInstance.InteractWithTarget();
                uiInstance.uiText.text = target.InteractionText;
            }
        }

        [AutoDoc("Register a new interactor component")]
        [AutoDocParameter("Component to register")]
        public void RegisterInteractorComponent(InteractorComponent component)
        {
            if (component == null) return;
            if (interactorComponents.Contains(component)) return;
            interactorComponents.Add(component);
        }

        [AutoDoc("Remove an interactor component from the list of registered components")]
        [AutoDocParameter("Component to remove")]
        public void RemoveInteractorComponent(InteractorComponent component)
        {
            interactorComponents.Remove(component);
        }

        #endregion

        #region Private Methods

        internal void CheckInteractionInput()
        {
            if (uiInstance == null)
            {
                Target = null;
                return;
            }

            if (uiInstance.requireHold)
            {
                if(!uiInstance.holding)
                {
                    switch (InterfaceManager.Current.interactionType)
                    {
                        case NavigationTypeSimple.ByButton:
                            if (InterfaceManager.Input.GetButtonDown(InterfaceManager.Current.interactionButton))
                            {
                                uiInstance.holding = true;
                                uiInstance.timeHeld = 0;
                            }
                            break;
                        case NavigationTypeSimple.ByKey:
                            if (InterfaceManager.Input.GetKeyDown(InterfaceManager.Current.interactionKey))
                            {
                                uiInstance.holding = true;
                                uiInstance.timeHeld = 0;
                            }
                            break;
                    }
                }   
                else
                {
                    switch (InterfaceManager.Current.interactionType)
                    {
                        case NavigationTypeSimple.ByButton:
                            if (InterfaceManager.Input.GetButtonUp(InterfaceManager.Current.interactionButton))
                            {
                                uiInstance.holding = false;
                                uiInstance.timeHeld = 0;
                            }
                            else
                            {
                                uiInstance.timeHeld += Time.deltaTime;
                            }
                            break;
                        case NavigationTypeSimple.ByKey:
                            if (InterfaceManager.Input.GetKeyUp(InterfaceManager.Current.interactionKey))
                            {
                                uiInstance.holding = false;
                                uiInstance.timeHeld = 0;
                            }
                            else
                            {
                                uiInstance.timeHeld += Time.deltaTime;
                            }
                            break;
                    }

                    if(uiInstance.timeHeld >= uiInstance.holdTime)
                    {
                        InteractWithTarget();
                        RemoveInteractionPrompt();
                    }
                }
            }
            else
            {
                switch (InterfaceManager.Current.interactionType)
                {
                    case NavigationTypeSimple.ByButton:
                        if (InterfaceManager.Input.GetButtonDown(InterfaceManager.Current.interactionButton))
                        {
                            InteractWithTarget();
                        }
                        break;
                    case NavigationTypeSimple.ByKey:
                        if (InterfaceManager.Input.GetKeyDown(InterfaceManager.Current.interactionKey))
                        {
                            InteractWithTarget();
                        }
                        break;
                }
            }
        }

        private void Detection_Cubecast()
        {
            Vector3 start = emissionSource.position + emissionOffset;

            if (Physics.BoxCast(start, halfExtends, emissionSource.forward, out RaycastHit hit, emissionSource.rotation, maxDistance, interactionLayer))
            {
                InteractableObject checkHit = hit.transform.gameObject.GetComponent<InteractableObject>();
                if (checkHit == null)
                {
                    InteractableChild child = hit.transform.gameObject.GetComponent<InteractableChild>();
                    if (child != null)
                    {
                        Target = child.parentInteractable;
                    }
                    else
                    {
                        Target = null;
                    }
                }
                else
                {
                    Target = checkHit;
                }
            }
            else
            {
                Target = null;
            }
        }

        private void Detection_Raycast()
        {
            Vector3 start = emissionSource.position + emissionOffset;
            if (Physics.Raycast(start, emissionSource.forward, out RaycastHit hit, maxDistance, interactionLayer))
            {
                InteractableObject checkHit = hit.transform.gameObject.GetComponent<InteractableObject>();
                if (checkHit == null)
                {
                    InteractableChild child = hit.transform.gameObject.GetComponent<InteractableChild>();
                    if (child != null)
                    {
                        Target = child.parentInteractable;
                    }
                    else
                    {
                        Target = null;
                    }
                }
                else
                {
                    Target = checkHit;
                }
            }
            else
            {
                Target = null;
            }
        }

        private void RemoveInteractionPrompt()
        {
            if (uiInstance != null)
            {
                Destroy(uiInstance.gameObject);
                uiInstance = null;
            }

            InterfaceManager.PromptOpen = false;
            onInteractableLost?.Invoke();
            if (broadcastEvents)
            {
                Broadcaster.Broadcast(channelName, "onInteractableLost");
            }
        }

        #endregion

    }
}