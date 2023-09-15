using Cysharp.Threading.Tasks;
using UnityEngine;


/*--------脚本描述-----------
电子邮箱：
    2605218730@qq.com
作者:
    琉璃无常
描述:
    场景跳转
-----------------------*/


namespace MyFrameworkCore
{
    public class Transition : MonoBehaviour
    {
        [SceneName] public string sceneToGo;        //去往的场景
        public Vector3 positionTpGo;                //玩家需要去往的位置

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(ConfigTag.TagPlayer))
                ConfigEvent.SceneTransition.EventTriggerUniTask(sceneToGo, positionTpGo).Forget();
        }

    }
}
