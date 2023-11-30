//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System.Collections.Generic;
using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocLocation("tool-registry/components")]
    [AutoDoc("This component allows you to easily add existing object to the Tool Registry")]
    [DefaultExecutionOrder(-200)]
    public class ToolRegistryHelper : MonoBehaviour
    {

        #region Fields

        [Tooltip("List of items to register")] public List<Object> registerItems;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            foreach (Object obj in registerItems)
            {
                ToolRegistry.RegisterComponent(obj);
            }
        }

        #endregion

    }
}