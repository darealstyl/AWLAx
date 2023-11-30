//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

#if ENABLE_INPUT_SYSTEM
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocLocation("action-icons-system")]
    [AutoDoc("The **Controller Map** allows you to map actions to icons for specific controllers. You can create a new map by right-clicking in the Project pane and selecting Create > Tools > GDTK > Controller Map.")]
    [AutoDocExcludeBase]
    [CreateAssetMenu(menuName = "Tools/GDTK/Controller Map", order = 0)]
    public class ControllerMap : ScriptableObject
    {

        #region Variables

        [Tooltip("Sets if this controller should be used if no compatible devices are found")] public bool isFallback;
        [Tooltip("Text Mesh Pro sprite asset to associate with this controller")] public TMP_SpriteAsset tmpSpriteAsset;
        [Tooltip("List of compatible devices")] public List<string> compatibleDevices;
        [Tooltip("List of associated inputs")] public List<InputMap> inputMaps;

        #endregion

        #region Public Methods

        [AutoDoc("Gets an input map associated with an action")]
        [AutoDocParameter("Name of the action to retrieve")]
        public InputMap GetAction(string actionName)
        {
            foreach (InputMap map in inputMaps)
            {
                if (map.inputName == actionName)
                {
                    map.TMPSpriteAsset = tmpSpriteAsset;
                    return map;
                }
            }

            return null;
        }

        [AutoDoc("Checks if there is an input map associated with an action name")]
        [AutoDocParameter("Name of the action to chedk")]
        public bool HasAction(string actionName)
        {
            foreach (InputMap map in inputMaps)
            {
                if (map.inputName == actionName)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

    }
}
#endif