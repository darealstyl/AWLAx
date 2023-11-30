//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

#if ENABLE_INPUT_SYSTEM
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace NullSave.GDTK
{
    [AutoDocLocation("action-icons-system")]
    [AutoDoc("Binds an action to a Unity Image")]
    [RequireComponent(typeof(Image))]
    public class ImageInputIcon : MonoBehaviour
    {

        #region Fields

        [Tooltip("Actions asset used for mapping actions")] public InputActionAsset inputActions;
        [Tooltip("Name of action to map")] public string actionName;

        private Image image;

        #endregion

        #region Unity Events

        private void Awake()
        {
            image = GetComponent<Image>();
            ActionIcons.instance.onControllerChanged.AddListener(UpdateIcons);
            UpdateIcons();
        }

        #endregion

        #region Private Methods

        private void UpdateIcons()
        {
            InputMap map = ActionIcons.GetActionIcon(inputActions, actionName);
            image.sprite = map == null ? null : map.TMPSpriteAsset.spriteGlyphTable[map.tmpSpriteIndex].sprite;
        }

        #endregion

    }
}
#endif