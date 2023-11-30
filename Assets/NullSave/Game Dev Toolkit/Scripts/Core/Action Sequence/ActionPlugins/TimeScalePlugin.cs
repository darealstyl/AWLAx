//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocLocation("action-sequence/built-in-plugins")]
    [AutoDoc("Seth the 'Time.timeScale value.")]
    public class TimeScalePlugin : ActionSequencePlugin
    {

        #region Fields

        [AutoDocAs("Time Scale", "Value to assign to `Time.timeScale`.")] public float timeScale;

        #endregion

        #region Properties

        [AutoDocSuppress] public override Texture2D icon { get { return GetResourceImage("icons/wait"); } }

        [AutoDocSuppress] public override string title { get { return "Time Scale"; } }

        [AutoDocSuppress]
        public override string titlebarText
        {
            get
            {
                return "Set Time.timeScale to " + timeScale;
            }
        }

        [AutoDocSuppress] public override string description { get { return "Sets the timescale to a provided value."; } }

        #endregion

        #region Plugin Methods

        [AutoDocSuppress]
        public override void StartAction(ActionSequence host)
        {
            isComplete = false;
            isStarted = true;
            Time.timeScale = timeScale;
            isComplete = true;
        }

        #endregion

    }
}