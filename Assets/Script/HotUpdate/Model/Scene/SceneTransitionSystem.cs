using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;


/*--------脚本描述-----------
电子邮箱：
    2605218730@qq.com
作者:
    琉璃无常
描述:
    场景过渡系统
-----------------------*/


namespace MyFrameworkCore
{
    public class SceneTransitionSystem :SingletonInit<SceneTransitionSystem>, ICore
    {
        public string currentceneName = string.Empty;//当前的场景,如果从记录中加载的场景可以在这里设置
        private bool isFade;//是否切换场景

        public void ICroeInit()
        {
            currentceneName = ConfigScenes.FieldScenes;
            ConfigEvent.SceneTransition.AddEventListenerUniTask<string, Vector3>(SceneTransition);
            //ConfigEvent.SceneTransition.AddEventListener<string, Vector3>((arg2, pos) => { SceneTransition(arg2, pos).Forget(); });
        }

        public void Init()
        {
            CreatScene().Forget();
        }

        private async UniTaskVoid CreatScene()
        {
            await ConfigScenes.PersistentSceneScenes.LoadSceneAsyncUnitask(LoadSceneMode.Single);
            await currentceneName.LoadSceneAsyncUnitask(LoadSceneMode.Additive);
            currentceneName.SetActivateScene();//设置为激活场景
            ConfigEvent.SwichConfinerShape.EventTrigger();//切换场景边界
        }

        //切换场景
        private async UniTask SceneTransition(string targetScene, Vector3 targetPosition)
        {
            if (!isFade)//如果是切换场景的情况下
            {
                isFade = true;
                ConfigEvent.SceneBeforeUnload.EventTrigger();
                await ConfigEvent.UIFade.EventTriggerUniTask((float)1);
                SceneOperationHandle sceneOperationHandle = await targetScene.LoadSceneAsyncUnitask(LoadSceneMode.Additive);//加载新的场景
                sceneOperationHandle.ActivateScene();                           //设置场景激活
                currentceneName.UnloadAsync();                                  //卸载原来的场景
                currentceneName = targetScene;                                  //变换当前场景的名称
                ConfigEvent.PlayerMoveToPosition.EventTrigger(targetPosition);  //移动人物坐标
                ConfigEvent.SwichConfinerShape.EventTrigger();                  //切换场景边界
                await UniTask.DelayFrame(40);
                ConfigEvent.SceneAfterLoaded.EventTrigger();                    //加载场景之后需要做的事情
                await ConfigEvent.UIFade.EventTriggerUniTask((float)0);
                isFade = false;
            }
        }
    }
}
