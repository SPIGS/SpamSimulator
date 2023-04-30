using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ModalWindowController : MonoBehaviour
{
    public UIDocument uIDocument;
    public FontScaler fontScaler;
    private VisualElement root;
    private int modalWindowCounter = 0;
    private float mouseX = 0.0f;
    private float mouseY = 0.0f;
    private VisualElement currentDrag;
    private float mouseXOnDrag = 0.0f;
    private float windowXOnDrag = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        root = uIDocument.rootVisualElement;
    }

    void Update () {
        mouseX = Input.mousePosition.x;
        mouseY = Input.mousePosition.y;
        if (currentDrag != null) {
            currentDrag.style.left = (StyleLength)(mouseX - mouseXOnDrag + windowXOnDrag);
            currentDrag.style.top = (StyleLength)(Screen.height - mouseY);
        }
    }

    public void CloseWindow(int windowId) {
        root.Q<VisualElement>("Window" + windowId).RemoveFromHierarchy();
    }

    public void DragWindow(MouseDownEvent evt, int windowId){
        // Set the windows position
        if (evt.button == 0) {
            VisualElement window = root.Query<VisualElement>("Window" + windowId);
            mouseXOnDrag = Input.mousePosition.x;
            windowXOnDrag = (float)window.style.left.value.value;
            this.currentDrag = window;
        }
    }

    public void LetGoWindow (MouseUpEvent evt) {
        if (evt.button == 0) {
            this.currentDrag = null;
        }
    }
    
    public void CreateModal(VisualTreeAsset windowTemplate, float x, float y, float width, float height, string title) {
        modalWindowCounter++;
        //styling
        VisualElement modalWindow = windowTemplate.Instantiate();
        modalWindow.style.width = width;
        modalWindow.style.height = height;
        modalWindow.style.position = Position.Absolute;
        modalWindow.style.left = x;
        modalWindow.style.top = y;
        fontScaler.scaleFont(modalWindow);
        
        //set the windows id
        int windowId = modalWindowCounter;
        modalWindow.name = "Window" + windowId;

        //set window title
        modalWindow.Q<Label>("Title").text = title;
        
        //Close window callback
        Button closeButton = modalWindow.Q<Button>("CloseWindow");
        closeButton.clickable.clicked += () => {
            CloseWindow(windowId);
        };

        //Drag window callback 
        modalWindow.Q<VisualElement>("WindowTitlebar").RegisterCallback<MouseDownEvent>((evt) => {
            DragWindow(evt, windowId);
        });

        //Let go windwo callabck
        root.RegisterCallback<MouseUpEvent>((evt) =>
        {
            LetGoWindow(evt);
        });
        
        root.Add(modalWindow);
    }

}
