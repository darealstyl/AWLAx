//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocLocation("action-sequence/built-in-plugins")]
    [AutoDoc("This plug-in allows you to invoke nearly any `public` method on any attached component. Therefore it should be used with care.")]
    public class InvokeComponentMethodPlugin : ActionSequencePlugin
    {

        #region Fields

        [AutoDocAs("Remote Target", "If checked the plug-in will target the component specified as the *Remote Target* on the **Action Sequence**. Otherwise it will use the `Component` on the current Game Object.")] public bool useRemoteTarget;
        [AutoDocAs("Component", "This list is built dynamically and fetches a list of all known components. Search for and select the component you wish and press `Enter`.")] public string componentType;
        [AutoDocAs("Method", "Dynamically built list for selected component showing all `public` methods.")] public string methodName;
        [AutoDocAs("Signature", "Dynamic list built based on the selected component, it shows all `overloads` for the selected method.")] public string methodSignature;
        [AutoDocAs("Parameters", "Dynamically build list of fields required by the selected method and signature.")] public string parameterJson;

        #endregion

        #region Properties

        [AutoDocSuppress] public override Texture2D icon { get { return GetResourceImage("icons/wizard"); } }

        [AutoDocSuppress] public override string title { get { return "Invoke Component Method"; } }

        [AutoDocSuppress] public override string description { get { return "Get a component on attached GameObject and invoke a method."; } }

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
                StringExtensions.Log(host, "InvokeComponentMethodPlugin", "Could not find component of type " + componentType);
                isComplete = true;
                return;
            }

            ActionSequenceHelper.InvokeMatchingSignature(component, methodName, methodSignature, parameterJson);

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