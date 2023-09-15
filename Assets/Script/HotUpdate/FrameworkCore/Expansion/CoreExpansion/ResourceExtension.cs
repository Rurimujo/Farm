

using Cysharp.Threading.Tasks;

/*--------脚本描述-----------
电子邮箱：
    2605218730@qq.com
作者:
    琉璃无常
描述:
    加载拓展类
-----------------------*/

namespace MyFrameworkCore
{
    public static class ResourceExtension
    {
        //同步加载
        public static T Load<T>(this string assetName) where T : UnityEngine.Object
        {
            return ResourceManager.Instance.Load<T>(assetName);
        }

        //异步加载
        public static UniTask<T> LoadAsyncUniTask<T>(this string assetName) where T : UnityEngine.Object
        {
            return ResourceManager.Instance.LoadAsyncUniTask<T>(assetName);
        }

        //资源释放
        public static void UnloadAssets()
        {
            ResourceManager.Instance.UnloadAssets();
        }
    }
}
