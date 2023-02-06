/* Copyright © 2014 Apex Software. All rights reserved. */
namespace Apex.AI.Editor.Reflection
{
    using System;

    public sealed class EditorItem
    {
        public object item;
        public string name;
        public EditorFieldCategory[] fieldCategories;
        public DependencyChecker dependencyChecker;

        public void Render(AIInspectorState state)
        {
            for (int i = 0; i < fieldCategories.Length; i++)
            {
                fieldCategories[i].Render(state, dependencyChecker);
            }
        }
    }
}
