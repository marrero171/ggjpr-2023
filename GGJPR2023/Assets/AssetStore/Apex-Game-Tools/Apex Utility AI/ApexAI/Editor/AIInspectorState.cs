namespace Apex.AI.Editor
{
    using Apex.AI.Editor.Reflection;
    using Apex.AI.Visualization;
    using UnityEditor;
    using UnityEngine;

    public class AIInspectorState : ScriptableObject
    {
        private AIUI _ui;

        public AIUI currentAIUI
        {
            get { return _ui; }
            set { _ui = value; }
        }

        public EditorItem editedItem
        {
            get;
            private set;
        }

        public AILinkView currentAILink
        {
            get { return _ui.currentAILink; }
        }

        public SelectorView currentSelector
        {
            get { return _ui.currentSelector; }
        }

        public QualifierView currentQualifier
        {
            get { return _ui.currentQualifier; }
        }

        public ActionView currentAction
        {
            get { return _ui.currentAction; }
        }

        public int selectedCount
        {
            get { return _ui.selectedViews.Count; }
        }

        public void MarkDirty()
        {
            if (_ui != null)
            {
                _ui.isDirty = true;
            }
        }

        public void Refresh()
        {
            if (_ui.currentAction != null)
            {
                UpdateCurrent(_ui.currentAction.action);
            }
            else if (_ui.currentQualifier != null)
            {
                UpdateCurrent(_ui.currentQualifier.qualifier);
            }
            else if (_ui.currentSelector != null)
            {
                UpdateCurrent(_ui.currentSelector.selector);
            }
            else
            {
                UpdateCurrent(null);
            }
        }

        private void UpdateCurrent(object item)
        {
            var visualizedItem = item as IVisualizedObject;
            if (visualizedItem != null)
            {
                this.editedItem = ReflectMaster.Reflect(visualizedItem.target);
            }
            else if (item != null)
            {
                this.editedItem = ReflectMaster.Reflect(item);
            }
            else
            {
                this.editedItem = null;
            }

            Selection.activeObject = this;

            var inspector = AIInspectorEditor.instance;
            if (inspector != null)
            {
                inspector.Repaint();
            }
        }
    }
}