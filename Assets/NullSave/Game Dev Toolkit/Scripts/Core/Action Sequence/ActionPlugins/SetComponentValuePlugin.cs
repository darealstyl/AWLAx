//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocLocation("action-sequence/built-in-plugins")]
    [AutoDoc("This plug-in allows you to set nearly any `public` value on any attached component. Therefore it should be used with care.")]
    public class SetComponentValuePlugin : ActionSequencePlugin
    {

        #region Fields

        [AutoDocAs("Remote Target", "If checked the plug-in will target the component specified as the *Remote Target* on the **Action Sequence**. Otherwise it will use the `Component` on the current Game Object.")] public bool useRemoteTarget;
        [AutoDocAs("Component", "This list is built dynamically and fetches a list of all known components. Search for and select the component you wish and press `Enter`.")] public string componentType;
        [AutoDocAs("Field", "Dynamically built list for selected component showing all `public` fields.")] public string memberName;
        [AutoDocSuppress] public string memberType;
        [AutoDocAs("Value", "Dynamic value field based on the selected **Field**.")] public string memberValue;
        [AutoDocSuppress] public string memberJson;

        #endregion

        #region Properties

        [AutoDocSuppress] public override Texture2D icon { get { return GetResourceImage("icons/wizard"); } }

        [AutoDocSuppress] public override string title { get { return "Set Component Value"; } }

        [AutoDocSuppress] public override string description { get { return "Get a component on attached GameObject and set a value."; } }

        #endregion

        #region Public Methods

        [AutoDocSuppress]
        public override void StartAction(ActionSequence host)
        {
            isStarted = true;

            // Find object
            object component = GetComponent(useRemoteTarget ? host.remoteTarget : host.gameObject);
            if (component == null)
            {
                StringExtensions.Log(host, "SetComponentValuePlugin", "Could not find component of type " + componentType);
                isComplete = true;
                return;
            }

            ActionSequenceHelper.SetMemberValue(component, memberName, memberValue, memberJson);



            isComplete = true;
        }

        #endregion

        #region Private Methods

        private object GetComponent(GameObject host)
        {
            object component = host.GetComponent(componentType);
            if (component != null) return component;
            try
            {
                component = host.GetComponent(componentType.Substring(componentType.LastIndexOf('.') + 1));
            }
            catch { }
            return component;
        }

        #endregion

    }
}