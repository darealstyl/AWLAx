//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocLocation("action-sequence/built-in-plugins")]
    [AutoDoc("This plug-in allows you to find a `GameObject` by name and set nearly any `public` value on any attached component. Therefore it should be used with care.")]
    public class SetGameObjectComponentValuePlugin : ActionSequencePlugin
    {

        #region Fields

        [AutoDocAs("Game Object Name", "")] public string gameObjectName;
        [AutoDocAs("Component", "This list is built dynamically and fetches a list of all known components. Search for and select the component you wish and press `Enter`.")] public string componentType;
        [AutoDocAs("Field", "Dynamically built list for selected component showing all `public` fields.")] public string memberName;
        [AutoDocSuppress] public string memberType;
        [AutoDocAs("Value", "Dynamic value field based on the selected **Field**.")] public string memberValue;
        [AutoDocSuppress] public string memberJson;

        #endregion

        #region Properties

        [AutoDocSuppress] public override Texture2D icon { get { return GetResourceImage("icons/wizard"); } }

        [AutoDocSuppress] public override string title { get { return "Set GameObject Component Value"; } }

        [AutoDocSuppress] public override string description { get { return "Find a game object and then a component attached to it and set a value."; } }

        #endregion

        #region Public Methods

        [AutoDocSuppress]
        public override void StartAction(ActionSequence host)
        {
            isStarted = true;

            // Find GameObject
            GameObject go = GameObject.Find(gameObjectName);
            if (go == null)
            {
                StringExtensions.Log(host, "SetGameObjectComponentValuePlugin", "Could not find GameObject named " + gameObjectName);
                isComplete = true;
                return;
            }

            // Find object
            object component = GetComponent(go);
            if (component == null)
            {
                StringExtensions.Log(host, "SetGameObjectComponentValuePlugin", "Could not find component of type " + componentType);
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