//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System;

namespace NullSave.GDTK
{
    [Serializable]
    [AutoDocExcludeBase]
    [AutoDocLocation("action-sequence/support-classes")]
    [AutoDoc("Base class for creating plugins for the Action Sequence component.")]
    public class ActionSequencePlugin : UniversalPlugin
    {

        #region Properties

        [AutoDoc("Get/Set plugin run complete")] public virtual bool isComplete { get; set; }

        [AutoDoc("Get/Set plugin run started")] public virtual bool isStarted { get; set; }

        [AutoDoc("Get/Set plugin run progress")] public virtual float progress { get; set; }

        #endregion

        #region Public Methods

        [AutoDoc("Start plugin action")]
        [AutoDocParameter("ActionSequence hosting the plugin")]
        public virtual void StartAction(ActionSequence host) { }

        [AutoDoc("Perform fixed update actions")]
        public virtual void FixedUpdateAction() { }

        [AutoDoc("Perform update actions")]
        public virtual void UpdateAction() { }

        #endregion

    }
}