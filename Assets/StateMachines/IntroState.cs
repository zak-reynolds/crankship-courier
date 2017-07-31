using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.StateMachines
{
    class IntroState : IState
    {
        private ShipDriver sd;
        private CaptainDriver cd;
        private int progression = 0;
        private float timer = 5;

        GameObject cam;

        public IntroState()
        {
            sd = ShipDriver.Get();
            cd = CaptainDriver.Get();
        }
        public void OnEnter()
        {
            cam = GameObject.FindWithTag("MainCamera");
        }

        public void OnExit()
        {
            cam.transform.rotation = Quaternion.identity;
            cam.GetComponent<SmoothTrack>().enabled = true;
            foreach (var gob in GameObject.FindGameObjectsWithTag("IntroUI"))
            {
                gob.SetActive(false);
            }
        }

        public IState Update()
        {
            return this;
        }
    }
}
