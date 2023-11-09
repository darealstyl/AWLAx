using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider;
    public Image fill;
    public Color normalColor = new(131.0f / 255.0f, 226.0f / 255.0f, 91.0f / 255.0f, 1.0f);

    public Color badlyDamaged = new(202.0f / 255.0f, 75 / 255.0f, 79 / 255.0f, 1.0f);

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = normalColor;

    }

    public void SetHealth(float health)
    {
        slider.value = health;

        if (health / slider.maxValue < 0.20f)
        {
            fill.color = badlyDamaged;
        }
        else
        {
            fill.color = normalColor;
        }
    }
}
