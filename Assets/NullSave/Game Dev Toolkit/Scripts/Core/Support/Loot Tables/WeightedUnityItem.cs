//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System;
using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDoc("Represents an item to be created by a weighted loot table")]
    [AutoDocLocation("loot-tables")]
    [Serializable]
    public class WeightedUnityItem
    {

        #region Fields

        [AutoDoc("Number of the item to create")] public int Count;
        [AutoDoc("Item to create")] public UnityEngine.Object Object;
        [AutoDoc("Create a random number of the object")] public bool UseRandomCount;
        [AutoDoc("Minimum count to create")] public int MinCount;
        [AutoDoc("Maximum count to create")] public int MaxCount;
        [AutoDoc("Assign a random rotation to object(s)")] public bool randomRotation;
        [AutoDoc("Minimum rotation")] public Vector3 minRotation;
        [AutoDoc("Maximum rotation")] public Vector3 maxRotation;

        #endregion

    }
}