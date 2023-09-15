using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MyFrameworkCore
{
    public class FsmLoadHotDll : IStateNode
    {
        private StateMachine _machine;
        public void OnCreate(StateMachine machine) 
        {
            this._machine = machine;
        }
        public void OnEnter()
        {
            PatchManager.Instance.LoadingText.text = "开始加载热更代码";
            Debug.Log("开始加载热更代码");
            // Editor环境下，HotUpdate.dll.bytes已经被自动加载，不需要加载，重复加载反而会出问题。
#if !UNITY_EDITOR
        Assembly hotUpdateAss = Assembly.Load(File.ReadAllBytes($"{Application.streamingAssetsPath}/HotUpdate.dll.bytes"));
#else
            // Editor下无需加载，直接查找获得HotUpdate程序集
            Assembly hotUpdateAss = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "HotUpdate");
#endif
            GameObject uiLoading = PatchManager.Instance.UILoading;
            PatchManager.Instance.Destroy();
            Type type = hotUpdateAss.GetType("MyFrameworkCore.InitGame");
            type.GetMethod("Init").Invoke(InitGame.Instance, new object[] { uiLoading });
        }
    }
}
