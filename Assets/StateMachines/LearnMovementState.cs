using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.StateMachines
{
    class LearnMovementState : IState
    {
        private ShipDriver sd;
        private CaptainDriver cd;
        private int progression = 0;
        private float timer = 5;

        public LearnMovementState()
        {
            sd = ShipDriver.Get();
            cd = CaptainDriver.Get();
        }
        public void OnEnter()
        {
            DialogueQueue.AddMessage("Let's crank up that gatling gun you have there. Press 'D' to move to the front of your ship.");
        }

        public void OnExit()
        {
            // Do nothing
        }

        public IState Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // TODO: skip
            }
            if (progression == 0 && cd.State == CaptainDriver.CrankState.Gun)
            {
                DialogueQueue.AddMessage("You found the gun! Now turn the crank and get it up to MAX POWER! Click the button, a lot.");
                progression++;
            }
            if (progression == 1 && sd.GetGunPowerNormalized() > 0.75)
            {
                DialogueQueue.AddMessage("Feel the power spewing forth from your hands! Ptht ptht ptht ptht!");
                DialogueQueue.AddMessage("Lets start up that engine now. Head back to the stern using 'A' to move.");
                progression++;
            }
            if (progression == 2 && cd.State == CaptainDriver.CrankState.Engine)
            {
                DialogueQueue.AddMessage("It needs some warming up, so give it a good few cranks.");
                progression++;
            }
            if (progression == 3 && sd.GetEnginePowerNormalized() > sd.unDockPowerThreshold)
            {
                DialogueQueue.AddMessage("BY THE POWER PROVIDED BY YOUR OWN TWO HANDS...", 1.2f);
                DialogueQueue.AddMessage("YOU ARE A SOARING EAGLE, AN ICARUS, AN ALMIGHTY BEING!", 3.5f);
                DialogueQueue.AddMessage(" FLY! FLY!");
                progression++;
            }
            if (progression == 4)
            {
                timer -= Time.deltaTime;
                if (timer < 0)
                {
                    return new LearnEngineState();
                }
            }
            return this;
        }
    }
}
