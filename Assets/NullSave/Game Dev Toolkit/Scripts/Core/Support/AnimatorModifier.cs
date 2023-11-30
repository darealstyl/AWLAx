//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System;
using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDoc("Provides the ability to modify Animator properties")]
    [AutoDocLocation("miscellaneous/classes")]
    [Serializable]
    public class AnimatorModifier 
    {

        #region Fields

        [AutoDoc("Name of the paramater to modify")] public string keyName;
        [AutoDoc("Type of the parameter")] public AnimatorParamType paramType;
        [AutoDoc("Boolean value to assign")] public bool boolVal;
        [AutoDoc("Integer value to assign")] public int intVal;
        [AutoDoc("Float value to assign")] public float floatVal;
        [AutoDoc("Triger value to assign")] public AnimatorTriggerType triggerVal;

        #endregion

        #region Editor Only Properties

#if UNITY_EDITOR

        [AutoDocSuppress]
        public bool Expanded { get; set; }

#endif

        #endregion

        #region Public Methods

        [AutoDoc("Set the value to the animator")]
        [AutoDocParameter("Animator to update")]
        public void ApplyMod(Animator animator)
        {
            if (animator == null) return;

            switch (paramType)
            {
                case AnimatorParamType.Bool:
                    animator.SetBool(keyName, boolVal);
                    break;
                case AnimatorParamType.Float:
                    animator.SetFloat(keyName, floatVal);
                    break;
                case AnimatorParamType.Int:
                    animator.SetInteger(keyName, intVal);
                    break;
                case AnimatorParamType.Trigger:
                    if (triggerVal == AnimatorTriggerType.Reset)
                    {
                        animator.ResetTrigger(keyName);
                    }
                    else
                    {
                        animator.SetTrigger(keyName);
                    }
                    break;
            }
        }

        [AutoDoc("Check of the Animator's value already equals the desired value")]
        [AutoDocParameter("Animator to check")]
        public bool CheckMod(Animator animator)
        {
            if (animator == null) return false;

            return paramType switch
            {
                AnimatorParamType.Bool => animator.GetBool(keyName) == boolVal,
                AnimatorParamType.Float => animator.GetFloat(keyName) == floatVal,
                AnimatorParamType.Int => animator.GetInteger(keyName) == intVal,
                _ => false,
            };
        }

        [AutoDoc("Create a clone of this object")]
        public AnimatorModifier Clone()
        {
            AnimatorModifier result = new AnimatorModifier();

            result.keyName = keyName;
            result.paramType = paramType;
            result.boolVal = boolVal;
            result.intVal = intVal;
            result.floatVal = floatVal;
            result.triggerVal = triggerVal;

            return result;
        }

        [AutoDocSuppress]
        public string ToDescriptive()
        {
            return paramType switch
            {
                AnimatorParamType.Bool => "Set " + keyName + " = " + boolVal,
                AnimatorParamType.Float => "Set " + keyName + " = " + floatVal,
                AnimatorParamType.Int => "Set " + keyName + " = " + intVal,
                _ => triggerVal.ToString() + " trigger " + keyName,
            };
        }

        #endregion

    }
}