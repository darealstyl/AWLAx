//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

namespace NullSave.GDTK
{
    [AutoDoc("Implements object as a uniquely identifiable object")]
    [AutoDocLocation("miscellaneous/interfaces")]
    public interface IUniquelyIdentifiable
    {

        #region Properties

        [AutoDoc("Gets the instance id for the object")]
        string instanceId { get; }

        #endregion

    }
}
