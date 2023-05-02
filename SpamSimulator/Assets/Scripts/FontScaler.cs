using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FontScaler : MonoBehaviour
{
    public UIDocument uI;
    public float fontScale;
    public float fontSizeLarge;
    public float fontSizeNormal;
    public float fontSizeSmall;

    void Awake()
    {
        if (fontScale == 1)
        {
            float playableHeight = Screen.height;
            fontScale = playableHeight / 1080;
        }

        scaleFont(uI.rootVisualElement);
        //scaleImage(uI.rootVisualElement);
    }

    public void scaleFont(VisualElement root) 
    {
        List<VisualElement> elements = root.Query<VisualElement>(className: "font-scale-large").ToList();

        foreach (var element in elements)
        {
            element.style.fontSize = fontSizeLarge * fontScale;
        }

        elements = root.Query<VisualElement>(className: "font-scale").ToList();

        foreach (var element in elements)
        {
            element.style.fontSize = fontSizeNormal * fontScale;
        }

        elements = root.Query<VisualElement>(className: "font-scale-small").ToList();

        foreach (var element in elements)
        {
            element.style.fontSize = fontSizeSmall * fontScale;
        }
    }

    // public void scaleImage(VisualElement root) 
    // {
    //     List<VisualElement> elements = root.Query<VisualElement>("Image").ToList();

    //     foreach (var element in elements)
    //     {
    //         // Doesn't work - element.style.width.value.value is always 0
    //         Debug.Log(element.style.width);
    //         Debug.Log(element.style.height);
    //         int imageWidth = element.style.width.value;
    //         int imageHeight = element.style.height.value;

    //         element.style.width = imageWidth * fontScale;
    //         element.style.height = imageHeight * fontScale;
    //     }
    // }
}
