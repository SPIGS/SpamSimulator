using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class BootController : MonoBehaviour
{
    public float startLabelUpdateSecs = 3.0f;
    public float testLabelUpdateSecs = 3.0f;
    public float doneLabelUpdateSecs = 1.0f;

    public SoundController soundController;
    public UIDocument uIDocument;
    private float timeElapsed = 0.0f;
    private BootStage stage = BootStage.START;
    private VisualElement root;
    private Label startLabel;
    private int startCount = 0;
    private Label memoryTestLabel;
    private int memoryCount = 0;
    private Label doneLabel;

    private VisualElement bootScreen;
    private VisualElement splashScreen;
    private VisualElement spinner;
    private int splashCount = 0;
    private int maxSplashCount;

    private enum BootStage {
        START,
        MEMCHECK,
        BOOT,
        SPLASH
    }

    // Start is called before the first frame update
    void Start()
    {
        root = uIDocument.rootVisualElement;
        startLabel = root.Q<Label>("StartLabel");
        memoryTestLabel= root.Q<Label>("TestingLabel");
        doneLabel= root.Q<Label>("DoneLabel");
        bootScreen = root.Q<VisualElement>("BootScreen");
        splashScreen = root.Q<VisualElement>("SplashScreen");
        spinner = root.Q<VisualElement>("Spinner");
        bootScreen.visible = true;
        splashScreen.visible = false;
        maxSplashCount = (int)(Screen.width / 40);
        // Play boot beep   
        soundController.PlaySoundEffect("Boot Beep");
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        switch(stage){
            case BootStage.START:
            if (timeElapsed >= startLabelUpdateSecs) {
                startLabel.text = startLabel.text + " .";
                timeElapsed = 0.0f;
                startCount++;
                if (startCount >= 3) {
                    stage = BootStage.MEMCHECK;
                }
            }
            break;
            case BootStage.MEMCHECK:
                memoryTestLabel.visible = true;
                if (timeElapsed >= testLabelUpdateSecs)
                {
                    if (memoryCount < 4) {
                        memoryTestLabel.text = memoryTestLabel.text + " .";
                        timeElapsed = 0.0f;
                        memoryCount++;
                    } else if (memoryCount == 4) {
                        memoryTestLabel.text = memoryTestLabel.text + " Done.";
                        timeElapsed = 0.0f;
                        memoryCount++;
                    } else {
                        stage = BootStage.BOOT;
                        doneLabel.visible = true;
                    }
                }
            break;
            case BootStage.BOOT:
                if (timeElapsed >= doneLabelUpdateSecs) {
                    timeElapsed = 0.0f;
                    doneLabel.visible = true;
                    stage = BootStage.SPLASH;
                    bootScreen.visible = false;
                    splashScreen.visible = true;
                    spinner.visible = true;
                }
            break;
            case BootStage.SPLASH:
                if (timeElapsed >= 0.1f) {
                    float offset = spinner.style.left.value.value + 450 * timeElapsed;
                    if (offset > Screen.width) {
                        offset = -spinner.style.width.value.value;
                    } 
                    spinner.style.left = offset;
                    timeElapsed = 0.0f;
                    splashCount++;
                    if (splashCount >= maxSplashCount) {
                        SceneManager.LoadScene("Game");
                    }
                }
            break;
        }
    }
}
