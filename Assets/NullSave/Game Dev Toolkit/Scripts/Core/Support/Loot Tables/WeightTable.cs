//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System.Collections.Generic;
using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDoc("Provides methods for getting results from a list of weighted entries")]
    [AutoDocLocation("loot-tables")]
    public class WeightTable
    {

        #region Public Methods

        [AutoDoc("Get results from a list of weighted unity entries")]
        [AutoDocParameter("List of entries to use")]
        public static List<WeightedUnityItem> GeneratedWeightedResult(List<WeightedUnityEntry> entries)
        {
            float totalWeight = 0;

            List<WeightedUnityEntry> tempList = new List<WeightedUnityEntry>();
            foreach (WeightedUnityEntry entry in entries)
            {
                tempList.Add(entry);
                totalWeight += entry.Weight;
            }

            // Sort items by weight
            tempList.Sort((p1, p2) => p1.Weight.CompareTo(p2.Weight));

            // Get roll
            float roll = Random.Range(0, totalWeight);

            // Get reward
            foreach (WeightedUnityEntry dropItem in tempList)
            {
                if (roll <= dropItem.Weight)
                {
                    return dropItem.Items;
                }
                else
                {
                    roll -= dropItem.Weight;
                }
            }

            return null;
        }

        [AutoDoc("Get results from a list of weighted entries")]
        [AutoDocParameter("List of entries to use")]
        public static List<WeightedItem> GeneratedWeightedResult(List<WeightedEntry> entries)
        {
            float totalWeight = 0;

            List<WeightedEntry> tempList = new List<WeightedEntry>();
            foreach (WeightedEntry entry in entries)
            {
                    tempList.Add(entry);
                    totalWeight += entry.Weight;
            }

            // Sort items by weight
            tempList.Sort((p1, p2) => p1.Weight.CompareTo(p2.Weight));

            // Get roll
            float roll = Random.Range(0, totalWeight);

            // Get reward
            foreach (WeightedEntry dropItem in tempList)
            {
                if (roll <= dropItem.Weight)
                {
                    return dropItem.Items;
                }
                else
                {
                    roll -= dropItem.Weight;
                }
            }

            return null;
        }

        #endregion

    }
}