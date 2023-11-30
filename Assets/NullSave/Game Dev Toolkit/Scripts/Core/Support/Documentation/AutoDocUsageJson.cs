using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocSuppress]
    public class AutoDocUsageJson
    {

        #region Fields

        public List<AutoDocUsageMethod> methods;
        public List<AutoDocUsageProperty> properties;

        #endregion

        #region Constructor

        public AutoDocUsageJson()
        {
            methods = new List<AutoDocUsageMethod>();
            properties = new List<AutoDocUsageProperty>();
        }

        #endregion

        #region Public Methods

        public AutoDocUsageMethod GetOrCreateMethodUsage(string name, string signature)
        {
            foreach(AutoDocUsageMethod method in methods)
            {
                if(method.name == name && method.signature == signature)
                {
                    return method;
                }
            }

            AutoDocUsageMethod adum = new AutoDocUsageMethod();
            adum.name = name;
            adum.signature = signature;
            adum.usage = "!! INSERT USAGE HERE";
            methods.Add(adum);

            Debug.Log("New usage entry created for: " + name + " " + signature);
            return adum;
        }

        public AutoDocUsageProperty GetOrCreatePropertyUsage(string name)
        {
            foreach (AutoDocUsageProperty p in properties)
            {
                if (p.name == name)
                {
                    return p;
                }
            }

            AutoDocUsageProperty adup = new AutoDocUsageProperty();
            adup.name = name;
            adup.usage = "!! INSERT USAGE HERE";
            properties.Add(adup);
            Debug.Log("New usage entry created for: " + name);
            return adup;
        }

        #endregion

    }
}