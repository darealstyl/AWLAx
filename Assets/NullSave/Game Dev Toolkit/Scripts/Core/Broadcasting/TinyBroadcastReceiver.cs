//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;
using UnityEngine.Events;

namespace NullSave.GDTK
{
    [AutoDocLocation("broadcast-system/components")]
    [AutoDoc("This component allows you to respond to messages sent by the Broadcast System without having to add any code.")]
    public class TinyBroadcastReceiver : MonoBehaviour, IBroadcastReceiver
    {

        #region Fields

        [Tooltip("Use public channel if checked, otherwise use named channel")] public bool usePublicChannel;
        [Tooltip("Name of channel to subscribe to")] public string channelName;

        [Tooltip("Message to wait for")] public string awaitMessage;

        [Tooltip("Event raised when a message is received")] public UnityEvent onMessageReceived;

        #endregion

        #region Unity Events

        /// <summary>
        /// Remove subscriptions on death
        /// </summary>
        private void OnDestroy()
        {
            Broadcaster.UnsubscribeFromAll(this);
        }

        /// <summary>
        /// Subscribe to broadcasts based on settings
        /// </summary>
        private void Start()
        {
            if (usePublicChannel)
            {
                Broadcaster.SubscribeToPublic(this);
            }
            else
            {
                Broadcaster.SubscribeToChannel(this, channelName);
            }
        }

        private void Reset()
        {
            usePublicChannel = true;
        }

        #endregion

        #region Receiver Methods

        [AutoDocSuppress]
        public void BroadcastReceived(object sender, string channel, string message, object[] args)
        {
            if (usePublicChannel) return;
            if (channel == channelName && message == awaitMessage)
            {
                onMessageReceived?.Invoke();
            }
        }

        [AutoDocSuppress]
        public void PublicBroadcastReceived(object sender, string message)
        {
            if (!usePublicChannel) return;
            if (message == awaitMessage)
            {
                onMessageReceived?.Invoke();
            }
        }

        #endregion

    }
}