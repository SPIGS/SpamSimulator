using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FontScaler : MonoBehaviour
{
    public float fontScale = 1.0f;
    private VisualElement root;

    void Start() {
        float playableHeight = Screen.height;
        fontScale = playableHeight / 1080;

        root = GetComponent<UIDocument>().rootVisualElement;
        scaleFont(root);

        // Super hardcoded stuff
        VisualElement fileDropdown = root.Query<VisualElement>("FileDropdown");
        fileDropdown.style.left = 10 * fontScale - fontScale *fontScale;
        VisualElement settingsDropdown = root.Query<VisualElement>("SettingsDropdown");
        settingsDropdown.style.left = 10 * fontScale + (55 - fontScale) * fontScale - fontScale * fontScale;
        VisualElement toolsDropdown = root.Query<VisualElement>("ToolsDropdown");
        toolsDropdown.style.left = 10 * fontScale + (160-fontScale) * fontScale - fontScale * fontScale;
    }

    public void scaleFont (VisualElement element) {
        if (element.childCount == 0) {
            element.style.fontSize = 24 * fontScale;
            
            if (element.style.borderBottomWidth.value > 0) {
                element.style.borderBottomWidth = fontScale * fontScale;
            }
            if (element.style.borderLeftWidth.value > 0)
            {
                element.style.borderLeftWidth = fontScale * fontScale;
            }
            if (element.style.borderTopWidth.value > 0)
            {
                element.style.borderTopWidth = fontScale * fontScale;
            }
            if (element.style.borderRightWidth.value > 0)
            {
                element.style.borderRightWidth = fontScale * fontScale;
            }

            // Style execptions
            if (element.ClassListContains("toolbar-button")) {
                element.style.marginRight = 10 * fontScale;
            } else if (element.ClassListContains("dropdown-button")) {
                element.style.paddingLeft = 30 * fontScale;
                element.style.paddingRight = 30 * fontScale;
            }else if(element.ClassListContains("dropdown")) {
                element.style.paddingTop = 1 * fontScale;
                element.style.paddingBottom = 1 * fontScale;
            }

            //Name execptions
            if (element.name == "ToolbarContainer") {
                element.style.paddingLeft = 10 * fontScale;
                element.style.paddingRight = 10 * fontScale;
                element.style.paddingTop = 5 * fontScale;
                element.style.paddingBottom = 5 * fontScale;
            } 

        } else if (element.childCount > 0) {
            foreach(VisualElement child in element.Children()) {
                scaleFont(child);
            }
        }
    }
}
