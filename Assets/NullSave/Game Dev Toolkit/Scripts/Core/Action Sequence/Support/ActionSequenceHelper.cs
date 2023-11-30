//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocLocation("action-sequence/support-classes")]
    [AutoDoc("This class provides helper methods for executing action sequences.")]
    public class ActionSequenceHelper
    {

        #region Public Methods

        [AutoDoc("Invoke a method on an object with a matching signature")]
        [AutoDocParameter("Object to use")]
        [AutoDocParameter("Name of the method")]
        [AutoDocParameter("Signature of the method")]
        [AutoDocParameter("Parameters to pass")]
        public static bool InvokeMatchingSignature(object component, string methodName, string signature, string parameterJson)
        {
            try
            {
                MethodInfo method = GetMatchingSignature(component, methodName, signature);
                if (method == null)
                {
                    return false;
                }

                method.Invoke(component, ParamDataListToObjectArray(method, parameterJson));
                return true;
            }
            catch
            {
                StringExtensions.LogError("ActionSequenceHelper", "InvokeMatchingSignature", "Invoke failed");
                return false;
            }
        }

        [AutoDoc("Get a method from an object with a matching name and signature")]
        [AutoDocParameter("Object to use")]
        [AutoDocParameter("Name of the method")]
        [AutoDocParameter("Signature of the method")]
        public static MethodInfo GetMatchingSignature(object component, string methodName, string signature)
        {
            // Find signature list
            MemberInfo[] signatures = component.GetType().GetMember(methodName);
            if (signatures == null || signatures.Length == 0)
            {
                StringExtensions.LogError("ActionSequenceHelper", "GetMatchingSignature", "Could not find method named " + methodName);
                return null;
            }

            // Find proper signature
            for (int i = 0; i < signatures.Length; i++)
            {
                if (signatures[i].ToString() == signature)
                {
                    return (MethodInfo)signatures[i];
                }
            }

            StringExtensions.LogError("ActionSequenceHelper", "GetMatchingSignature", "Could not find signature " + signature);
            return null;
        }

        [AutoDoc("Get the object value of a json string")]
        [AutoDocParameter("Value")]
        [AutoDocParameter("JSON")]
        [AutoDocParameter("Type of object")]
        public static object GetValue(string value, string json, Type type)
        {
            if (value == null)
            {
                // Try to create first
                try
                {
                    return Activator.CreateInstance(type);
                }
                catch
                {
                    try { return Convert.ChangeType(SimpleJson.FromJSON(json, type), type); } catch { }
                }
            }
            else
            {
                if (json != null && json.StartsWith("!!RESOURCE!!"))
                {
                    return Resources.Load(json.Substring(12));
                }

                try
                {
                    return Convert.ChangeType(value, type);
                }
                catch
                {
                    try
                    {
                        return Convert.ChangeType(SimpleJson.FromJSON(json, type), type);
                    }
                    catch
                    {
                        try { return Activator.CreateInstance(type); } catch { }
                    }
                }
            }

            return null;
        }

        [AutoDoc("Set the value of a member on an object")]
        [AutoDocParameter("Object to use")]
        [AutoDocParameter("Name of the member")]
        [AutoDocParameter("Name of the value")]
        [AutoDocParameter("Value to set")]
        public static bool SetMemberValue(object component, string memberName, string value, string valueJson)
        {
            try
            {
                // Find member
                MemberInfo[] memberInfo = component.GetType().GetMember(memberName);
                if (memberInfo == null || memberInfo.Length == 0)
                {
                    StringExtensions.LogError("ActionSequenceHelper", "SetMemberValue", "Could not find member " + memberName);
                    return false;
                }

                switch (memberInfo[0].MemberType)
                {
                    case MemberTypes.Field:
                        FieldInfo field = (FieldInfo)memberInfo[0];
                        field.SetValue(component, GetValue(value, valueJson, field.FieldType));
                        break;
                    case MemberTypes.Property:
                        PropertyInfo property = (PropertyInfo)memberInfo[0];
                        property.SetValue(component, GetValue(value, valueJson, property.PropertyType));
                        break;
                }

                return true;
            }
            catch
            {
                StringExtensions.LogError("ActionSequenceHelper", "SetMemberValue", "Could not set value");
                return false;
            }
        }

        [AutoDoc("Convert JSON to a list of parameters")]
        [AutoDocParameter("MethodInfo to invoke")]
        [AutoDocParameter("JSON parameters")]
        public static object[] ParamDataListToObjectArray(MethodInfo method, string json)
        {
            ParameterInfo[] pi = method.GetParameters();
            ParamData[] parameters = new ParamData[pi.Length];
            object[] result = new object[pi.Length];

            // Load in names and types
            for (int i = 0; i < pi.Length; i++)
            {
                parameters[i] = new ParamData()
                {
                    paramName = pi[i].Name,
                    type = pi[i].ParameterType,
                    paramType = pi[i].ParameterType.ToString(),
                };
            }

            // Load in objects
            if (!string.IsNullOrEmpty(json))
            {
                string[] parts = json.Split((char)1);
                for (int i = 0; i < parts.Length; i++)
                {
                    if (!string.IsNullOrEmpty(parts[i]))
                    {
                        ParamData pd = SimpleJson.FromJSON<ParamData>(parts[i]);
                        result[i] = GetValue(pd.paramValue, pd.paramJson, parameters[i].type);
                    }
                }
            }

            return result;
        }

#if UNITY_EDITOR
        [AutoDocSuppress]
        public static string ToJson(object value, out string objectError)
        {
            objectError = string.Empty;
            if (value == null) return string.Empty;

            if (value is UnityEngine.Object @object)
            {
                string path = UnityEditor.AssetDatabase.GetAssetPath(@object);
                path = path.Replace('\\', '/');
                int resourcesStart = path.IndexOf("resources/", StringComparison.OrdinalIgnoreCase);
                if (resourcesStart < 0)
                {
                    objectError = "Only objects in a Resources directory may be used with plugins";
                    return string.Empty;
                }
                else
                {
                    return "!!RESOURCE!!" + Path.Combine(Path.GetDirectoryName(path.Substring(resourcesStart + 10)), Path.GetFileNameWithoutExtension(path));
                }
            }

            return SimpleJson.ToJSON(value);
        }
#endif

        #endregion

    }
}