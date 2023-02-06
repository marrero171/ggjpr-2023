/* Copyright © 2014 Apex Software. All rights reserved. */
namespace Apex.AI.Editor.UndoRedo
{
    public interface IUndoRedo
    {
        void Do();

        void Undo();
    }
}
