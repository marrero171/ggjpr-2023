/* Copyright © 2014 Apex Software. All rights reserved. */

namespace Apex.AI.Editor.UndoRedo
{
    public sealed class AddAILinkOperation : AddTopLevelViewOperation
    {
        private AILinkView _target;

        public AddAILinkOperation(AIUI ui, AILinkView target)
            : base(ui)
        {
            _target = target;
        }

        protected override void DoUndo()
        {
            _ui.RemoveAILink(_target, false);
        }

        protected override void DoRedo()
        {
            _ui.canvas.views.Add(_target);
            _ui.Select(_target);
        }
    }
}
