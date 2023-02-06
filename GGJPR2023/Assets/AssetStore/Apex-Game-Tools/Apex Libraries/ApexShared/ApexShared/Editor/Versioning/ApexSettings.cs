/* Copyright © 2014 Apex Software. All rights reserved. */
namespace Apex.Editor.Versioning
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    public class ApexSettings : ScriptableObject
    {
        private static ApexSettings _instance;

        [SerializeField, HideInInspector]
        private bool _allowAutomaticUpdateCheck;

        [SerializeField, HideInInspector]
        private int _updateCheckIntervalHours = 12;

        [SerializeField, HideInInspector]
        private string _lastUpdateCheck;

        [SerializeField, HideInInspector]
        private string _updateCheckBaseUrl = "http://apexgametoolsproductservice.azurewebsites.net";

        [SerializeField, HideInInspector]
        private string[] _knownPendingUpdates;

        [SerializeField, HideInInspector]
        private bool _checkNews = true;

        [SerializeField, HideInInspector]
        private string _lastNewsDate;

        [SerializeField, HideInInspector]
        private string _lastNewsCheck;

        private bool _isDirty;

        public bool allowAutomaticUpdateCheck
        {
            get
            {
                return _allowAutomaticUpdateCheck;
            }

            set
            {
                if (_allowAutomaticUpdateCheck != value)
                {
                    _allowAutomaticUpdateCheck = value;
                    _isDirty = true;
                }
            }
        }

        public bool checkNews
        {
            get
            {
                return _checkNews;
            }

            set
            {
                if (_checkNews != value)
                {
                    _checkNews = value;
                    _isDirty = true;
                }
            }
        }

        public int updateCheckIntervalHours
        {
            get
            {
                return _updateCheckIntervalHours;
            }

            set
            {
                if (_updateCheckIntervalHours != value)
                {
                    _updateCheckIntervalHours = value;
                    _isDirty = true;
                }
            }
        }

        public string updateCheckBaseUrl
        {
            get { return _updateCheckBaseUrl; }
        }

        public DateTime? lastUpdateCheck
        {
            get
            {
                if (string.IsNullOrEmpty(_lastUpdateCheck))
                {
                    return null;
                }

                return DateTime.Parse(_lastUpdateCheck, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
            }
        }

        public DateTime? lastNewsDate
        {
            get
            {
                if (string.IsNullOrEmpty(_lastNewsDate))
                {
                    return null;
                }

                return DateTime.Parse(_lastNewsDate, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
            }
        }

        public bool timeToUpdate
        {
            get
            {
                if (string.IsNullOrEmpty(_lastUpdateCheck) || !_allowAutomaticUpdateCheck)
                {
                    return _allowAutomaticUpdateCheck;
                }

                return (DateTime.UtcNow - this.lastUpdateCheck.Value).TotalHours > _updateCheckIntervalHours;
            }
        }

        public bool timeToCheckNews
        {
            get
            {
                if (string.IsNullOrEmpty(_lastNewsCheck) || !_checkNews)
                {
                    return _checkNews;
                }

                var lastNewsCheckDate = DateTime.Parse(_lastNewsCheck, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
                return (DateTime.UtcNow - lastNewsCheckDate).TotalHours > 12;
            }
        }

        public string rootFolder
        {
            get;
            private set;
        }

        public string dataFolder
        {
            get;
            private set;
        }

        public string relativeDataFolder
        {
            get;
            private set;
        }

        public bool isDirty
        {
            get { return _isDirty; }
        }

        public static bool TryGetSettings(out ApexSettings settings)
        {
            if (_instance != null)
            {
                settings = _instance;
                return true;
            }

            var apexRoot = AssetPath.GetApexRoot(false);
            var dataFolder = AssetPath.GetApexDataFolder(false);
            var relativeDataFolder = AssetPath.GetApexDataFolder(true);
            string settingsPath = AssetPath.Combine(relativeDataFolder, "ApexSettings.asset");

            AssetPath.EnsurePath(relativeDataFolder);

            _instance = AssetDatabase.LoadAssetAtPath(settingsPath, typeof(ApexSettings)) as ApexSettings;

            bool settingsFound = (_instance != null);
            if (!settingsFound)
            {
                _instance = ScriptableObject.CreateInstance<ApexSettings>();

                AssetDatabase.CreateAsset(_instance, settingsPath);
                AssetDatabase.SaveAssets();
            }

            _instance.rootFolder = apexRoot;
            _instance.dataFolder = dataFolder;
            _instance.relativeDataFolder = relativeDataFolder;
            settings = _instance;
            return settingsFound;
        }

        public HashSet<string> GetKnownPendingUpdates()
        {
            if (_knownPendingUpdates != null)
            {
                return new HashSet<string>(_knownPendingUpdates);
            }

            return new HashSet<string>();
        }

        public void UpdateKnowPendingUpdates(string[] pendingUpdates)
        {
            _knownPendingUpdates = pendingUpdates;
            _isDirty = true;
        }

        public void UpdateCompleted(string updatedCheckUrl)
        {
            _lastUpdateCheck = DateTime.UtcNow.ToString(DateTimeFormatInfo.InvariantInfo);

            if (!string.IsNullOrEmpty(updatedCheckUrl))
            {
                _updateCheckBaseUrl = updatedCheckUrl;
            }

            _isDirty = true;
        }

        public void LatestNewsRetrieved(DateTime latestNewsDate)
        {
            if (latestNewsDate.Kind != DateTimeKind.Utc)
            {
                latestNewsDate = latestNewsDate.ToUniversalTime();
            }

            _lastNewsDate = latestNewsDate.ToString(DateTimeFormatInfo.InvariantInfo);
            _isDirty = true;
            SaveChanges();
        }

        public void NewsChecked()
        {
            _lastNewsCheck = DateTime.UtcNow.ToString(DateTimeFormatInfo.InvariantInfo);
            _isDirty = true;
            SaveChanges();
        }

        public void SaveChanges()
        {
            if (_isDirty)
            {
                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssets();
                _isDirty = false;
            }
        }
    }
}
