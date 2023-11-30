//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System;

namespace NullSave.GDTK
{
    [AutoDocExcludeBase]
    [AutoDocLocation("auto-documentation/attributes")]
    [AutoDoc("Exclude class, property, method or field from auto documentation")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Delegate)]
    public class AutoDocSuppress : Attribute { }
}
