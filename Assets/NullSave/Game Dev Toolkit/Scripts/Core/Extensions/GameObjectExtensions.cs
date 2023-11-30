//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocLocation("extensions")]
    [AutoDoc("This class contains methods that extends GameObject.")]
    public static class GameObjectExtensions 
    {

        #region Public Methods

        [AutoDoc("Check if a GameObject is a child of another GameObject")]
        [AutoDocParameter("GameObject to check")]
        [AutoDocParameter("Parent object to check")]
        public static bool IsChildOf(this GameObject gameObject, GameObject parent)
        {
            Transform t = gameObject.transform;
            Transform target = parent.transform;

            while(true)
            {
                if (t.parent == null) return false;
                if (t.parent == target) return true;
                t = t.parent;
            }
        }

        [AutoDoc("Set the layer of a GameObject")]
        [AutoDocParameter("GameObject to update")]
        [AutoDocParameter("Layer to set")]
        [AutoDocParameter("Set all children")]
        public static void SetLayer(this GameObject gameObject, int layer, bool recursive)
        {
            if(recursive)
            {
                foreach(Transform t in gameObject.GetComponentsInChildren<Transform>())
                {
                    t.gameObject.layer = layer;
                }
            }
            else
            {
                gameObject.layer = layer;
            }
        }

        #endregion

    }
}