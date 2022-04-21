using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageChange : MonoBehaviour
{
    public Sprite changeImage;
    Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    public void ChangeImage()
    {
        image.sprite = changeImage;
    }
}
