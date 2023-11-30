//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NullSave.GDTK
{
    [AutoDocLocation("ui")]
    [AutoDoc("Changes the scale of an item based on whether or not the pointer is over it")]
    public class FocusScaleUIElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {

        #region Members

        [Tooltip("Element to size")] public Transform target;
        [Tooltip("Scale to use when not hovering")] public Vector3 defaultScale;
        [Tooltip("Scale to use when hovering")] public Vector3 hoverScale;
        [Tooltip("Time in seconds to transition between scales")] public float transitionTime;

        private float elapsedTime;
        private bool working;
        private Vector3 destScale;

        #endregion

        #region Unity Methods

        private void OnDisable()
        {
            target.localScale = defaultScale;
        }

        private void OnEnable()
        {
            target.localScale = defaultScale;
        }

        [AutoDocSuppress]
        public void OnPointerEnter(PointerEventData eventData)
        {
            destScale = hoverScale;
            if (!working) StartCoroutine(ScaleElement());
        }

        [AutoDocSuppress]
        public void OnPointerExit(PointerEventData eventData)
        {
            destScale = defaultScale;
            if (!working) StartCoroutine(ScaleElement());
        }

        private void Reset()
        {
            target = transform;
            defaultScale = new Vector3(1, 1, 1);
            hoverScale = new Vector3(1.5f, 1.5f, 1.5f);
            transitionTime = 0.25f;
        }

        #endregion

        #region Private Methods

        private IEnumerator ScaleElement()
        {
            working = true;
            elapsedTime = 0;

            while (elapsedTime < transitionTime)
            {
                yield return new WaitForEndOfFrame();
                elapsedTime += Time.unscaledDeltaTime;
                target.localScale = Vector3.Lerp(target.localScale, destScale, elapsedTime / transitionTime);
            }

            target.localScale = destScale;

            working = false;
        }

        #endregion

    }
}