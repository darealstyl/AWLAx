//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System;
using System.Collections.Generic;

namespace NullSave.GDTK
{
    [AutoDoc("Represents an entry of Unity objects in a weight loot table.")]
    [AutoDocLocation("loot-tables")]
    [Serializable]
    public class WeightedUnityEntry
    {

        #region Fields

        [AutoDoc("The weight assigned to this entry, higher values represent a larger chance")] public float Weight;
        [AutoDoc("List of items associated with the entry")] public List<WeightedUnityItem> Items;

        #endregion

    }
}