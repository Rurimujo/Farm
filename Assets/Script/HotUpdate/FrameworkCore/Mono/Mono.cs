using UnityEngine;
using UnityEngine.Events;

/*--------脚本描述-----------
电子邮箱：
    2605218730@qq.com
作者:
    琉璃无常
描述:
    Mono模块
-----------------------*/
namespace MyFrameworkCore
{
    public class Mono : MonoBehaviour
    {
        private event UnityAction AwakeEvent;
        private event UnityAction UpdateEvent;
        private event UnityAction FixedUpdateEvent;

        private void Awake()
        {
            AwakeEvent?.Invoke();
        }
        private void Update()
        {
            UpdateEvent?.Invoke();
        }
        private void FixedUpdate()
        {
            FixedUpdateEvent?.Invoke();
        }

        public void OnAddAwakeEvent(UnityAction  unityAction)
        {
            AwakeEvent += unityAction;
        }
        public void OnRemoveAwakeEvent(UnityAction unityAction)
        {
            AwakeEvent -= unityAction;
        }

        public void OnAddUpdateEvent(UnityAction unityAction)
        {
            UpdateEvent += unityAction;
        }
        public void OnRemoveUpdateEvent(UnityAction unityAction)
        {
            UpdateEvent -= unityAction;
        }

        public void OnAddFixedUpdateEvent(UnityAction unityAction)
        {
            FixedUpdateEvent += unityAction;
        }
        public void OnRemoveFixedUpdateEvent(UnityAction unityAction)
        {
            FixedUpdateEvent -= unityAction;
        }
    }
}
