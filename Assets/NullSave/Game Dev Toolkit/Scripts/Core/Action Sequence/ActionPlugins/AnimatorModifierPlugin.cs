//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocExcludeBase]
    [AutoDocLocation("action-sequence/built-in-plugins")]
    [AutoDoc("This plug-in allows you to set a parameter or trigger value on the associated `Animator` component.")]
    public class AnimatorModifierPlugin : ActionSequencePlugin
    {

        #region Fields

        [AutoDocAs("Remote Target", "If checked the plug-in will target the component specified as the *Remote Target* on the **Action Sequence**. Otherwise it will use the `Animator` on the current Game Object.")] public bool useRemoteTarget;
        [AutoDoc("Name of the parameter on the `Animator` to modify.")] public string keyName;
        [AutoDocAs("Parameter Type", "Type of the associated parameter. `Bool`, `Float`, `Int`, `Trigger`")] public AnimatorParamType paramType;
        [AutoDocAs("Value", "Value to assign to the parameter.")] public bool boolVal;
        [AutoDocSuppress] public int intVal;
        [AutoDocSuppress] public float floatVal;
        [AutoDocSuppress] public AnimatorTriggerType triggerVal;

        private Animator target;

        #endregion

        #region Properties

        [AutoDocSuppress]
        public override Texture2D icon { get { return GetResourceImage("icons/animate"); } }

        [AutoDocSuppress]
        public override string title { get { return "Animator Modifier"; } }

        [AutoDocSuppress]
        public override string titlebarText
        {
            get
            {
                string prepend = useRemoteTarget ? "(Remote) " : string.Empty;

                return paramType switch
                {
                    AnimatorParamType.Bool => prepend + "Animator: Set " + keyName + " = " + boolVal,
                    AnimatorParamType.Float => prepend + "Animator: Set " + keyName + " = " + floatVal,
                    AnimatorParamType.Int => prepend + "Animator: Set " + keyName + " = " + intVal,
                    _ => prepend + "Animator: " + triggerVal.ToString() + " trigger " + keyName,
                };
            }
        }

        [AutoDocSuppress]
        public override string description { get { return "Sets a value on the attached Animator (if any)."; } }

        #endregion

        #region Plugin Methods

        [AutoDocSuppress]
        public override void StartAction(ActionSequence host)
        {
            isComplete = false;
            isStarted = true;

            if (useRemoteTarget)
            {
                target = host.remoteTarget.GetComponentInChildren<Animator>();
            }
            else
            {
                target = host.GetComponentInChildren<Animator>();
            }

            if (target != null)
            {
                switch (paramType)
                {
                    case AnimatorParamType.Bool:
                        target.SetBool(keyName, boolVal);
                        break;
                    case AnimatorParamType.Float:
                        target.SetFloat(keyName, floatVal);
                        break;
                    case AnimatorParamType.Int:
                        target.SetInteger(keyName, intVal);
                        break;
                    case AnimatorParamType.Trigger:
                        if (triggerVal == AnimatorTriggerType.Reset)
                        {
                            target.ResetTrigger(keyName);
                        }
                        else
                        {
                            target.SetTrigger(keyName);
                        }
                        break;
                }
            }

            isComplete = true;
        }

        #endregion

    }
}