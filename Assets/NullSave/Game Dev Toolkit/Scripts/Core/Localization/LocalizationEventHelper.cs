﻿//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace NullSave.GDTK
{
    [AutoDocLocation("localization/classes")]
    [AutoDoc("This component allows you to easily manage Localization from Unity events and references")]
    public class LocalizationEventHelper : MonoBehaviour
    {

        #region Fields

        [Tooltip("GameObject to activate while downloading")] public GameObject downloadWindow;
        [Tooltip("Slider used to display download progress")] public Slider downloadProgress;

        [Tooltip("Event raised when source is local")] public UnityEvent onSourceIsLocal;
        [Tooltip("Event raised when source is DLC")] public UnityEvent onSourceIsDLC;
        [Tooltip("Event raised when source is loaded into memory")] public UnityEvent onLoadIsMemory;
        [Tooltip("Event raised when source is read in real-time each request")] public UnityEvent onLoadIsRealTime;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            Localize.onSettingsChanged.AddListener(SettingsChanged);
            if (Localize.Initialized)
            {
                SettingsChanged();
            }
        }

        #endregion

        #region Public Methods

        [AutoDoc("Download an localization asset bundle.")]
        public void DownloadAssetBundle()
        {
            StartCoroutine(BundleDownload());
        }

        [AutoDoc("Open a URL")]
        [AutoDocParameter("URL to open")]
        public void OpenLink(string url)
        {
            Application.OpenURL(url);
        }

        [AutoDoc("Set the current language")]
        [AutoDocParameter("Language to use")]
        public void SetLanguage(string language)
        {
            Localize.CurrentLanguage = language;
        }

        [AutoDoc("Set localization to load from memory")]
        public void SetLoadFromMemory()
        {
            if (Localize.LookupMode != DictionaryLookupMode.StoreInMemory)
            {
                Localize.LookupMode = DictionaryLookupMode.StoreInMemory;
                Localize.Initialize();
            }
        }

        [AutoDoc("Set localization to load in real-time")]
        public void SetLoadRealtime()
        {
            if (Localize.LookupMode != DictionaryLookupMode.ReadOnRequest)
            {
                Localize.LookupMode = DictionaryLookupMode.ReadOnRequest;
                Localize.Initialize();
            }
        }

        [AutoDoc("Set source to asset bundle")]
        public void SetSourceToAssetBundle()
        {
            if (Localize.SourceType != DictionarySource.AssetBundle)
            {
                Localize.SourceType = DictionarySource.AssetBundle;
                Localize.Initialize();
            }
        }

        [AutoDoc("Set source to resource file")]
        public void SetSourceToResourceFile()
        {
            if (Localize.SourceType != DictionarySource.ResourceFile)
            {
                Localize.SourceType = DictionarySource.ResourceFile;
                Localize.Initialize();
            }
        }

        #endregion

        #region Private Methods

        private IEnumerator BundleDownload()
        {
            if (downloadWindow != null)
            {
                downloadWindow.SetActive(true);
            }

            if (downloadProgress != null)
            {
                downloadProgress.minValue = 0;
                downloadProgress.maxValue = 1;
                downloadProgress.value = 0;
            }

            yield return new WaitForEndOfFrame();

            using (UnityWebRequest www = UnityWebRequest.Get(Localize.BundleURL))
            {
                www.SendWebRequest();

                while (!www.isDone)
                {
                    if (downloadProgress != null)
                    {
                        downloadProgress.value = www.downloadProgress;
                    }
                    yield return null;
                }

                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError || www.result == UnityWebRequest.Result.DataProcessingError)
                {
                    StringExtensions.LogError(gameObject, "LocalizationEventHelper", "Download failed: " + www.error);
                }
                else
                {
                    if (!Directory.Exists(Application.streamingAssetsPath))
                    {
                        Directory.CreateDirectory(Application.streamingAssetsPath);
                    }

                    using (FileStream fs = new FileStream(Path.Combine(Application.streamingAssetsPath, Localize.BundleName), FileMode.Create, FileAccess.Write))
                    {
                        byte[] b = www.downloadHandler.data;
                        fs.Write(b, 0, b.Length);
                    }

                    if (downloadWindow != null)
                    {
                        downloadWindow.SetActive(false);
                    }

#if UNITY_EDITOR
                    UnityEditor.AssetDatabase.Refresh();
#endif

                }
            }
        }

        private void SettingsChanged()
        {
            if (Localize.SourceType == DictionarySource.AssetBundle)
            {
                onSourceIsDLC?.Invoke();
            }
            else
            {
                onSourceIsLocal?.Invoke();
            }

            if (Localize.LookupMode == DictionaryLookupMode.ReadOnRequest)
            {
                onLoadIsRealTime?.Invoke();
            }
            else
            {
                onLoadIsMemory?.Invoke();
            }
        }

        #endregion

    }
}