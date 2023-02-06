/* Copyright © 2014 Apex Software. All rights reserved. */
namespace Apex.AI.Editor.UndoRedo
{
    public interface IMergableOperation
    {
        bool TryMergeWith(IUndoRedo other, bool isBulkOperation);
    }
}
