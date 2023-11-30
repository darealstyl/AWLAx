//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocLocation("object-managers")]
    [AutoDocExcludeBase]
    [CreateAssetMenu(menuName = "Tools/GDTK/Settings/Unity Object Manager", fileName = "Unity Object Manager")]
    public class UnityObjectManager : ObjectManager
    {

        #region Public Methods

        public override T InstantiateObject<T>(T original)
        {
            return Instantiate(original);
        }

        public override T InstantiateObject<T>(T original, Transform parent)
        {
            return Instantiate(original, parent);
        }

        public override T InstantiateObject<T>(T original, Vector3 position, Quaternion rotation)
        {
            return Instantiate(original, position, rotation);
        }

        public override T InstantiateObject<T>(T original, Vector3 position, Quaternion rotation, Transform parent)
        {
            return Instantiate(original, position, rotation, parent);
        }

        public override void DestroyObject(Object target)
        {
            Destroy(target);
        }

        #endregion

    }
}