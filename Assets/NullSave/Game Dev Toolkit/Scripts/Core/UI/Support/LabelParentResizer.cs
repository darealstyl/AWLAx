//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;

namespace NullSave.GDTK
{
    [ExecuteInEditMode]
    [AutoDoc("Resizes a GameObject based on child Label's resize event")]
    [AutoDocLocation("ui")]
    public class LabelParentResizer : MonoBehaviour
    {

        #region Fields

        [Tooltip("Label to resize to")] public Label label;
        [Tooltip("Padding to apply to resize")] public Vector2 padding;

        private RectTransform rt, rtLabel;

        #endregion

        #region Unity Methods

        private void OnDisable()
        {
            if(label != null)
            {
                label.onResized.RemoveListener(Resize);
            }
        }

        private void OnEnable()
        {
            label = GetComponentInChildren<Label>();
            rt = GetComponent<RectTransform>();
            if (label == null)
            {
                enabled = false;
                return;
            }
            rtLabel = label.GetComponent<RectTransform>();
            label.onResized.AddListener(Resize);
        }

        #endregion

        #region Private Methods

        private void Resize()
        {
            float width = Mathf.Max(rtLabel.rect.width, rtLabel.sizeDelta.x);
            float height = Mathf.Max(rtLabel.rect.height, rtLabel.sizeDelta.y);
            Vector2 sizeTo = new Vector2(width + padding.x, height + padding.y);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sizeTo.y);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sizeTo.x);
        }

        #endregion

    }
}
