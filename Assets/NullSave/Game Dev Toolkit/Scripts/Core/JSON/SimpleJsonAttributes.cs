//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System;

namespace NullSave.GDTK
{
    [AutoDocExcludeBase]
    [AutoDoc("Mark property or field to be serialized even if it is private")]
    [AutoDocLocation("json/attributes")]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class JsonSerialize : Attribute { }

    [AutoDocExcludeBase]
    [AutoDoc("Mark a property of field to not be serialized")]
    [AutoDocLocation("json/attributes")]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class JsonDoNotSerialize : Attribute { }

    [AutoDocExcludeBase]
    [AutoDoc("Set method to be invoked before serialization")]
    [AutoDocLocation("json/attributes")]
    [AttributeUsage(AttributeTargets.Method)]
    public class JsonBeforeSerialization : Attribute { }

    [AutoDocExcludeBase]
    [AutoDoc("Set method to be invoked after serialization")]
    [AutoDocLocation("json/attributes")]
    [AttributeUsage(AttributeTargets.Method)]
    public class JsonAfterDeserialization : Attribute { }

    [AutoDocExcludeBase]
    [AutoDoc("Mark property to field to be serialized using a different name")]
    [AutoDocLocation("json/attributes")]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public class JsonSerializeAs : Attribute
    {

        #region Fields

        [AutoDoc("Name to use in serialization/deserialization")] public string SerializeName;

        #endregion

        #region Constructor

        public JsonSerializeAs(string serializeName)
        {
            SerializeName = serializeName;
        }

        #endregion

    }

}