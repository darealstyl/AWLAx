//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

namespace NullSave.GDTK
{

    public enum UISelectionState
    {
        Normal = 0,
        Selected = 4,
        Highlighted = 1,
        Pressed = 2,
        Disabled = 3
    }

    public enum UITransition
    {
        None = 0,
        ColorTint = 1,
        SpriteSwap = 2,
        Animation = 3
    }

}