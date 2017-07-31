using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.StateMachines
{
    public interface IState
    {
        void OnEnter();
        void OnExit();
        IState Update();
    }
}
