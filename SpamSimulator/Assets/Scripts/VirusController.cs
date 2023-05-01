using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using SpamSim;

public class VirusController : MonoBehaviour
{
    public UIDocument uIDocument;
    public GameController gameController;
    public EmailController emailController;
    public SoundController soundController;
    public ModalWindowController windowController;
    public StoryController storyController;

    private VisualElement root;
    private List<Virus> viruses = new List<Virus>();

    void Start() {
        root = uIDocument.rootVisualElement;
    }

    public void CreateAndTriggerVirus(VirusType virusType) {
        switch(virusType){
            case VirusType.SEND_EXTRA_SPAM:
                Virus sendExtraSpamVirus = new ExtraSpamVirus(
                    VirusType.SEND_EXTRA_SPAM, 
                    root, 
                    gameController, 
                    emailController, 
                    soundController, 
                    windowController,
                    storyController
                );
                sendExtraSpamVirus.TriggerVirus();
                viruses.Add(sendExtraSpamVirus);
            break;
        }
    }

    void Update () {

        //Update each virus
        foreach (Virus virus in viruses) {
            virus.Update();
        }
    }
}
