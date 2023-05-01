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
    public VisualTreeAsset popupVirusTemplate;
    public VisualTreeAsset soundVirusTemplate;
    public VisualTreeAsset cursorVirusTemplate;
    public Texture2D virusCursor;

    private VisualElement root;
    private List<Virus> viruses = new List<Virus>();

    void Start() {
        root = uIDocument.rootVisualElement;
    }

    public bool VirusListContainsVirusType (VirusType virusType) {
        foreach(Virus virus in viruses) {
            if (virus.GetVirusType() == virusType) {
                return true;
            }
        }

        return false;
    }

    public void CreateAndTriggerVirus(VirusType virusType) {
        VirusType actualVirusType = virusType;

        // check if cetain type of viruses have been acitvated already
        // set it to another virus type.
        if (virusType == VirusType.SOUND && VirusListContainsVirusType(virusType) 
            || virusType == VirusType.CURSOR && VirusListContainsVirusType(virusType)) {
                float coinFlip = Random.Range(0.0f, 1.0f);
                if (coinFlip < 0.50f) {
                    actualVirusType = VirusType.SEND_EXTRA_SPAM;
                } else {
                    actualVirusType = VirusType.POPUP;
                }      
        }

        switch(actualVirusType){
            case VirusType.SEND_EXTRA_SPAM:
                Virus sendExtraSpamVirus = new ExtraSpamVirus(
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
            case VirusType.POPUP:
                Virus popupVirus = new PopUpVirus(
                    root,
                    gameController,
                    emailController,
                    soundController,
                    windowController,
                    storyController,
                    popupVirusTemplate
                );
                popupVirus.TriggerVirus();
                viruses.Add(popupVirus);
            break;
            case VirusType.SOUND:
                Virus soundVirus = new SoundVirus(
                    root,
                    gameController,
                    emailController,
                    soundController,
                    windowController,
                    storyController,
                    soundVirusTemplate
                );
                soundVirus.TriggerVirus();
                viruses.Add(soundVirus);
            break;
            case VirusType.CURSOR:
                Virus cursorVirus = new CursorVirus(
                    root,
                    gameController,
                    emailController,
                    soundController,
                    windowController,
                    storyController,
                    cursorVirusTemplate,
                    virusCursor
                );
                cursorVirus.TriggerVirus();
                viruses.Add(cursorVirus);
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
