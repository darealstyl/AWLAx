using System.Collections.Generic;
using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocLocation("auto-documentation")]
    [AutoDoc("This component automatically generates markdown documentation for a project.")]
    public class AutoDocGenerator : MonoBehaviour
    {

        #region Fields

        [AutoDocAs("Document Namespace")] [Tooltip("Namespace of items to auto document")] public string documentNamespace;
        [AutoDocAs("Stub Image")] [Tooltip("Create image stubs")] public bool stubImage;
        [AutoDocAs("Image Root")] [Tooltip("Image stub root")] public string imageRoot;
        [AutoDocAs("Output Directory")] [Tooltip("Directory to use when creating documentation")] public string outputDir;
        [AutoDocAs("Usages")] [Tooltip("Create usage entries for methods and properties")] public bool usages;
        [AutoDocAs("Usage Directory")] [Tooltip("Directory storing usage data")] public string usageDir;

        [AutoDoc("List of dlls to exclude from documentation")] public List<string> excludeDlls;

        #endregion

        #region Unity Methods

        private void Reset()
        {
            outputDir = "/output/";
            usages = true;
            usageDir = "/usage/";
        }

        #endregion

    }
}