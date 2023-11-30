//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System;
using UnityEngine;
using UnityEngine.Events;

namespace NullSave.GDTK
{

    [AutoDocSuppress]
    [Serializable]
    public class GameObjectEvent : UnityEvent<GameObject> { }

    [AutoDocSuppress]
    [Serializable]
    public class LanguageChanged : UnityEvent<string> { }

    [AutoDocSuppress]
    [Serializable]
    public class SelectedIndexChanged : UnityEvent<int> { }

    [AutoDocSuppress]
    [Serializable]
    public delegate void SimpleEvent();

    [AutoDocSuppress]
    [Serializable]
    public delegate void TooltipEvent(TooltipDisplay display);

    [AutoDocSuppress]
    [Serializable]
    public class StateChanged : UnityEvent<bool> { }

    [AutoDocSuppress]
    [Serializable]
    public class ValueChanged : UnityEvent<float> { }

    [AutoDocSuppress]
    [Serializable]
    public class InteractableChanged: UnityEvent<InteractableObject> { }

    [AutoDocSuppress]
    [Serializable]
    public delegate void SequenceComplete(int index);

    [AutoDocSuppress]
    [Serializable]
    public class MenuItemEvent : UnityEvent<UIMenuItem> { }

    [AutoDocSuppress]
    [Serializable]
    public class MenuItemHoverStateChanged : UnityEvent<UIMenuItem, bool> { }

    [AutoDocSuppress]
    [Serializable]
    public class MenuItemInteractableChanged : UnityEvent<UIMenuItem, bool> { }

    [AutoDocSuppress]
    [Serializable]
    public class ToggleEvent : UnityEvent<bool> { }

    [AutoDocSuppress]
    [Serializable]
    public delegate void ValueChange(float oldValue, float newValue);

}