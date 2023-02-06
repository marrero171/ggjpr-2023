/* Copyright © 2014 Apex Software. All rights reserved. */

namespace Apex.AI.Editor.UndoRedo
{
    using System;
    using UnityEngine;

    public sealed class AddQualifierOperation : AIUIOperation, IUndoRedo
    {
        private QualifierView _target;

        public AddQualifierOperation(AIUI ui, QualifierView result)
            : base(ui)
        {
            _target = result;
        }

        void IUndoRedo.Do()
        {
            _ui.AddQualifier(_target, _target.parent, false);
        }

        void IUndoRedo.Undo()
        {
            _ui.RemoveQualifier(_target, false);
        }
    }
}
