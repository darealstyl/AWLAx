//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

namespace NullSave.GDTK
{
    [AutoDocSuppress]
    internal class BroadcastSubscription
    {

        #region Fields

        public IBroadcastReceiver receiver;
        public string channel;

        #endregion

    }
}