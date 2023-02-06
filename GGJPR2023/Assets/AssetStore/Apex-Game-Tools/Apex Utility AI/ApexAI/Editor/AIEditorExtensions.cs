/* Copyright © 2014 Apex Software. All rights reserved. */
namespace Apex.AI.Editor
{
    using System;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    public static class AIEditorExtensions
    {
        public static void Select(this IView view)
        {
            view.parentUI.selectedView = view;
        }

        public static Rect Scale(this Rect area, float scale)
        {
            return new Rect(area.x * scale, area.y * scale, area.width * scale, area.height * scale);
        }

        public static Rect Scale(this Rect area, float oldScale, float newScale)
        {
            var scale = newScale / oldScale;
            return new Rect(area.x * scale, area.y * scale, area.width * scale, area.height * scale);
        }

        public static Vector2 Scale(this Vector2 v, float oldScale, float newScale)
        {
            var scale = newScale / oldScale;
            return new Vector2(v.x * scale, v.y * scale);
        }

        public static RectOffset Scale(this RectOffset r, float scale)
        {
            return new RectOffset((int)(r.left * scale), (int)(r.right * scale), (int)(r.top * scale), (int)(r.bottom * scale));
        }

        public static void SetTitle(this EditorWindow win, string title, Texture2D icon)
        {
#if UNITY_5 || UNITY_2017
            win.titleContent = new GUIContent(title, icon);
#else
            win.title = title;

            //Set the tab icon. Since texture are destroyed between playmode changes, we have to reapply this.
            var propertyInfo = typeof(EditorWindow).GetProperty("cachedTitleContent", BindingFlags.Instance | BindingFlags.NonPublic);
            if (propertyInfo != null)
            {
                var style = (GUIContent)propertyInfo.GetValue(win, null);
                style.image = icon;
            }
#endif
        }
    }
}
