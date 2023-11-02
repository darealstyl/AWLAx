using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroller : MonoBehaviour
{
    [SerializeField] private RawImage image;
    [SerializeField] private float speedX = 0.1f;
    [SerializeField] private float speedY = 0f;
       // Update is called once per frame
    void Update()
    {
        image.uvRect = new Rect(image.uvRect.position + new Vector2(speedX, speedY) * Time.deltaTime, image.uvRect.size);
    }
}
