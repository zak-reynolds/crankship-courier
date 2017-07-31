using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.StateMachines
{
    class LearnEngineState : IState
    {
        private ShipDriver sd;
        private CaptainDriver cd;
        private int progression = 0;
        private float timer = 5;

        private GameObject engineTrainingSpawner;
        private GameObject shootTrainingSpawner;
        private Vector3 spawnerOffset;

        private int ramCount;
        private int shootCount;

        public LearnEngineState(int ram, int shoot)
        {
            ramCount = ram;
            shootCount = shoot;
            engineTrainingSpawner = (GameObject)Resources.Load("EngineTrainingSpawner");
            shootTrainingSpawner = (GameObject)Resources.Load("ShootTrainingSpawner");
            spawnerOffset = new Vector3(57, 5, 0);
            sd = ShipDriver.Get();
            cd = CaptainDriver.Get();
        }

        public LearnEngineState() : this(5, 5) {}

        public void OnEnter()
        {
            DialogueQueue.AddMessage("Ram the targets as they come up. You'll get the hang of flying this thing soon enough.");
            engineTrainingSpawner = GameObject.Instantiate(engineTrainingSpawner, sd.transform.position + spawnerOffset, Quaternion.FromToRotation(Vector3.up, Vector3.left));
            engineTrainingSpawner.GetComponent<SmoothTrack>().target = sd.transform;
        }

        public void OnExit()
        {
            GameObject.Destroy(engineTrainingSpawner);
            GameObject.Destroy(shootTrainingSpawner);
            //GobPool.ClearPool("TrainingTarget");
            //GobPool.ClearPool("TrainingTargetShoot");
        }

        public IState Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // TODO: skip
            }
            if (progression == 0 && DataDump.GetInt("TrainingTargetsHit") >= ramCount)
            {
                DialogueQueue.AddMessage("Well done! Now try shooting them. Remember to move between the gun and engine using 'A' and 'D'.");
                progression++;
                var targets = GameObject.FindGameObjectsWithTag("TrainingTarget");
                foreach (var t in targets)
                {
                    t.GetComponent<TrainingTarget>().Doomed();
                }

                engineTrainingSpawner.SetActive(false);
                shootTrainingSpawner = GameObject.Instantiate(shootTrainingSpawner, sd.transform.position + spawnerOffset, Quaternion.FromToRotation(Vector3.up, Vector3.left));
                shootTrainingSpawner.GetComponent<SmoothTrack>().target = sd.transform;

            }
            if (progression == 1 && DataDump.GetInt("TrainingTargetsShot") >= shootCount)
            {
                var targets = GameObject.FindGameObjectsWithTag("TrainingTargetShoot");
                foreach (var t in targets)
                {
                    t.GetComponent<TrainingTargetShoot>().Doomed();
                }
                shootTrainingSpawner.SetActive(false);
                DialogueQueue.AddMessage("Nice shooting! You're pretty much a professional at this point.");
                DialogueQueue.AddMessage("Deliver the package safely. There might be a few birds on the way to watch out for, but nothing to worry about.");
                DialogueQueue.AddMessage("...");
                progression++;
            }
            if (progression == 2)
            {
                timer -= Time.deltaTime;
                if (timer < 0)
                {
                    progression++;
                    return new BirdsState();
                }
            }
            return this;
        }
    }
}
