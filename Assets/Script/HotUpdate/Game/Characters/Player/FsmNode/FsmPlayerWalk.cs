using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MyFrameworkCore
{
    public class FsmPlayerWalk : IStateNode
    {
        private StateMachine machine;
        private InputActions inputActions;
        private Rigidbody2D rb;
        public void OnCreate(StateMachine machine, InputActions inputActions, Rigidbody2D rb)
        {
            this.machine = machine;
            this.inputActions = inputActions;
            this.rb = rb;
        }

        public void OnEnter()
        {
            RDebug.Log("进入Walk");
        }
        public void OnUpdate()
        {
            if (rb.velocity == Vector2.zero)
                machine.ChangeState<FsmPlayerIdle>();
        }
        public void OnExit()
        {
            RDebug.Log("退出Walk");
        }
    }
}
