//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System;

namespace NullSave.GDTK
{
    [AutoDocLocation("json/classes")]
    [AutoDoc("Provides JSON serialization and deserialization")]
    public static class SimpleJson
    {

        #region Public Methods

        [AutoDoc("Converts a JSON string to a typed object")]
        [AutoDocParameter("String to convert")]
        public static T FromJSON<T>(string json)
        {
            JsonDeserializationObject jdo = new JsonDeserializationObject(json);
            return jdo.Deserialize<T>();
        }

        [AutoDoc("Converts a JSON string to a typed object")]
        [AutoDocParameter("String to convert")]
        [AutoDocParameter("Type to convert to")]
        public static object FromJSON(string json, Type type)
        {
            JsonDeserializationObject jdo = new JsonDeserializationObject(json);
            return jdo.Deserialize(type);
        }

        /// <summary>
        /// Simple method for serializing to JSON.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="serializationType"></param>
        /// <returns></returns>
        [AutoDoc("Converts an object to a JSON string")]
        [AutoDocParameter("Object to convert")]
        [AutoDocParameter("Remove nulls from output")]
        [AutoDocParameter("Include tabs and returns to make result easily readable")]
        public static string ToJSON(object obj, bool removeNulls = true, bool readable = false)
        {
            return new JsonSerializationObject(obj, removeNulls, readable).value;
        }

        #endregion

    }
}