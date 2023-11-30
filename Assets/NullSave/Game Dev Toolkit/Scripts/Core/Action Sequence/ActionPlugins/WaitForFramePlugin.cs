//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocLocation("action-sequence/built-in-plugins")]
    [AutoDoc("Wait for a single frame to pass")]
    public class WaitForFramePlugin : ActionSequencePlugin
    {

        #region Fields

        private int remainingCount;

        #endregion

        #region Properties

        [AutoDocSuppress] public override Texture2D icon { get { return GetResourceImage("icons/hourglass"); } }

        [AutoDocSuppress] public override string title { get { return "Wait for End of Frame"; } }

        [AutoDocSuppress] public override string description { get { return "Waits for a end of frame before continuing to the next action."; } }

        #endregion

        #region Plugin Methods

        [AutoDocSuppress]
        public override void StartAction(ActionSequence host)
        {
            isComplete = false;
            isStarted = true;
            remainingCount = 1;
        }

        [AutoDocSuppress]
        public override void UpdateAction()
        {
            remainingCount -= 1;
            if (remainingCount < 0)
            {
                isComplete = true;
            }
        }

        #endregion

    }
}