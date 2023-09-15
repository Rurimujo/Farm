using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*--------�ű�����-----------
�������䣺
    2605218730@qq.com
����:
    �����޳�
����:
    ������Ϸ
-----------------------*/

namespace MyFrameworkCore
{
   
    public class InitGame : Singleton<InitGame>
    {
        public StateMachine initMachine;
        public GameObject UILoading;
        public void Init(GameObject uiLoading)
        {
            Debug.Log("�����ȸ�����ɹ�");
            UILoading = uiLoading;

            initMachine = new StateMachine(this);
            initMachine.AddNode<FSMInitBaseCore>();
            initMachine.AddNode<FSMInitModel>();
            initMachine.AddNode<FSMInitUI>();

            initMachine.Run<FSMInitBaseCore>();
        }
    }
}
