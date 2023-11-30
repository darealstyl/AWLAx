using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace NullSave.GDTK
{
    [AutoDocSuppress]
    [InitializeOnLoad]
    public class DefinitionsManager
    {

        #region Constructor

        static DefinitionsManager()
        {
            CreateDefinitions();
        }

        #endregion

        #region Public Methods

        public static void CreateDefinitions()
        {
            string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            List<string> allDefines = definesString.Split(';').ToList();
            List<string> allNullSaveDefines = new List<string>();

            var denitionsType = GetAllDefinitions();
            foreach (var t in denitionsType)
            {
                var value = t.InvokeMember(null, BindingFlags.DeclaredOnly |
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.CreateInstance, null, null, null);

                List<string> list = null;
                try
                {
                    list = (List<string>)t.InvokeMember("GetSymbols", BindingFlags.DeclaredOnly |
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty, null, value, null);
                    if (list != null)
                    {
                        allNullSaveDefines.AddRange(list.Except(allNullSaveDefines));
                    }
                }
                catch { }
            }

            var allDefiniesToAdd = allNullSaveDefines.FindAll(s => !allDefines.Contains(s));
            if (allDefiniesToAdd.Count > 0)
            {
                AddDefinitionSymbols(allDefiniesToAdd, allDefines);
            }

        }

        public static void RemoveDefinition(string definition)
        {
            string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            List<string> allDefines = definesString.Split(';').ToList();
            if (!allDefines.Contains(definition)) return;
            allDefines.Remove(definition);

            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup,
                string.Join(";", allDefines.ToArray()));
        }

        public static void SetDefinition(string definition)
        {
            string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            List<string> allDefines = definesString.Split(';').ToList();
            if (allDefines.Contains(definition)) return;
            List<string> addDefines = new List<string>();
            addDefines.Add(definition);
            AddDefinitionSymbols(addDefines, allDefines);
        }

        #endregion

        #region Private Methods

        static void AddDefinitionSymbols(List<string> targetDefineSymbols, List<string> currentDefineSymbols)
        {
            bool needUpdate = false;
            for (int i = 0; i < targetDefineSymbols.Count; i++)
            {
                if (!currentDefineSymbols.Contains(targetDefineSymbols[i]))
                {
                    needUpdate = true; break;
                }
            }
            currentDefineSymbols.AddRange(targetDefineSymbols.Except(currentDefineSymbols));
            if (needUpdate)
                PlayerSettings.SetScriptingDefineSymbolsForGroup(
                    EditorUserBuildSettings.selectedBuildTargetGroup,
                    string.Join(";", currentDefineSymbols.ToArray()));
        }

        static List<Type> GetAllDefinitions()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                 .Where(x => typeof(ToolDefinition).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).ToList();
        }

        #endregion

    }
}
