using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.StateMachines
{
    class DropoffState : IState
    {
        private int progression = 0;
        private float timer = 7;

        private GameObject dockingShip;

        private GameObject birdSpawner;
        private GameObject birdTightSpawner;
        private GameObject birdBomberSpawner;
        private GameObject hpSpawner;
        private Vector3 spawnerOffset;
        private ShipDriver sd;

        public DropoffState()
        {
            dockingShip = (GameObject)Resources.Load("DockingShip");
            //birdSpawner = (GameObject)Resources.Load("BirdSpawner");
            //birdTightSpawner = (GameObject)Resources.Load("BirdTightSpawner");
            //birdBomberSpawner = (GameObject)Resources.Load("BirdBomberSpawner");
            //hpSpawner = (GameObject)Resources.Load("HpSpawner");
            spawnerOffset = new Vector3(90, 5, 0);
            sd = ShipDriver.Get();
        }
        public void OnEnter()
        {
            DialogueQueue.AddMessage("Dock with the ship to drop off your package.");
            MusicBox.ChangeMusic(MusicBox.Song.Fly);
        }

        public void OnExit()
        {
            //GameObject.Destroy(birdSpawner);
            //GameObject.Destroy(birdTightSpawner);
            //GameObject.Destroy(birdBomberSpawner);
        }

        public IState Update()
        {
            if (progression == 0 || progression == 2)
            {
                timer -= Time.deltaTime;
            }
            if (progression == 0 && timer < 0)
            {
                progression++;
                dockingShip = GameObject.Instantiate(dockingShip, sd.transform.position + spawnerOffset + Vector3.up * -5, Quaternion.identity);
            }
            if (progression == 1 && DataDump.GetInt("PlayerDocked") > 0)
            {
                DialogueQueue.AddMessage("Well done!");
                DialogueQueue.AddMessage("This was an entry to Ludum Dare 39: Running out of Power, created by Zak Reynolds in 48 hours.");
                DialogueQueue.AddMessage("Thanks for playing!");
                progression++;
                timer = 15;
                var st = GameObject.FindWithTag("MainCamera").GetComponent<SmoothTrack>();
                st.target = dockingShip.transform;
                st.smoothTime = 0.5f;
                st.positionOffset = new Vector3(13, 5, -45);
            }
            if (progression == 2)
            {
                if (timer < 0)
                {
                    // TODO: End
                    progression++;
                    DialogueQueue.AddMessage("END GAME\nESC to quit\nBACKSPACE to restart", 20);
                    return this;
                }
            }
            return this;
        }
    }
}
