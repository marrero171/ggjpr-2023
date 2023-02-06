/* Copyright © 2014 Apex Software. All rights reserved. */

namespace Apex.AI.Visualization
{
    public sealed class SelectorActionVisualizer : ConnectorActionVisualizer
    {
        private ISelect _connectedSelector;

        public SelectorActionVisualizer(SelectorAction action, IQualifierVisualizer parent)
            : base(action, parent)
        {
        }

        public override void Init()
        {
            _connectedSelector = parent.parent.parent.FindSelector(((SelectorAction)this.action).selector.id);
        }

        public override IAction Select(IAIContext context)
        {
            var action = _connectedSelector.Select(context);
            if (action == null)
            {
                this.parent.parent.parent.PostExecute();
            }

            return action;
        }
    }
}
