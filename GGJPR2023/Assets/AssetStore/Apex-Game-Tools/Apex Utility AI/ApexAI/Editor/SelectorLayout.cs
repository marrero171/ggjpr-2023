/* Copyright © 2014 Apex Software. All rights reserved. */
namespace Apex.AI.Editor
{
    using UnityEditor;
    using UnityEngine;

    public sealed class SelectorLayout : ViewLayout
    {
        private SelectorView _selectorView;

        private XRange _dragArea;
        private XRange _contentArea;
        private XRange _scoreArea;
        private XRange _toggleArea;
        private XRange _anchorArea;

        public SelectorLayout(SelectorView selectorView, float windowTop, ScaleSettings scaling)
            : base(selectorView, windowTop, scaling)
        {
            _selectorView = selectorView;

            float leftMost = _localViewRange.xMin;
            float rightMost = _localViewRange.xMax;

            _dragArea = new XRange(leftMost, scaling.dragHandleWidth);
            leftMost += scaling.dragHandleWidth;

            rightMost -= scaling.anchorAreaWidth;
            _anchorArea = new XRange(rightMost, scaling.anchorAreaWidth);
            _toggleArea = new XRange(rightMost, scaling.toggleAreaWidth);

            if (EditorApplication.isPlaying)
            {
                var remainder = rightMost - leftMost;
                var scoreAreaWidth = remainder * 0.25f;

                rightMost -= scoreAreaWidth;
                _scoreArea = new XRange(rightMost, scoreAreaWidth);
            }

            _contentArea = new XRange(leftMost, rightMost - leftMost);
        }

        public SelectorView selectorView
        {
            get { return _selectorView; }
        }

        public Rect GetContentAreaLocal(float x, float y, float height)
        {
            return new Rect(x + _contentArea.xMin, y, _contentArea.width, height);
        }

        public Rect GetScoreAreaLocal(float x, float y, float height)
        {
            return new Rect(x + _scoreArea.xMin, y, _scoreArea.width, height);
        }

        public Rect GetDragAreaLocal(float x, float ypos)
        {
            return new Rect(x + _dragArea.xMin, ypos, _scaling.dragHandleWidth, _scaling.qualifierHeight);
        }

        public Rect GetAnchorAreaLocal(float x, float ypos)
        {
            return new Rect(x + _anchorArea.xMin, ypos, _scaling.anchorAreaWidth, _scaling.actionHeight);
        }

        public Rect GetToggleAreaLocal(float x, float ypos)
        {
            return new Rect(x + _toggleArea.xMin, ypos, _scaling.toggleAreaWidth, _scaling.qualifierHeight);
        }

        public QualifierHit GetQualifierAtPosition(Vector2 position)
        {
            var result = new QualifierHit(this);

            var localY = position.y - _viewRect.y - _scaling.titleHeight;
            if (localY < 0f)
            {
                return result;
            }

            var areaY = _viewRect.y + _scaling.titleHeight;
            var qcount = _selectorView.qualifierViews.Count;

            //Default index to be outside range to represent the default qualifier
            var idx = qcount > 0 ? qcount : 1;
            for (int i = 0; i < qcount; i++)
            {
                var height = _selectorView.qualifierViews[i].GetHeight(_scaling);
                if (localY < height)
                {
                    idx = i;
                    break;
                }

                localY -= height;
                areaY += height;
            }

            result.index = idx;
            result.offset = new Vector2(position.x - _viewRect.x, localY);
            return result;
        }

        public class QualifierHit
        {
            private SelectorLayout _parent;

            public int index = -1;
            public Vector2 offset;

            public QualifierHit(SelectorLayout parent)
            {
                _parent = parent;
            }

            public int clampedIndex
            {
                get { return Mathf.Clamp(index, 0, _parent._selectorView.qualifierViews.Count - 1); }
            }

            public QualifierView qualifier
            {
                get
                {
                    if (index < 0)
                    {
                        return null;
                    }

                    if (index >= _parent._selectorView.qualifierViews.Count)
                    {
                        return _parent._selectorView.defaultQualifierView;
                    }

                    return _parent._selectorView.qualifierViews[index];
                }
            }

            public bool InActionArea
            {
                get
                {
                    if (this.offset.y <= _parent._scaling.qualifierHeight)
                    {
                        return false;
                    }

                    var q = this.qualifier;
                    if (q.isExpanded)
                    {
                        return (this.offset.y > (q.GetHeight(_parent._scaling) - _parent._scaling.actionHeight));
                    }

                    return (this.offset.y > _parent._scaling.qualifierHeight);
                }
            }

            public bool InDragArea
            {
                get
                {
                    if (this.offset.y > _parent._scaling.qualifierHeight)
                    {
                        return false;
                    }

                    return _parent._dragArea.Contains(this.offset.x);
                }
            }

            public bool InAnchorArea
            {
                get
                {
                    if (!InActionArea)
                    {
                        return false;
                    }

                    return _parent._anchorArea.Contains(this.offset.x);
                }
            }

            public bool InToggle
            {
                get
                {
                    if (this.offset.y > _parent._scaling.qualifierHeight)
                    {
                        return false;
                    }

                    return _parent._toggleArea.Contains(this.offset.x);
                }
            }
        }
    }
}
