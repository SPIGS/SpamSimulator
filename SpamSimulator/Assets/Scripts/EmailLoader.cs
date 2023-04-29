using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EmailLoader : MonoBehaviour
{
    private VisualElement emailRoot;

    // Start is called before the first frame update
    void Start()
    {
        emailRoot = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>(name: "EmailUI");
        emailRoot.style.backgroundColor = new StyleColor(Color.red);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
