//
// Game Developers Toolkit � 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NullSave.GDTK
{
    [AutoDocLocation("ui")]
    [AutoDoc("Display component for BasicInfo")]
    public class BasicInfoUI : MonoBehaviour, IPointerClickHandler
    {

        #region Fields

        [Tooltip("Label used to display Id")] public Label id;
        [Tooltip("Label used to display Title")] public Label title;
        [Tooltip("Label used to display Abbreviation")] public Label abbreviation;
        [Tooltip("Label used to display Description")] public Label description;
        [Tooltip("Label used to display Group Name")] public Label groupName;
        [Tooltip("Image used to display Sprite")] public Image image;

        [Tooltip("Event raised when object is clicked")] public UnityEvent onClick;

        #endregion

        #region Properties

        [AutoDoc("Gets the BasicInfo being displayed")]
        public BasicInfo basicInfo { get; private set; }

        #endregion

        #region Unity Methods

        [AutoDocSuppress]
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            onClick?.Invoke();
        }

        #endregion

        #region Public Methods

        [AutoDoc("Load Basic Info for UI")]
        [AutoDocParameter("Basic Info to load")]
        public void Load(BasicInfo info)
        {
            basicInfo = info;
            if(info == null)
            {
                if (id) id.text = string.Empty;
                if (title) title.text = string.Empty;
                if (abbreviation) abbreviation.text = string.Empty;
                if (description) description.text = string.Empty;
                if (groupName) groupName.text = string.Empty;
                if (image)
                {
                    image.sprite = null;
                    image.enabled = false;
                }

                return;
            }

            if (id) id.text = info.id;
            if (title) title.text = info.title;
            if (abbreviation) abbreviation.text = info.abbr;
            if (description) description.text = info.description;
            if (groupName) groupName.text = info.groupName;
            if (image)
            {
                image.sprite = info.image.GetImage();
                image.enabled = image.sprite != null;
            }
        }

        #endregion

    }
}