using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpamSim {
    public enum VirusType {
        SEND_EXTRA_SPAM
    }

    public abstract class Virus {
        protected int timesTriggered;
        protected VirusType virusType;

        protected VisualElement root;
        protected GameController gameController;
        protected EmailController emailController;
        protected SoundController soundController;
        protected ModalWindowController windowController;
        protected StoryController storyController;

        public Virus(VirusType virusType, 
                     VisualElement root, 
                     GameController gameController, 
                     EmailController emailController, 
                     SoundController soundController, 
                     ModalWindowController windowController,
                     StoryController storyController)
        {   
            this.virusType = virusType;
            this.root = root;
            this.gameController = gameController;
            this.emailController = emailController;
            this.soundController = soundController;
            this.windowController = windowController;
            this.storyController = storyController;
        }

        public abstract void Update ();
        public abstract void TriggerVirus();
    }
    
}
