using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.StateMachines
{
    class BirdsState : IState
    {
        private int progression = 0;
        private float timer = 10;
        private int loops = 1;

        private GameObject birdSpawner;
        private GameObject birdTightSpawner;
        private GameObject birdBomberSpawner;
        private GameObject hpSpawner;
        private Vector3 spawnerOffset;
        private ShipDriver sd;

        public BirdsState()
        {
            birdSpawner = (GameObject)Resources.Load("BirdSpawner");
            birdTightSpawner = (GameObject)Resources.Load("BirdTightSpawner");
            birdBomberSpawner = (GameObject)Resources.Load("BirdBomberSpawner");
            hpSpawner = (GameObject)Resources.Load("HpSpawner");
            spawnerOffset = new Vector3(57, 5, 0);
            sd = ShipDriver.Get();
        }
        public void OnEnter()
        {
            birdSpawner = GameObject.Instantiate(birdSpawner, sd.transform.position + spawnerOffset + Vector3.up * -5, Quaternion.FromToRotation(Vector3.forward, Vector3.left));
            birdSpawner.GetComponent<SmoothTrack>().target = sd.transform;
            birdSpawner.SetActive(false);
            birdTightSpawner = GameObject.Instantiate(birdTightSpawner, sd.transform.position + spawnerOffset + Vector3.up * 5, Quaternion.FromToRotation(Vector3.forward, Vector3.left));
            birdTightSpawner.GetComponent<SmoothTrack>().target = sd.transform;
            birdTightSpawner.SetActive(true);
            birdBomberSpawner = GameObject.Instantiate(birdBomberSpawner, sd.transform.position + spawnerOffset, Quaternion.FromToRotation(Vector3.forward, Vector3.left));
            birdBomberSpawner.GetComponent<SmoothTrack>().target = sd.transform;
            birdBomberSpawner.SetActive(false);
            hpSpawner = GameObject.Instantiate(hpSpawner, sd.transform.position + spawnerOffset + Vector3.up * 5, Quaternion.FromToRotation(Vector3.forward, Vector3.left));
            hpSpawner.GetComponent<SmoothTrack>().target = sd.transform;
            hpSpawner.SetActive(true);
            MusicBox.ChangeMusic(MusicBox.Song.Fly);
        }

        public void OnExit()
        {
            GameObject.Destroy(birdSpawner);
            GameObject.Destroy(birdTightSpawner);
            GameObject.Destroy(birdBomberSpawner);
        }

        public IState Update()
        {
            if (progression == 0 && DataDump.GetInt("BirdsKilled") >= 20)
            {
                DataDump.SetInt("BirdsKilled", 0);
                progression++;

                birdSpawner.SetActive(true);

            }
            if (progression == 1 && DataDump.GetInt("BirdsKilled") >= 20)
            {
                DataDump.SetInt("BirdsKilled", 0);
                progression++;

                birdBomberSpawner.SetActive(true);

            }
            if (progression == 2 && DataDump.GetInt("BirdsKilled") >= 20)
            {
                DataDump.SetInt("BirdsKilled", 0);
                progression++;

                birdSpawner.SetActive(false);
                birdTightSpawner.SetActive(false);
                birdBomberSpawner.SetActive(false);
            }
            if (progression == 3)
            {
                timer -= Time.deltaTime;
                if (timer < 0)
                {
                    loops--;
                    progression = 0;
                    birdTightSpawner.SetActive(true);
                    timer = 10;
                    if (loops <= 0) return new BossState();
                    return this;
                }
            }
            return this;
        }
    }
}
