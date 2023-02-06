/* Copyright © 2014 Apex Software. All rights reserved. */
namespace Apex.AI.Editor
{
    using System;
    using UnityEngine;

    public sealed class AILinkView : TopLevelView
    {
        private Guid _aiId;
        private string _aiName;

        public AILinkView()
        {
        }

        public AILinkView(Guid aiId, Rect viewArea)
            : base(viewArea)
        {
            _aiId = aiId;
        }

        public Guid aiId
        {
            get
            {
                return _aiId;
            }

            set
            {
                _aiId = value;
                _aiName = null;
            }
        }

        public string title
        {
            get
            {
                if (string.IsNullOrEmpty(this.name))
                {
                    return "AI Link";
                }

                return this.name;
            }
        }

        public string aiName
        {
            get
            {
                if (_aiName == null)
                {
                    var aiStorage = StoredAIs.GetById(aiId.ToString());
                    if (aiStorage != null)
                    {
                        _aiName = aiStorage.name;
                    }
                    else
                    {
                        _aiName = "Missing";
                    }
                }

                return _aiName;
            }
        }

        public void Refresh()
        {
            _aiName = null;
        }

        public override string ToString()
        {
            return this.title;
        }

        public override void RecalcHeight(ScaleSettings scaling)
        {
            var h = scaling.titleHeight + scaling.aiLinkBodyHeight;

            viewArea.height = h;
        }
    }
}
