using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.StateMachines
{
    class BossState : IState
    {
        private int progression = 0;
        private float timer = 7;

        private GameObject boss;

        private GameObject birdSpawner;
        private GameObject birdTightSpawner;
        private GameObject birdBomberSpawner;
        private GameObject hpSpawner;
        private Vector3 spawnerOffset;
        private ShipDriver sd;

        public BossState()
        {
            boss = (GameObject)Resources.Load("Boss");
            //birdSpawner = (GameObject)Resources.Load("BirdSpawner");
            //birdTightSpawner = (GameObject)Resources.Load("BirdTightSpawner");
            //birdBomberSpawner = (GameObject)Resources.Load("BirdBomberSpawner");
            //hpSpawner = (GameObject)Resources.Load("HpSpawner");
            spawnerOffset = new Vector3(57, 5, 0);
            sd = ShipDriver.Get();
        }
        public void OnEnter()
        {
            DialogueQueue.AddMessage("Oh! There's a minor detail that I forgot to mention in your training...");
            DialogueQueue.AddMessage("There's a party that's very interested in taking that package you're carrying.");
            DataDump.SetInt("BossHP", 750);
            MusicBox.ChangeMusic(MusicBox.Song.Boss);
            //birdSpawner = GameObject.Instantiate(birdSpawner, sd.transform.position + spawnerOffset + Vector3.up * -5, Quaternion.FromToRotation(Vector3.forward, Vector3.left));
            //birdSpawner.GetComponent<SmoothTrack>().target = sd.transform;
            //birdSpawner.SetActive(false);
            //birdTightSpawner = GameObject.Instantiate(birdTightSpawner, sd.transform.position + spawnerOffset + Vector3.up * 5, Quaternion.FromToRotation(Vector3.forward, Vector3.left));
            //birdTightSpawner.GetComponent<SmoothTrack>().target = sd.transform;
            //birdTightSpawner.SetActive(true);
            //birdBomberSpawner = GameObject.Instantiate(birdBomberSpawner, sd.transform.position + spawnerOffset, Quaternion.FromToRotation(Vector3.forward, Vector3.left));
            //birdBomberSpawner.GetComponent<SmoothTrack>().target = sd.transform;
            //birdBomberSpawner.SetActive(false);
            //hpSpawner = GameObject.Instantiate(hpSpawner, sd.transform.position + spawnerOffset + Vector3.up * 5, Quaternion.FromToRotation(Vector3.forward, Vector3.left));
            //hpSpawner.GetComponent<SmoothTrack>().target = sd.transform;
            //hpSpawner.SetActive(true);
        }

        public void OnExit()
        {
            //GameObject.Destroy(birdSpawner);
            //GameObject.Destroy(birdTightSpawner);
            //GameObject.Destroy(birdBomberSpawner);
        }

        public IState Update()
        {
            if (progression == 0 || progression == 4)
            {
                timer -= Time.deltaTime;
            }
            if (progression == 0 && timer < 0)
            {
                DialogueQueue.AddMessage("Here they come!");
                boss = GameObject.Instantiate(boss, sd.transform.position + spawnerOffset + Vector3.up * -5, Quaternion.FromToRotation(Vector3.forward, Vector3.left));
                boss.GetComponent<SmoothTrack>().target = sd.transform;
                progression++;
            }
            if (progression == 1 && DataDump.GetInt("BossHP") < 500)
            {
                DialogueQueue.AddMessage("Keep it up!");
                progression++;

            }
            if (progression == 2 && DataDump.GetInt("BossHP") < 250)
            {
                DialogueQueue.AddMessage("You have them on the ropes!");
                progression++;
            }
            if (progression == 3 && DataDump.GetInt("BossHP") <= 0)
            {
                DialogueQueue.AddMessage("Yeah! Great work comrade! The drop point is just ahead!");
                timer = 5;
                progression++;
            }
            if (progression == 4 && timer < 0) {
                progression++;
                return new DropoffState();
            }
            return this;
        }
    }
}
