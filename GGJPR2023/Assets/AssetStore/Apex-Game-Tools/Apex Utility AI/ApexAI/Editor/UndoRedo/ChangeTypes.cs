/* Copyright © 2014 Apex Software. All rights reserved. */
namespace Apex.AI.Editor.UndoRedo
{
    using System;

    [Flags]
    public enum ChangeTypes
    {
        None = 0,
        Pan = 1,
        Resize = 2,
        Move = 4,
        Zoom = 8,
        Undoable = 128
    }
}
