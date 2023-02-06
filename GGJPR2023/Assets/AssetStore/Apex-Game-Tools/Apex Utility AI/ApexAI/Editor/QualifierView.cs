/* Copyright © 2014 Apex Software. All rights reserved. */

namespace Apex.AI.Editor
{
    using System;
    using Apex.AI.Visualization;
    using UnityEditor;

    public sealed class QualifierView : IView
    {
        private IQualifier _qualifier;
        private Type _qualifierType;
        private bool _expanded;

        public string name
        {
            get;
            set;
        }

        public string description
        {
            get;
            set;
        }

        public string friendlyName
        {
            get
            {
                if (string.IsNullOrEmpty(this.name) && _qualifierType != null)
                {
                    return DisplayHelper.GetFriendlyName(_qualifierType);
                }

                return this.name;
            }
        }

        public string friendlyDescription
        {
            get
            {
                if (string.IsNullOrEmpty(this.description))
                {
                    return "Right Click for Options";
                }

                return this.description;
            }
        }

        public IQualifier qualifier
        {
            get
            {
                return _qualifier;
            }

            set
            {
                _qualifier = value;

                var qv = _qualifier as IQualifierVisualizer;
                _qualifierType = qv == null ? _qualifier.GetType() : qv.qualifier.GetType();
            }
        }

        AIUI IView.parentUI
        {
            get { return this.parent.parent; }
        }

        public SelectorView parent
        {
            get;
            set;
        }

        public ActionView actionView
        {
            get;
            set;
        }

        public bool isExpanded
        {
            get
            {
                return _expanded && (this.qualifier is CompositeQualifierVisualizer);
            }

            set
            {
                if (value && UserSettings.instance.visualDebug && EditorApplication.isPlaying)
                {
                    var vq = this.qualifier as CompositeQualifierVisualizer;
                    if (vq != null)
                    {
                        _expanded = value;
                    }
                }
                else
                {
                    _expanded = false;
                }
            }
        }

        public bool isDefault
        {
            get { return _qualifier is IDefaultQualifier; }
        }

        public bool isHighScorer
        {
            get
            {
                var vq = _qualifier as IQualifierVisualizer;
                if (vq != null)
                {
                    return vq.isHighScorer;
                }

                return false;
            }
        }

        public override string ToString()
        {
            return this.friendlyName;
        }

        public static QualifierView Create(Type qualifierType)
        {
            if (!typeof(IQualifier).IsAssignableFrom(qualifierType))
            {
                throw new ArgumentException("The proposed type is not a Qualifier.", "qualifierType");
            }

            var qv = new QualifierView();
            qv.qualifier = Activator.CreateInstance(qualifierType) as IQualifier;

            return qv;
        }

        public float GetHeight(ScaleSettings scaling)
        {
            var height = scaling.qualifierHeight + scaling.actionHeight;

            if (this.isExpanded)
            {
                var sq = this.qualifier as ICompositeVisualizer;
                height += sq.children.Count * scaling.scorerHeight;
            }

            return height;
        }

        public void Reconnect(IQualifier q)
        {
            this.qualifier = q;

            if (this.actionView != null)
            {
                //If an action view is present bug the action is null, this means inconsistency between config end editor config
                //While this should not occur, we still handle it just in case.
                if (q.action == null)
                {
                    this.actionView = null;
                }
                else
                {
                    this.actionView.action = q.action;
                }
            }
        }

        public void PruneBrokenConnections()
        {
            var av = this.actionView as IConnectorActionView;

            if (av != null && (av.targetView == null || object.ReferenceEquals(av.targetView, this.parent)))
            {
                var cav = av as CompositeActionView;
                if (cav != null)
                {
                    ((CompositeAction)cav.action).connectorAction = null;
                    cav.targetView = null;
                }
                else
                {
                    this.actionView = null;
                    this.qualifier.action = null;
                }
            }
        }
    }
}