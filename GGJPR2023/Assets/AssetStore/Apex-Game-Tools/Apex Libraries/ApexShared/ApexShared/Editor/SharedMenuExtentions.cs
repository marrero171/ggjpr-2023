namespace Apex.Editor
{
    using Apex.Editor.Versioning;
    using UnityEditor;

    public static class SharedMenuExtentions
    {

        [MenuItem("Tools/Apex/Upgrade", false, 300)]
        public static void CleanupMenu()
        {
            VersionUpgraderWindow.ShowWindow();
        }
    }
}
