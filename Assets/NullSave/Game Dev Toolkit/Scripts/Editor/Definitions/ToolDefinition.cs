using System.Collections.Generic;

namespace NullSave.GDTK
{

    [AutoDocSuppress]
    public abstract class ToolDefinition
    {
        public abstract List<string> GetSymbols
        {
            get;
        }
    }

}