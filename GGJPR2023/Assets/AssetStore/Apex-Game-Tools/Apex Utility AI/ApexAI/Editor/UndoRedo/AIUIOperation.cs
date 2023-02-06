/* Copyright © 2014 Apex Software. All rights reserved. */
namespace Apex.AI.Editor.UndoRedo
{
    public abstract class AIUIOperation
    {
        protected AIUI _ui;

        public AIUIOperation(AIUI ui)
        {
            _ui = ui;
        }
    }
}
