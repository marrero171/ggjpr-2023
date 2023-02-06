/* Copyright © 2014 Apex Software. All rights reserved. */

namespace Apex.AI.Editor.UndoRedo
{
    using System;
    using UnityEngine;

    public sealed class DisableOperation : IUndoRedo, IViewBoundOperation
    {
        private ICanBeDisabled _target;

        public DisableOperation(ICanBeDisabled target)
        {
            _target = target;
        }

        IView IViewBoundOperation.view
        {
            get;
            set;
        }

        void IUndoRedo.Do()
        {
            _target.isDisabled = !_target.isDisabled;
        }

        void IUndoRedo.Undo()
        {
            _target.isDisabled = !_target.isDisabled;
        }
    }
}
