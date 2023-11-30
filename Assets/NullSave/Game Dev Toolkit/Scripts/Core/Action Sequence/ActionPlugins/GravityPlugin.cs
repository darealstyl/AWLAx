//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocLocation("action-sequence/built-in-plugins")]
    [AutoDoc("This plug-in allows you to set `Physics.gravity` value.")]
    public class GravityPlugins : ActionSequencePlugin
    {

        #region Fields

        [AutoDoc("Value to assign to `Physics.gravity`.")] public Vector3 gravity;

        #endregion

        #region Properties

        [AutoDocSuppress] public override Texture2D icon { get { return GetResourceImage("icons/heavy"); } }

        [AutoDocSuppress] public override string title { get { return "Gravity"; } }

        [AutoDocSuppress]
        public override string titlebarText
        {
            get
            {
                return "Set Physics.gravity to " + gravity;
            }
        }

        [AutoDocSuppress] public override string description { get { return "Adjusts the gravity to a provided value."; } }

        #endregion

        #region Plugin Methods

        [AutoDocSuppress]
        public override void StartAction(ActionSequence host)
        {
            isComplete = false;
            isStarted = true;
            Physics.gravity = gravity;
            isComplete = true;
        }

        #endregion

    }
}