//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocLocation("action-sequence/built-in-plugins")]
    [AutoDoc("This plug-in uses the [Broadcast System](../../broadcast-system/introduction) to send a message on a named channel.")]
    public class PrivateBroadcastPlugin : ActionSequencePlugin
    {

        #region Fields

        [AutoDocAs("Channel Name", "Name of the channel to use when broadcasting message.")] public string channelName;
        [AutoDocAs("Message", "Message to broadcast.")] public string message;

        #endregion

        #region Properties

        [AutoDocSuppress] public override Texture2D icon { get { return GetResourceImage("icons/broadcast"); } }

        [AutoDocSuppress] public override string title { get { return "Private Broadcast"; } }

        [AutoDocSuppress]
        public override string titlebarText
        {
            get
            {
                return "Broadcast on channel " + channelName;
            }
        }

        [AutoDocSuppress] public override string description { get { return "Sends a message via Broadcaster on a private named channel."; } }

        #endregion

        #region Plugin Methods

        [AutoDocSuppress]
        public override void StartAction(ActionSequence host)
        {
            isComplete = false;
            isStarted = true;
            Broadcaster.Broadcast(host.gameObject, channelName, message);
            isComplete = true;
        }

        #endregion

    }
}