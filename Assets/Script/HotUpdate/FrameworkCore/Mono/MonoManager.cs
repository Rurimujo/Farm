using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

/*--------脚本描述-----------
电子邮箱：
    2605218730@qq.com
作者:
    琉璃无常
描述:
    Mono模块管理类
-----------------------*/
namespace MyFrameworkCore
{
    public class MonoManager :SingletonInit<MonoManager>, ICore
    {
        public void ICroeInit()
        {
            GameObject monoTemp = new GameObject("Mono");
            monoController = monoTemp.AddComponent<Mono>();
            GameObject.DontDestroyOnLoad(monoTemp);
        }
        public void Init()
        {
            RDebug.Log("初始化Mono完毕!");
        }
        private Mono monoController;

        public void OnAddAwakeEvent(UnityAction unityAction)
        {
            monoController.OnAddAwakeEvent(unityAction);
        }
        public void OnRemoveAwakeEvent(UnityAction unityAction)
        {
            monoController.OnRemoveAwakeEvent(unityAction);
        }

        public void OnAddUpdateEvent(UnityAction unityAction)
        {
            monoController.OnAddUpdateEvent(unityAction);
        }
        public void OnRemoveUpdateEvent(UnityAction unityAction)
        {
            monoController.OnRemoveUpdateEvent(unityAction);
        }

        public void OnAddFixedUpdateEvent(UnityAction unityAction)
        {
            monoController.OnAddFixedUpdateEvent(unityAction);
        }
        public void OnRemoveFixedUpdateEvent(UnityAction unityAction)
        {
            monoController.OnRemoveFixedUpdateEvent(unityAction);
        }


    }
}
