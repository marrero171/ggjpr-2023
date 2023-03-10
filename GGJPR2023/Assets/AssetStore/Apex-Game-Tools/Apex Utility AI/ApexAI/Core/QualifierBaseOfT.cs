/* Copyright © 2014 Apex Software. All rights reserved. */

namespace Apex.AI
{
    using Apex.Serialization;

    /// <summary>
    /// Base class for Qualifiers
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <seealso cref="Apex.AI.IQualifier" />
    public abstract class QualifierBase<TContext> : IQualifier where TContext : class, IAIContext
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is disabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is disabled; otherwise, <c>false</c>.
        /// </value>
        [ApexSerialization(hideInEditor = true, defaultValue = false)]
        public bool isDisabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the associated action.
        /// </summary>
        /// <value>
        /// The action.
        /// </value>
        [ApexSerialization(hideInEditor = true)]
        public IAction action
        {
            get;
            set;
        }

        /// <summary>
        /// Calculates the score for this qualifier given the provided context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The score.
        /// </returns>
        float IQualifier.Score(IAIContext context)
        {
            return Score((TContext)context);
        }

        /// <summary>
        /// Calculates the score for this qualifier given the provided context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The score.
        /// </returns>
        public abstract float Score(TContext context);
    }
}