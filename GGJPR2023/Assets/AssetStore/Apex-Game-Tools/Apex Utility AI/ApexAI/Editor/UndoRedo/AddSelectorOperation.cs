/* Copyright © 2014 Apex Software. All rights reserved. */

namespace Apex.AI.Editor.UndoRedo
{
    public sealed class AddSelectorOperation : AddTopLevelViewOperation
    {
        private SelectorView _target;

        public AddSelectorOperation(AIUI ui, SelectorView target)
            : base(ui)
        {
            _target = target;
        }

        public SelectorView target
        {
            get { return _target; }
        }

        protected override void DoUndo()
        {
            _ui.RemoveSelector(_target, false);
        }

        protected override void DoRedo()
        {
            _ui.canvas.views.Add(_target);
            _ui.ai.AddSelector(_target.selector);
            _ui.Select(_target, null, null);
        }
    }
}
