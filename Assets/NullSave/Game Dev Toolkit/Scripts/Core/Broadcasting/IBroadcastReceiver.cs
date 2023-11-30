//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

namespace NullSave.GDTK
{
    [AutoDocLocation("broadcast-system/interfaces")]
    [AutoDoc("Use this interface to have a class access the Broadcast System.")]
    public interface IBroadcastReceiver
    {

        #region Methods

        [AutoDoc("Method for receiving broadcasts on named channels")]
        [AutoDocParameter("Object sending the message")]
        [AutoDocParameter("Channel message is being sent on")]
        [AutoDocParameter("Message")]
        [AutoDocParameter("Arguments for the message")]
        void BroadcastReceived(object sender, string channel, string message, object[] args);

        [AutoDoc("Method for receiving public broadcasts")]
        [AutoDocParameter("Object sending the message")]
        [AutoDocParameter("Message")]
        void PublicBroadcastReceived(object sender, string message);

        #endregion

    }
}