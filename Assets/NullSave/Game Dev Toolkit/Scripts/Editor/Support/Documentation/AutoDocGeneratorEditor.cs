using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(AutoDocGenerator))]
    public class AutoDocGeneratorEditor : GDTKEditor
    {

        #region Fields

        private List<string> excludedMethods;
        private List<string> excludedProperties;
        private List<string> excludeDlls;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            excludedMethods = new List<string>();
            excludedMethods.Add("Awake");
            excludedMethods.Add("Update");
            excludedMethods.Add("FixedUpdate");
            excludedMethods.Add("Reset");
            excludedMethods.Add("Start");
            excludedMethods.Add("OnEnable");
            excludedMethods.Add("OnDisable");
            excludedMethods.Add("OnDestroy");
            excludedMethods.Add("LateUpdate");

            excludedMethods.Add("OnDrawGizmos");

            Type t = typeof(MonoBehaviour);
            foreach (MethodInfo mi in t.GetMethods())
            {
                excludedMethods.Add(mi.Name);
            }

            excludedProperties = new List<string>();
            foreach (PropertyInfo pi in t.GetProperties())
            {
                excludedProperties.Add(pi.Name);
            }
        }

        public override void OnInspectorGUI()
        {
            string path;

            MainContainerBegin();


            SimpleProperty("documentNamespace");
            SimpleProperty("stubImage");
            if (SimpleValue<bool>("stubImage"))
            {
                SimpleProperty("imageRoot");
            }
            SimpleProperty("usages");
            if (SimpleValue<bool>("usages"))
            {
                SimpleProperty("usageDir");
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Browse"))
                {
                    GUI.FocusControl(null);
                    path = EditorUtility.OpenFolderPanel("Usasge Directory", Path.Combine(Application.dataPath, SimpleValue<string>("usageDir")), "");
                    if (!string.IsNullOrEmpty(path))
                    {
                        path = path.Substring(Application.dataPath.Length);
                        if (path.StartsWith("/") || path.StartsWith("\\"))
                        {
                            path = path.Substring(1);
                        }
                        serializedObject.FindProperty("usageDir").stringValue = path;
                    }
                }
                GUILayout.EndHorizontal();

            }
            SimpleProperty("outputDir");
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Browse"))
            {
                GUI.FocusControl(null);
                path = EditorUtility.OpenFolderPanel("Output Directory", Path.Combine(Application.dataPath, SimpleValue<string>("outputDir")), "");
                if (!string.IsNullOrEmpty(path))
                {
                    path = path.Substring(Application.dataPath.Length);
                    if (path.StartsWith("/") || path.StartsWith("\\"))
                    {
                        path = path.Substring(1);
                    }
                    serializedObject.FindProperty("outputDir").stringValue = path;
                }
            }
            GUILayout.EndHorizontal();

            StringList("excludeDlls");

            GUILayout.Space(12);
            if (GUILayout.Button("Generate"))
            {
                GUI.FocusControl(null);
                GenerateDocumentation();
            }

            MainContainerEnd();
        }

        #endregion

        #region Private Methods

        private void BuildDoc(Type t, string root)
        {
            string niceName = ObjectNames.NicifyVariableName(t.Name);
            Dictionary<string, List<MethodInfo>> methods = GetNonUnityMethods(t);
            List<PropertyInfo> properties = GetProperties(t);
            string webName = niceName.Replace(" ", "-").ToLower();
            bool needsSubDir = methods.Count > 0 || properties.Count > 0;
            string dir;
            StringBuilder sb = new StringBuilder();
            string tmp;
            AutoDocUsageJson usage = null;
            string usageRoot = Path.Combine(Application.dataPath, SimpleValue<string>("usageDir"));
            string usageFile = Path.Combine(usageRoot, t.FullName.Replace(".", "_") + "-Usage.json");

            // Usage
            if (SimpleBool("usages"))
            {
                if (!Directory.Exists(usageRoot))
                {
                    Directory.CreateDirectory(usageRoot);
                }

                if (File.Exists(usageFile))
                {
                    usage = SimpleJson.FromJSON<AutoDocUsageJson>(File.ReadAllText(usageFile));
                }
                else
                {
                    usage = new AutoDocUsageJson();
                }
            }

            // Loation
            string customDir = GetCustomDirectory(t);
            if (!string.IsNullOrEmpty(customDir))
            {
                dir = needsSubDir ? Path.Combine(root, customDir + "/" + webName) : Path.Combine(root, customDir);
            }
            else
            {
                dir = needsSubDir ? Path.Combine(root, webName) : root;
            }

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            if (needsSubDir)
            {
                CreateCategoryJson(dir, niceName);

                sb.Append("---\r\nsidebar_position: 1\r\n---\r\n\r\n");
                sb.Append("# Introduction\r\n");

                sb.Append(GetAutoDocDescription(t) + "\r\n\r\n");
            }
            else
            {
                sb.Append("# " + niceName + "\r\n");
                sb.Append(GetAutoDocDescription(t) + "\r\n\r\n");
            }

            AutoDocYouTube ady = t.GetCustomAttribute<AutoDocYouTube>();
            if (ady != null)
            {
                sb.Append("<iframe width=\"" + ady.width + "\" height=\"" + ady.height + "\" src=\"https://www.youtube.com/embed/" + ady.source + "\" title=\"YouTube video player\" frameborder=\"0\" allow=\"accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share\" allowfullscreen=\"true\"></iframe>\r\n\r\n");
            }

            if (SimpleValue<bool>("stubImage"))
            {
                if (TypeEmbedsImage(t))
                {
                    sb.Append("![\"" + niceName + "\"](" + SimpleValue<string>("imageRoot") + webName + ".png)\r\n\r\n");
                }
            }

            FieldInfo[] fi = t.GetFields();
            if (fi.Count() > 0)
            {
                sb.Append("### Fields\r\n|Name|Description|\r\n|---|---|\r\n");
                foreach (FieldInfo field in fi)
                {
                    if (field.GetCustomAttribute<AutoDocSuppress>() == null && !(field.IsInitOnly && field.IsLiteral))
                    {
                        sb.Append("|" + GetFieldName(field) + "|" + GetUnityTooltip(field) + "|\r\n");
                    }
                }
            }

            // Properties
            if (properties.Count > 0)
            {
                CreatePropertiesDirectory(dir);
                sb.Append("### Properties\r\n|Name|Description|\r\n|---|---|\r\n");
                foreach (PropertyInfo property in properties)
                {
                    tmp = GetAutoDocDescription(property);
                    sb.Append("|[" + property.Name + "](properties/" + property.Name.ToLower() + ")|" + tmp + "|\r\n");
                    BuildDocs_Property(t, property, tmp, dir, usage);
                }
            }

            // Methods
            if (methods.Count > 0)
            {
                CreateMethodsDirectory(dir);
                sb.Append("### Methods\r\n|Name|Description|\r\n|---|---|\r\n");
                foreach (var entry in methods)
                {
                    sb.Append("|[" + entry.Key + "](methods/" + entry.Key + ")|" + GetAutoDocDescription(entry.Value[0]) + "|\r\n");
                    BuildDocs_Method(t, entry.Value, dir, usage);
                }
            }

            if (needsSubDir && SimpleBool("usages"))
            {
                string json = SimpleJson.ToJSON(usage, false, true);
                using (FileStream fs = new FileStream(usageFile, FileMode.Create, FileAccess.Write))
                {
                    fs.WriteString(json);
                }
            }

            if (needsSubDir)
            {
                using (FileStream fs = new FileStream(Path.Combine(dir, "introduction.md"), FileMode.Create, FileAccess.Write))
                {
                    fs.WriteString(sb.ToString());
                };
            }
            else
            {
                using (FileStream fs = new FileStream(Path.Combine(dir, webName + ".md"), FileMode.Create, FileAccess.Write))
                {
                    fs.WriteString(sb.ToString());
                };
            }
        }

        private void BuildDocs_Method(Type t, List<MethodInfo> signatures, string root, AutoDocUsageJson usage)
        {
            string path = Path.Combine(Path.Combine(root, "methods"), signatures[0].Name.ToLower() + ".md");
            string usageSignature;
            StringBuilder sb = new StringBuilder();
            sb.Append("---\r\ntitle: \"" + signatures[0].Name + "\"\r\n---\r\n\r\n");
            sb.Append("# " + t.Name + "." + signatures[0].Name + "\r\n\r\n");

            foreach (MethodInfo signature in signatures)
            {
                sb.Append("## Declaration\r\n```csharp\r\n");

                sb.Append("public ");
                if (signature.IsStatic)
                {
                    sb.Append("static ");
                }
                if (signature.IsAbstract)
                {
                    sb.Append("abstract ");
                }
                if (signature.IsVirtual)
                {
                    sb.Append("virtual ");
                }

                string returnType = GetFriendlyTypeName(signature.ReturnType);
                sb.Append(returnType + " ");
                sb.Append(signature.Name + "(");

                ParameterInfo[] parameters = signature.GetParameters();
                usageSignature = string.Empty;
                if (parameters.Count() > 0)
                {
                    StringBuilder sbParams = new StringBuilder();
                    sbParams.Append("\r\n### Parameters\r\n");
                    sbParams.Append("|Name|Description|\r\n|---|---|\r\n");

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        if (i > 0)
                        {
                            sb.Append(", ");
                            usageSignature += ", ";
                        }
                        usageSignature += parameters[i].ParameterType;
                        sb.Append(GetFriendlyTypeName(parameters[i].ParameterType) + " " + parameters[i].Name);
                        sbParams.Append("|" + parameters[i].Name + "|" + GetAutoDocParameter(signature, i) + "|\r\n");
                    }

                    sb.Append(")\r\n");
                    sb.Append("```\r\n\r\n");

                    sb.Append(sbParams.ToString());
                    sb.Append("\r\n");

                }
                else
                {
                    sb.Append(")\r\n");
                    sb.Append("```\r\n\r\n");
                }

                sb.Append("### Description\r\n");
                sb.Append(GetAutoDocDescription(signature) + "\r\n\r\n");

                if (SimpleBool("usages"))
                {
                    sb.Append("### Usage\r\n");
                    sb.Append(usage.GetOrCreateMethodUsage(signature.Name, usageSignature).usage + "\r\n");
                }
            }

            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                fs.WriteString(sb.ToString());
            };

        }

        private void BuildDocs_Property(Type t, PropertyInfo property, string description, string root, AutoDocUsageJson usage)
        {
            bool showGet = false;
            bool showSet = false;
            string returnType = "void";
            string extraAccessor = string.Empty;
            string path = Path.Combine(Path.Combine(root, "properties"), property.Name.ToLower() + ".md");

            StringBuilder sb = new StringBuilder();
            sb.Append("---\r\ntitle: \"" + property.Name + "\"\r\n---\r\n\r\n");
            sb.Append("# " + t.Name + "." + property.Name + "\r\n\r\n");

            // Build property signature
            foreach (var accessor in property.GetAccessors())
            {
                if (accessor.Name.StartsWith("set_"))
                {
                    if (accessor.IsPublic)
                    {
                        showSet = true;
                        if (accessor.IsStatic) extraAccessor = "static ";
                    }
                }
                else if (accessor.Name.StartsWith("get_"))
                {
                    if (accessor.IsPublic)
                    {
                        showGet = true;
                        if (accessor.IsStatic) extraAccessor = "static ";
                        returnType = GetFriendlyTypeName(accessor.ReturnType);
                    }
                }
            }

            sb.Append("```csharp\r\npublic ");
            if (!string.IsNullOrEmpty(extraAccessor)) sb.Append(extraAccessor);
            sb.Append(returnType + " " + property.Name + " { ");
            if (showGet) sb.Append("get; ");
            if (showSet) sb.Append("set; ");
            sb.Append("}\r\n");
            sb.Append("```\r\n\r\n");

            sb.Append("### Description\r\n");
            sb.Append(description + "\r\n\r\n");

            if (SimpleBool("usages"))
            {
                sb.Append("### Usage\r\n");
                sb.Append(usage.GetOrCreatePropertyUsage(property.Name).usage + "\r\n");
            }

            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                fs.WriteString(sb.ToString());
            };

        }

        private void CreateCategoryJson(string dir, string label)
        {
            using (FileStream fs = new FileStream(Path.Combine(dir, "_category_.json"), FileMode.Create, FileAccess.Write))
            {
                fs.WriteString("{\r\n\t\"label\": \"" + label + "\"\r\n}");
            };
        }

        private void CreateCategoryJson(string dir, string label, int position)
        {
            using (FileStream fs = new FileStream(Path.Combine(dir, "_category_.json"), FileMode.Create, FileAccess.Write))
            {
                fs.WriteString("{\r\n\t\"label\": \"" + label + "\",\r\n\t\"position\": " + position + "\r\n}");
            };
        }

        private void CreateMethodsDirectory(string root)
        {
            string dir = Path.Combine(root, "methods");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            CreateCategoryJson(dir, "Methods");
        }

        private void CreatePropertiesDirectory(string root)
        {
            string dir = Path.Combine(root, "properties");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            CreateCategoryJson(dir, "Properties", 2);
        }

        private void GenerateDocumentation()
        {
            string ns = SimpleValue<string>("documentNamespace");
            string root = Path.Combine(Application.dataPath, SimpleValue<string>("outputDir"));

            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            excludeDlls = ((AutoDocGenerator)target).excludeDlls.ToList();

            foreach (Type type in AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.Namespace != null && type.Namespace == ns))
            {

                if (!ShouldSuppress(type))
                {
                    //Debug.Log("Documenting: " + type.AssemblyQualifiedName);
                    BuildDoc(type, root);
                }
            }

            AssetDatabase.Refresh();
        }

        private string GetAutoDocDescription(MethodInfo method)
        {
            AutoDoc ad = method.GetCustomAttribute<AutoDoc>();
            if (ad != null) return ad.Description;

            Debug.Log("No description for " + method.DeclaringType.AssemblyQualifiedName + "." + method.Name);
            return "!! INSERT DESCRIPTION HERE";
        }

        private string GetAutoDocDescription(PropertyInfo property)
        {
            AutoDoc ad = property.GetCustomAttribute<AutoDoc>();
            if (ad != null) return ad.Description;

            Debug.Log("No description for " + property.DeclaringType.AssemblyQualifiedName + "." + property.Name);
            return "!! INSERT DESCRIPTION HERE";
        }

        private string GetAutoDocDescription(Type t)
        {
            AutoDoc ad = t.GetCustomAttribute<AutoDoc>();
            if (ad != null) return ad.Description;

            Debug.Log("No description for " + t.AssemblyQualifiedName);
            return "!! INSERT DESCRIPTION HERE";
        }

        private string GetAutoDocParameter(MethodInfo method, int index)
        {
            int count = -1;

            foreach (var attrib in method.CustomAttributes)
            {
                if (attrib.AttributeType == typeof(AutoDocParameter))
                {
                    count += 1;
                    if (count == index)
                    {
                        return (string)attrib.ConstructorArguments[0].Value;
                    }
                }
            }

            Debug.Log("No description for " + method.DeclaringType.AssemblyQualifiedName + "." + method.Name + " property index " + index);
            return "!! INSERT DESCRIPTION HERE";
        }

        private string GetCustomDirectory(Type type)
        {
            AutoDocLocation adl = type.GetCustomAttribute<AutoDocLocation>();
            if (adl != null)
            {
                return adl.Location;
            }

            return null;
        }

        private string GetFieldName(FieldInfo field)
        {
            AutoDocAs ad = field.GetCustomAttribute<AutoDocAs>();
            if (ad != null) return ad.Name;

            return ObjectNames.NicifyVariableName(field.Name);
        }

        private string GetFriendlyTypeName(Type t)
        {
            if (t == typeof(bool) || t == typeof(Boolean))
            {
                return "bool";
            }

            if (t == typeof(float))
            {
                return "float";
            }

            if (t == typeof(void))
            {
                return "void";
            }

            if (t == typeof(string))
            {
                return "string";
            }

            if (t.IsGenericType)
            {
                int paramCountIndex = t.Name.IndexOf('`');
                int paramCount = int.Parse(t.Name.Substring(paramCountIndex + 1));
                string result = t.Name.Substring(0, paramCountIndex) + "<";

                for (int i = 0; i < paramCount; i++)
                {
                    if (i > 0)
                    {
                        result += ", ";
                    }
                    result += GetFriendlyTypeName(t.GetGenericArguments()[i]);
                }

                result += ">";

                return result;
            }

            return t.Name;
        }

        private Dictionary<string, List<MethodInfo>> GetNonUnityMethods(Type t)
        {
            Dictionary<string, List<MethodInfo>> result = new Dictionary<string, List<MethodInfo>>();
            bool excludeBase = t.GetCustomAttribute<AutoDocExcludeBase>() != null;

            foreach (MethodInfo method in t.GetMethods())
            {
                if (!excludeBase || method.GetBaseDefinition().DeclaringType == t)
                {
                    if (!method.IsSpecialName && !excludedMethods.Contains(method.Name) && method.GetCustomAttribute<AutoDocSuppress>() == null)
                    {
                        if (result.ContainsKey(method.Name))
                        {
                            result[method.Name].Add(method);
                        }
                        else
                        {
                            List<MethodInfo> list = new List<MethodInfo>();
                            list.Add(method);
                            result.Add(method.Name, list);
                        }
                    }
                }
            }

            return result;
        }

        private List<PropertyInfo> GetProperties(Type t)
        {
            List<PropertyInfo> result = new List<PropertyInfo>();
            bool excludeBase = t.GetCustomAttribute<AutoDocExcludeBase>() != null;

            foreach (PropertyInfo prop in t.GetProperties())
            {
                if (!excludeBase || prop.DeclaringType == t)
                {
                    if (!excludedProperties.Contains(prop.Name) && !ShouldSuppress(prop))
                    {
                        result.Add(prop);
                    }
                }
            }

            return result;
        }

        private string GetUnityTooltip(FieldInfo field)
        {
            TooltipAttribute ta = field.GetCustomAttribute<TooltipAttribute>();
            if (ta != null) return ta.tooltip;

            AutoDoc ad = field.GetCustomAttribute<AutoDoc>();
            if (ad != null) return ad.Description;

            AutoDocAs ada = field.GetCustomAttribute<AutoDocAs>();
            if (ada != null) return ada.Description;

            return "!! INSERT DESCRIPTION HERE";
        }

        private bool ShouldSuppress(Type target)
        {
            if (target.AssemblyQualifiedName.Contains("<")) return true;
            if (target.AssemblyQualifiedName.Contains("+")) return true;
            if (excludeDlls.Contains(target.Assembly.GetName().Name)) return true;
            if (target.IsSubclassOf(typeof(Editor))) return true;
            if (target.IsSubclassOf(typeof(UniversalPluginEditor))) return true;
            if (target.IsSubclassOf(typeof(PopupWindowContent))) return true;
            if (target.IsSubclassOf(typeof(EditorWindow))) return true;
            if (target.IsSubclassOf(typeof(ToolDefinition))) return true;
            if (target.IsSubclassOf(typeof(PropertyDrawer))) return true;
            if (target.IsEnum || target.IsValueType) return true;

            foreach (var attrib in target.CustomAttributes)
            {
                if (attrib.AttributeType == typeof(AutoDocSuppress))
                {
                    return true;
                }
            }

            return false;
        }

        private bool ShouldSuppress(PropertyInfo target)
        {
            foreach (var attrib in target.CustomAttributes)
            {
                if (attrib.AttributeType == typeof(AutoDocSuppress))
                {
                    return true;
                }
            }

            return false;
        }

        public bool TypeEmbedsImage(Type t)
        {
            if (t.IsAssignableFrom(typeof(MonoBehaviour)) || t.IsAssignableFrom(typeof(ScriptableObject)) || t.IsAssignableFrom(typeof(UniversalPlugin)) || t.IsAssignableFrom(typeof(ActionSequencePlugin)))
            {
                return true;
            }

            if (t.BaseType == null || t.BaseType == typeof(System.Object)) return false;

            Type tbase = t.BaseType;
            while (tbase.BaseType != null && t.BaseType != typeof(System.Object))
            {
                tbase = tbase.BaseType;
            }

            if (tbase.IsAssignableFrom(typeof(MonoBehaviour)) || tbase.IsAssignableFrom(typeof(ScriptableObject)) || tbase.IsAssignableFrom(typeof(UniversalPlugin)) || tbase.IsAssignableFrom(typeof(ActionSequencePlugin)))
            {
                return true;
            }

            return false;
        }

        #endregion

    }
}