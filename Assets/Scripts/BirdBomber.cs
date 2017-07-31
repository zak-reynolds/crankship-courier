using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    class BirdBomber : BirdDriver
    {

        public override void Start()
        {
            base.Start();
        }

        protected override void Move()
        {
            effectiveVelocity += Vector3.down * Time.deltaTime * speed;
        }

        protected override void Doomed()
        {
            base.Doomed();
        }
    }
}
