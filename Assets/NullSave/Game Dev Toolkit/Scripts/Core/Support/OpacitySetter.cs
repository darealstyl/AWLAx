//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NullSave.GDTK
{
    [AutoDocLocation("ui")]
    [AutoDoc("This component allows you to set the opacity for a list of graphics")]
    public class OpacitySetter : MonoBehaviour
    {

        #region Fields

        [Tooltip("List of graphics to target")] public List<Graphic> targets;

        #endregion

        #region Public Methods

        [AutoDoc("Set the opacity for all graphics in the 'targets' list")]
        [AutoDocParameter("Opactiy to set")]
        public void SetOpacity(float value)
        {
            foreach(Graphic g in targets)
            {
                g.color = new Color(g.color.r, g.color.g, g.color.b, value);
            }
        }

        #endregion

    }
}
