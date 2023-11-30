//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocLocation("action-sequence/built-in-plugins")]
    [AutoDoc("Set the active state of a game object")]
    public class SetActivePlugin : ActionSequencePlugin
    {

        #region Fields

        [AutoDocAs("Active", "Active state.")] public bool active;
        [AutoDocAs("Remote Target", "If checked the plug-in will target the component specified as the *Remote Target* on the **Action Sequence**. Otherwise it will use the `Component` on the current Game Object.")] public bool useRemoteTarget;

        #endregion

        #region Properties

        [AutoDocSuppress] public override Texture2D icon { get { return GetResourceImage("icons/check"); } }

        [AutoDocSuppress] public override string title { get { return "Set Active"; } }

        [AutoDocSuppress]
        public override string titlebarText
        {
            get
            {
                return "Set Active = " + active;
            }
        }

        [AutoDocSuppress] public override string description { get { return "Set GameObject active state."; } }

        #endregion

        #region Plugin Methods

        [AutoDocSuppress]
        public override void StartAction(ActionSequence host)
        {
            if (useRemoteTarget)
            {
                host.remoteTarget.SetActive(active);
            }
            else
            {
                host.gameObject.SetActive(active);
            }

            isComplete = true;
            isStarted = true;
        }

        #endregion

    }
}