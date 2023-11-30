//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;
using UnityEngine.UI;

namespace NullSave.GDTK
{
    [RequireComponent(typeof(Slider))]
    [AutoDoc("Allow slider to respond to input events")]
    [AutoDocLocation("ui")]
    public class SliderInput : MonoBehaviour
    {

        #region Fields

        [AutoDoc("Axis used to control input")] public string slideAxis;
        [AutoDoc("Axis sensitivity")] public float sensitivity;
        [AutoDoc("Update slider with integer values")] public bool integerValues;
        [AutoDoc("Seconds to wait before repeating input")] public float repeatDelay;

        private Slider slider;
        private bool inputStarted;
        private bool inputPos;
        private float elapsed;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            slider = GetComponent<Slider>();
            if (sensitivity < 1) sensitivity = 1;
        }

        private void OnEnable()
        {
            inputStarted = false;
            elapsed = 0;
        }

        private void Reset()
        {
            slideAxis = "Horizontal";
            sensitivity = 1;
            repeatDelay = 0.25f;
        }

        private void Update()
        {
            float input = InterfaceManager.Input.GetAxis(slideAxis) * Time.deltaTime * sensitivity;

            if (integerValues)
            {
                if(!inputStarted)
                {
                    if(input > 0)
                    {
                        inputStarted = true;
                        inputPos = true;
                        slider.value += 1;
                    }
                    else if (input < 0)
                    {
                        inputStarted = true;
                        inputPos = false;
                        slider.value -= 1;
                    }
                }
                else
                {
                    if(input > 0 && inputPos || input < 0 && !inputPos)
                    {
                        elapsed += Time.deltaTime;
                        if(elapsed >= repeatDelay)
                        {
                            elapsed -= repeatDelay;
                            slider.value += inputPos ? 1 : -1;
                        }
                    }
                    else
                    {
                        inputStarted = false;
                        elapsed = 0;
                    }
                }
            }
            else
            {
                slider.value += input;
            }
        }

        #endregion


    }
}