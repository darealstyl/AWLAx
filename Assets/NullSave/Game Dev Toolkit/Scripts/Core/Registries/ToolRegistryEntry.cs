//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine.SceneManagement;

namespace NullSave.GDTK
{

    [AutoDocSuppress]
    internal class ToolRegistryEntry
    {

        #region Fields

        public string Key;
        public object Object;
        public bool Persist;
        public Scene Scene;

        #endregion

    }

}