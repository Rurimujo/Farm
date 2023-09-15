using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*--------脚本描述-----------
电子邮箱：
    2605218730@qq.com
作者:
    琉璃无常
描述:
    进入游戏
-----------------------*/

namespace MyFrameworkCore
{
   
    public class InitGame : Singleton<InitGame>
    {
        public StateMachine initMachine;
        public GameObject UILoading;
        public void Init(GameObject uiLoading)
        {
            Debug.Log("加载热更代码成功");
            UILoading = uiLoading;

            initMachine = new StateMachine(this);
            initMachine.AddNode<FSMInitBaseCore>();
            initMachine.AddNode<FSMInitModel>();
            initMachine.AddNode<FSMInitUI>();

            initMachine.Run<FSMInitBaseCore>();
        }
    }
}
