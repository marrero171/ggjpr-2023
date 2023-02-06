/* Copyright © 2014 Apex Software. All rights reserved. */
namespace Apex.AI.Editor
{
    using UnityEngine;

    public abstract class TopLevelView : IView
    {
        public Rect viewArea;
        public AIUI parent;

        protected TopLevelView()
        {
        }

        protected TopLevelView(Rect viewArea)
        {
            this.viewArea = viewArea;
        }

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

        public bool isSelected
        {
            get { return parent.selectedViews.Contains(this); }
        }

        AIUI IView.parentUI
        {
            get { return this.parent; }
        }

        public Vector3 ConnectorAnchorIn(ScaleSettings scaling)
        {
            return new Vector3(this.viewArea.x, this.viewArea.y + (scaling.titleHeight * 0.5f), 0f);
        }

        public void Scale(float oldScale, float newScale)
        {
            this.viewArea = this.viewArea.Scale(oldScale, newScale);
        }

        public virtual void RecalcHeight(ScaleSettings scaling)
        {
            var h = scaling.titleHeight;

            viewArea.height = h;
        }
    }
}
