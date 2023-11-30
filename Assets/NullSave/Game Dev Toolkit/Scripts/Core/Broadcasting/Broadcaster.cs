//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NullSave.GDTK
{
    [AutoDocLocation("broadcast-system/components")]
    [AutoDoc("This component allows you to send/receive messages to any item with an IBroadcastReceiver implemented.")]
    [ExecuteInEditMode]
    [DefaultExecutionOrder(-900)]
    public class Broadcaster : MonoBehaviour
    {

        #region Fields

        private List<BroadcastSubscription> channelReceivers;
        private List<IBroadcastReceiver> publicReceivers;
        private List<IBroadcastReceiver> globalReceivers;

        private static string preventRebirth;

        #endregion

        #region Properties

        [AutoDoc("Returns current broadcaster (automatically created if needed)")]
        public static Broadcaster Current
        {
            get
            {
                Broadcaster result = ToolRegistry.GetComponent<Broadcaster>();
                if (result != null) return result;

                if (preventRebirth == SceneManager.GetActiveScene().name) return null;

                GameObject go = new GameObject("GDTK Broadcaster");
                result = go.AddComponent<Broadcaster>();
                DontDestroyOnLoad(go);

                return result;
            }
        }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            channelReceivers = new List<BroadcastSubscription>();
            publicReceivers = new List<IBroadcastReceiver>();
            globalReceivers = new List<IBroadcastReceiver>();
            preventRebirth = string.Empty;
        }

        private void OnDestroy()
        {
            preventRebirth = SceneManager.GetActiveScene().name;
        }

        private void OnDisable()
        {
            ToolRegistry.RemoveComponent(this);
        }

        private void OnEnable()
        {
            ToolRegistry.RegisterComponent(this);

            if (ToolRegistry.GetComponents<Broadcaster>().Count > 1)
            {
                StringExtensions.LogWarning(gameObject, "Broadcaster", "More than one broadcaster is currently registered. Most components will only subscribe to the first one.");
            }
        }

        #endregion

        #region Public Methods

        [AutoDoc("Broadcast a message to all receivers of a channel")]
        [AutoDocParameter("Channel to use for message")]
        [AutoDocParameter("Message to send")]
        public static void Broadcast(string channel, string message)
        {
            Broadcast(null, channel, message, null);
        }

        [AutoDoc("Broadcast a message to all receivers of a channel")]
        [AutoDocParameter("Sender of the message")]
        [AutoDocParameter("Channel to use for message")]
        [AutoDocParameter("Message to send")]
        public static void Broadcast(object sender, string channel, string message)
        {
            Broadcast(sender, channel, message, null);
        }

        [AutoDoc("Broadcast a message to all receivers of a channel")]
        [AutoDocParameter("Channel to use for message")]
        [AutoDocParameter("Message to send")]
        [AutoDocParameter("Arguments for the message")]
        public static void Broadcast(string channel, string message, object[] args)
        {
            Broadcast(null, channel, message, args);
        }

        [AutoDoc("Broadcast a message to all receivers of a channel")]
        [AutoDocParameter("Sender of the message")]
        [AutoDocParameter("Channel to use for message")]
        [AutoDocParameter("Message to send")]
        [AutoDocParameter("Arguments for the message")]
        public static void Broadcast(object sender, string channel, string message, object[] args)
        {
            foreach (BroadcastSubscription subscription in Current.channelReceivers)
            {
                if (subscription.channel == channel)
                {
                    subscription.receiver.BroadcastReceived(sender, channel, message, args);
                }
            }

            foreach (IBroadcastReceiver receiver in Current.globalReceivers)
            {
                receiver.BroadcastReceived(sender, channel, message, args);
            }
        }

        [AutoDoc("Broadcast to all receivers")]
        [AutoDocParameter("Sender of the message")]
        [AutoDocParameter("Message to send")]
        public static void PublicBroadcast(object sender, string message)
        {
            // .ToArray() prevents collection modification collisions
            foreach (IBroadcastReceiver receiver in Current.publicReceivers.ToArray())
            {
                receiver.PublicBroadcastReceived(sender, message);
            }

            foreach (IBroadcastReceiver receiver in Current.globalReceivers.ToArray())
            {
                receiver.PublicBroadcastReceived(sender, message);
            }
        }

        [AutoDoc("Non-static version of PublicBroadcast so Unity Events can see/address")]
        [AutoDocParameter("Message to send")]
        public void SendPublicBroadcast(string message)
        {
            PublicBroadcast(this, message);
        }

        [AutoDoc("Subscribe to all public and named channels")]
        [AutoDocParameter("Receiver requesting subscription")]
        public static void SubscribeToAll(IBroadcastReceiver receiver)
        {
            Current.globalReceivers.Add(receiver);
        }

        [AutoDoc("Subscribe to a channel")]
        [AutoDocParameter("Receiver requesting subscription")]
        [AutoDocParameter("Channel to add subscription")]
        public static void SubscribeToChannel(IBroadcastReceiver receiver, string channel)
        {
            BroadcastSubscription sub = new BroadcastSubscription { channel = channel, receiver = receiver };
            if (!Current.channelReceivers.Contains(sub))
            {
                Current.channelReceivers.Add(sub);
            }
        }

        [AutoDoc("Subscribe to public broadcasts")]
        [AutoDocParameter("Receiver requesting subscription")]
        public static void SubscribeToPublic(IBroadcastReceiver receiver)
        {
            if (!Current.publicReceivers.Contains(receiver))
            {
                Current.publicReceivers.Add(receiver);
            }
        }

        [AutoDoc("Unsubscribe from all channels and public broadcasts")]
        [AutoDocParameter("Receiver requesting unsubscription")]
        public static void UnsubscribeFromAll(IBroadcastReceiver receiver)
        {
            if (Current == null) return;
            Current.publicReceivers.Remove(receiver);
            Current.globalReceivers.Remove(receiver);

            List<BroadcastSubscription> removals = new List<BroadcastSubscription>();
            foreach (BroadcastSubscription subscription in Current.channelReceivers)
            {
                if (subscription.receiver == receiver)
                {
                    removals.Add(subscription);
                }
            }

            foreach (BroadcastSubscription subscription in removals)
            {
                Current.channelReceivers.Remove(subscription);
            }
        }

        [AutoDoc("Unsubscribe from a channel")]
        [AutoDocParameter("Receiver requesting unsubscription")]
        [AutoDocParameter("Channel to remove subscription")]
        public static void UnsubscribeFromChannel(IBroadcastReceiver receiver, string channel)
        {
            if (Current == null) return;
            List<BroadcastSubscription> removals = new List<BroadcastSubscription>();
            foreach (BroadcastSubscription subscription in Current.channelReceivers)
            {
                if (subscription.receiver == receiver && subscription.channel == channel)
                {
                    removals.Add(subscription);
                }
            }

            foreach (BroadcastSubscription subscription in removals)
            {
                Current.channelReceivers.Remove(subscription);
            }
        }

        [AutoDoc("Unsubscribe from public broadcasts")]
        [AutoDocParameter("Receiver requesting unsubscription")]
        public static void UnsubscribeFromPublic(IBroadcastReceiver receiver)
        {
            if (Current == null) return;
            Current.publicReceivers.Remove(receiver);
        }

        #endregion

    }
}