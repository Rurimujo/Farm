using Cysharp.Threading.Tasks;
using System;
using UnityEngine.SceneManagement;
using YooAsset;

/*--------脚本描述-----------
电子邮箱：
    2605218730@qq.com
作者:
    琉璃无常
描述:
    场景加载拓展类
-----------------------*/

namespace MyFrameworkCore
{
    public static class SceneExpansion
    {
        public static void LoadSceneAsync(this string SceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single,
            Action<SceneOperationHandle> action = null, bool suspendLoad = false, int priority = 100)
        {
            ManagerScene.Instance.LoadSceneAsync(SceneName, loadSceneMode, action, suspendLoad, priority);
        }

        public static async UniTask<SceneOperationHandle> LoadSceneAsyncUnitask(this string SceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single, bool suspendLoad = false, int priority = 100)
        {
            return await ManagerScene.Instance.LoadSceneAsync(SceneName, loadSceneMode, suspendLoad, priority);
        }
        public static async UniTask<SceneOperationHandle> ChangeScene(this string oldScene, string newScene, LoadSceneMode loadSceneMode)
        {
            return await ManagerScene.Instance.ChangeScene(oldScene, newScene, loadSceneMode);
        }
        public static  void UnloadAsync(this string scnenName)
        {
            ManagerScene.Instance.UnloadAsync(scnenName);
        }
        public static void SetActivateScene(this string scnenName)
        {
            ManagerScene.Instance.SetActivateScene(scnenName);
        }
    }
}
