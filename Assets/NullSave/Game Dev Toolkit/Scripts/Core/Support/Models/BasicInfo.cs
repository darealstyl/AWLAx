//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDoc("Provides information about an object")]
    [AutoDocLocation("miscellaneous/classes")]
    [Serializable]
    public class BasicInfo
    {

        #region Fields

        [Tooltip("Id used to reference this object")] public string id;
        [Tooltip("Automatically generate Id from Title")] public bool autoGenId;
        [Tooltip("Title to display in UI")] public string title;
        [Tooltip("Abberviation to display in UI")] public string abbr;
        [Tooltip("Description to display in UI")] [TextArea(2, 5)] public string description;
        [Tooltip("Optional name used to group similar items")] public string groupName;
        [Tooltip("Image to display in UI")] public ImageInfo image;
        [Tooltip("UI Color associated with object")] public Color color;
        [Tooltip("Hide object from UI")] public bool hidden;
        [Tooltip("List of string tags")] public List<string> tags;

        [Tooltip("Owner or parent of the object")] public object source;

#if UNITY_EDITOR
        [SerializeField] private bool z_expanded;
#endif

        #endregion

        #region Constructor

        public BasicInfo()
        {
            image = new ImageInfo();
            color = Color.white;
            autoGenId = true;
#if UNITY_EDITOR
            z_expanded = true;
            bool x = z_expanded; // Remove stupid unity warning
#endif
            tags = new List<string>();
        }

        #endregion

        #region Public Methods

        [AutoDoc("Create a clone of this object")]
        public BasicInfo Clone()
        {
            BasicInfo result = new BasicInfo();

            result.id = id;
            result.title = title;
            result.abbr = abbr;
            result.description = description;
            result.image = image.Clone();
            result.color = color;
            result.hidden = hidden;
            result.autoGenId = autoGenId;
            result.groupName = groupName;
            result.tags = tags.ToList();

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

            id = stream.ReadStringPacket();
            autoGenId = stream.ReadBool();
            title = stream.ReadStringPacket();
            abbr = stream.ReadStringPacket();
            description = stream.ReadStringPacket();
            groupName = stream.ReadStringPacket();
            image.DataLoad(stream, version);
            color = stream.ReadColor();
            hidden = stream.ReadBool();
            tags.Clear();
            int count = stream.ReadInt();
            for (int i = 0; i < count; i++)
            {
                tags.Add(stream.ReadStringPacket());
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

            stream.WriteStringPacket(id);
            stream.WriteBool(autoGenId);
            stream.WriteStringPacket(title);
            stream.WriteStringPacket(abbr);
            stream.WriteStringPacket(description);
            stream.WriteStringPacket(groupName);
            image.DataSave(stream, version);
            stream.WriteColor(color);
            stream.WriteBool(hidden);
            stream.WriteInt(tags.Count);
            foreach (string tag in tags)
            {
                stream.WriteStringPacket(tag);
            }
        }

        [AutoDoc("Check if data matches")]
        [AutoDocParameter("Object to compair to")]
        public bool Matches(BasicInfo source)
        {
            if (source.id != id) return false;
            if (source.autoGenId != autoGenId) return false;
            if (source.title != title) return false;
            if (source.abbr != abbr) return false;
            if (source.description != description) return false;
            if (source.groupName != groupName) return false;
            if (!source.image.Matches(image)) return false;
            if (source.color != color) return false;
            if (source.hidden != hidden) return false;

            if (tags == null && source.tags != null) return false;
            if (tags != null && source.tags == null) return false;
            if (tags.Count != source.tags.Count) return false;
            foreach (string tag in tags)
            {
                if (!source.tags.Contains(tag)) return false;
            }

            return true;
        }

        [AutoDoc("Validate object data")]
        [AutoDocParameter("Validation errors")]
        public bool Validate(out string errors)
        {
            errors = string.Empty;
            if (string.IsNullOrEmpty(id))
            {
                errors = "Id cannot be empty";
                return false;
            }
            if (!id.IsAllowedId())
            {
                errors = "Id can only contain letters, numbers, underscores, and dashes";
                return false;
            }

            return true;
        }

        #endregion

    }
}