//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System;
using System.IO;
using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDoc("Provides a JSON compatible reference to an Image")]
    [AutoDocLocation("miscellaneous/classes")]
    [Serializable]
    public class ImageInfo
    {

        #region Fields

        [Tooltip("Sprite to display in UI")] public Sprite sprite;
        [Tooltip("Source of sprite data")] public ImageSource source;
        [Tooltip("Path of sprite")] public string path;
        [Tooltip("Asset Bundle containing sprite")] public string bundleName;
        [Tooltip("Name of sprite in the bundle")] public string assetName;

#if UNITY_EDITOR
        [SerializeField] private bool z_imageError;
#endif

        #endregion

        #region Public Methods

        [AutoDoc("Creates a clone of this object")]
        public ImageInfo Clone()
        {
            ImageInfo result = new ImageInfo();

            result.sprite = sprite;
            result.source = source;
            result.path = path;
            result.bundleName = bundleName;

            return result;
        }

        [AutoDoc("Load data from a stream")]
        [AutoDocParameter("Stream to use")]
        [AutoDocParameter("Version")]
        public void DataLoad(Stream stream, int version)
        {
            if (version > 1)
            {
                throw new NotSupportedException("Invalid file version");
            }

            source = (ImageSource)stream.ReadInt();
            switch (source)
            {
                case ImageSource.AssetBundle:
                    path = stream.ReadStringPacket();
                    bundleName = stream.ReadStringPacket();
                    assetName = stream.ReadStringPacket();
                    break;
                case ImageSource.PersistentData:
                    path = stream.ReadStringPacket();
                    break;
                case ImageSource.Resources:
                    path = stream.ReadStringPacket();
                    break;
            }
        }

        [AutoDoc("Save data to a stream")]
        [AutoDocParameter("Stream to use")]
        [AutoDocParameter("Version")]
        public void DataSave(Stream stream, int version)
        {
            if (version > 1)
            {
                throw new NotSupportedException("Invalid file version");
            }

            stream.WriteInt((int)source);
            switch (source)
            {
                case ImageSource.AssetBundle:
                    stream.WriteStringPacket(path);
                    stream.WriteStringPacket(bundleName);
                    stream.WriteStringPacket(assetName);
                    break;
                case ImageSource.PersistentData:
                    stream.WriteStringPacket(path);
                    break;
                case ImageSource.Resources:
                    stream.WriteStringPacket(path);
                    break;
            }
        }

        [AutoDoc("Gets Image from data, loading if needed")]
        public Sprite GetImage()
        {
            if (sprite != null) return sprite;
            LoadImage();
            return sprite;
        }

        [AutoDoc("Loads Image from data")]
        public void LoadImage()
        {
            switch (source)
            {
                case ImageSource.AssetBundle:
                    sprite = GetBundle().LoadAsset<Sprite>(assetName);
                    break;
                case ImageSource.PersistentData:
                    byte[] bytes = File.ReadAllBytes(path);
                    Texture2D texture = new Texture2D(1, 1);
                    texture.LoadImage(bytes);
                    sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    break;
                case ImageSource.Resources:
                    sprite = Resources.Load<Sprite>(path);
                    break;
            }
        }

        [AutoDoc("Checks of the data matches another ImageInfo")]
        [AutoDocParameter("Object to compair to")]
        public bool Matches(ImageInfo source)
        {
            return source.source == this.source &&
                source.path == path &&
                source.bundleName == bundleName &&
                source.assetName == assetName;
        }

        #endregion

        #region Private Methods

        private AssetBundle GetBundle()
        {
            foreach (AssetBundle bundle in AssetBundle.GetAllLoadedAssetBundles())
            {
                if (bundle.name == bundleName) return bundle;
            }

            return AssetBundle.LoadFromFile(Path.Combine(Application.persistentDataPath, assetName));
        }

        #endregion

    }
}