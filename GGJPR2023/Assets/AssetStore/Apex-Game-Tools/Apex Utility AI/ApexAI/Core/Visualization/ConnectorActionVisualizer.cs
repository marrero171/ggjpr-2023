/* Copyright © 2014 Apex Software. All rights reserved. */
namespace Apex.AI.Visualization
{
    public abstract class ConnectorActionVisualizer : ActionVisualizer, IConnectorAction
    {
        public ConnectorActionVisualizer(IAction action, IQualifierVisualizer parent)
            : base(action, parent)
        {
        }

        public override void Execute(IAIContext context, bool doCallback)
        {
            /* NOOP */
        }

        public abstract IAction Select(IAIContext context);
    }
}
