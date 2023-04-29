using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ModalWindowCreator : MonoBehaviour
{
    private VisualElement root;
    private int modalWindowCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
    }
    
    public void CreateModal(VisualTreeAsset windowTemplate, int x, int y, int width, int height) {
        modalWindowCounter++;
        VisualElement modalWindow = windowTemplate.Instantiate();
        modalWindow.style.width = width;
        modalWindow.style.height = height;
        modalWindow.style.position = Position.Absolute;
        modalWindow.style.left = x;
        modalWindow.style.top = y;
        
        int windowId = modalWindowCounter;
        modalWindow.name = "Window" + windowId;
        
        root.Add(modalWindow);
    }
}
