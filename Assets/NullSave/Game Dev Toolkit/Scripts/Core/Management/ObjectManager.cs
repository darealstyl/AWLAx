//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDoc("Provides object creation/deletion management. This can be used in place of the normal Instantiate and Destroy methods to allow working with networked projects.")]
    [AutoDocLocation("object-managers")]
    [AutoDocExcludeBase]
    public class ObjectManager : ScriptableObject
    {

        #region Public Methods

        [AutoDoc("Instantiate an object")]
        [AutoDocParameter("Original object to instantiate")]
        public virtual T InstantiateObject<T>(T original) where T : Object { throw new System.NotImplementedException(); }

        [AutoDoc("Instantiate an object")]
        [AutoDocParameter("Original object to instantiate")]
        [AutoDocParameter("Transform to use as object parent")]
        public virtual T InstantiateObject<T>(T original, Transform parent) where T : Object { throw new System.NotImplementedException(); }

        [AutoDoc("Instantiate an object")]
        [AutoDocParameter("Original object to instantiate")]
        [AutoDocParameter("Position to apply to object")]
        [AutoDocParameter("Rotation to apply to object")]
        public virtual T InstantiateObject<T>(T original, Vector3 position, Quaternion rotation) where T : Object { throw new System.NotImplementedException(); }

        [AutoDoc("Instantiate an object")]
        [AutoDocParameter("Original object to instantiate")]
        [AutoDocParameter("Position to apply to object")]
        [AutoDocParameter("Rotation to apply to object")]
        [AutoDocParameter("Transform to use as object parent")]
        public virtual T InstantiateObject<T>(T original, Vector3 position, Quaternion rotation, Transform parent) where T : Object { throw new System.NotImplementedException(); }

        [AutoDoc("Destroy an object")]
        [AutoDocParameter("Object to destroy")]
        public new virtual void DestroyObject(Object target) { }

        #endregion

    }
}