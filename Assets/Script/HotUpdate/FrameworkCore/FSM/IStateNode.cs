using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
/*--------脚本描述-----------
电子邮箱：
    2605218730@qq.com
作者:
    琉璃无常
描述:
    状态节点
-----------------------*/
namespace MyFrameworkCore
{
    public interface IStateNode
    {
        void OnCreate(StateMachine machine) { }
        void OnCreate(StateMachine machine,InputActions inputActions) { }
        void OnCreate(StateMachine machine,InputActions inputActions,Rigidbody2D rb) { }
        void OnEnter();
        void OnUpdate() { }
        void OnExit() { }
    }
}
