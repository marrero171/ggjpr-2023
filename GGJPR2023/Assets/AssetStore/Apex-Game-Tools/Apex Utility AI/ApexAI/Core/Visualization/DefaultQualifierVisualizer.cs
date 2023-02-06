/* Copyright © 2014 Apex Software. All rights reserved. */

namespace Apex.AI.Visualization
{
    public sealed class DefaultQualifierVisualizer : QualifierVisualizer, IDefaultQualifier
    {
        private IDefaultQualifier _defQualifier;

        public DefaultQualifierVisualizer(IDefaultQualifier q, SelectorVisualizer parent)
            : base(q, parent)
        {
            _defQualifier = q;
        }

        /// <summary>
        /// Gets or sets the default score.
        /// </summary>
        /// <value>
        /// The score.
        /// </value>
        public float score
        {
            get
            {
                this.lastScore = _defQualifier.score;
                return _defQualifier.score;
            }

            set
            {
                _defQualifier.score = value;
            }
        }
    }
}
