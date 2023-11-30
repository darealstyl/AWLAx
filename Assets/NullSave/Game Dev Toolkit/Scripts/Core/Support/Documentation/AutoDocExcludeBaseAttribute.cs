//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System;

namespace NullSave.GDTK
{
    [AutoDocExcludeBase]
    [AutoDocLocation("auto-documentation/attributes")]
    [AutoDoc("Exclude base methods and properties from auto documentation")]
    [AttributeUsage(AttributeTargets.Class)]
    public class AutoDocExcludeBase : Attribute { }
}