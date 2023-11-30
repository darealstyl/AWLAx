//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System;
using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocLocation("action-sequence/built-in-plugins")]
    [AutoDoc("This plug-in causes the **Action Sequence** to pause for a specified amount of time before moving to the next action.")]
    public class WaitPlugin : ActionSequencePlugin
    {

        #region Fields

        [AutoDocAs("Seconds To Wait", "Number of seconds to wait.")] public float secondsToWait;
        [AutoDocAs("Real Time", "If checked the wait will use the system clock for waiting. Otherwise it will use `Time.deltaTime` to track passage of time.")] public bool realTime;

        private long startTime;
        private long waitUntil;
        private float waited;

        #endregion

        #region Properties

        [AutoDocSuppress] public override Texture2D icon { get { return GetResourceImage("icons/hourglass"); } }

        [AutoDocSuppress] public override string title { get { return "Wait"; } }

        [AutoDocSuppress]
        public override string titlebarText
        {
            get
            {
                if (realTime)
                {
                    return "Wait for " + secondsToWait + "s (realtime)";
                }

                return "Wait for " + secondsToWait + "s";
            }
        }

        [AutoDocSuppress] public override string description { get { return "Waits for a set amount of time before continuing to the next action."; } }

        #endregion

        #region Plugin Methods

        [AutoDocSuppress]
        public override void StartAction(ActionSequence host)
        {
            if (realTime)
            {
                startTime = DateTime.Now.Ticks;
                waitUntil = DateTime.Now.AddSeconds(secondsToWait).Ticks;
            }
            else
            {
                waited = 0;
            }

            isComplete = false;
            isStarted = true;
        }

        [AutoDocSuppress]
        public override void UpdateAction()
        {
            if (realTime)
            {
                long ticks = DateTime.Now.Ticks;

                progress = (ticks - startTime) / (waitUntil - startTime);

                if (ticks >= waitUntil)
                {
                    isComplete = true;
                }
            }
            else
            {
                waited += Time.deltaTime;
                progress = waited / secondsToWait;

                if (waited >= secondsToWait)
                {
                    isComplete = true;
                }
            }
        }

        #endregion

    }
}