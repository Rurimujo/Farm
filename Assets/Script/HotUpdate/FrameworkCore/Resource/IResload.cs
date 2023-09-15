
using Cysharp.Threading.Tasks;
using UnityEngine.Events;
using YooAsset;
namespace MyFrameworkCore
{
    public interface IResload
    {
        /// <summary> 同步加载资源对象 </summary>
        public T Load<T>(string AssetName) where T : UnityEngine.Object;

        /// <summary> 异步加载资源对象 </summary>
        public UniTask<T> LoadAsyncUniTask<T>(string AssetName) where T : UnityEngine.Object;


        /// <summary>
        /// 同步加载子资源对象
        /// 例如：通过TexturePacker创建的图集，如果需要访问图集的精灵对象，可以通过子对象加载接口。
        /// </summary>
        public T LoadSub<T>(string location, string ResName) where T : UnityEngine.Object;

        /// <summary>  
        ///异步加载子资源对象
        ///</summary>
        public UniTask LoadSubAsync<T>(string location, string ResName, UnityAction<T> callback = null) where T : UnityEngine.Object;

        /// <summary>  
        ///同步加载资源包内所有资源对象
        ///</summary>
        public T[] LoadAll<T>(string ResName, UnityAction<T[]> callback) where T : UnityEngine.Object;

        /// <summary>
        /// 异步加载资源包内所有资源对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        public UniTask<UnityEngine.Object[]> LoadAllAsync<T>(string location) where T : UnityEngine.Object;


        public AssetOperationHandle LoadAssetAsync<T>(string assetName) where T : UnityEngine.Object;
        public T LoadAssetAsyncAsT<T>(string assetName) where T : UnityEngine.Object;

        /// <summary>  
        ///同步获取原生文件
        ///</summary>
        public RawFileOperationHandle LoadRawFile<T>(string location) where T : class;

        /// <summary>  
        ///异步获取原生文件
        ///</summary>
        public UniTask<RawFileOperationHandle> LoadRawFileAsync<T>(string location) where T : UnityEngine.Object;


        /// <summary>  
        ///资源卸载
        ///</summary>
        public void ReleaseAsset(string ResName = null);

        /// <summary>  
        ///资源释放
        /// 可以在切换场景之后调用资源释放方法或者写定时器间隔时间去释放。
        /// 注意：只有调用资源释放方法，资源对象才会在内存里被移除。
        ///</summary>
        public void UnloadAssets();

        /// <summary>
        /// 通过资源标签来获取资源信息列表
        /// </summary>
        public AssetInfo[] GetAssetInfosByTag(string tag);
    }
}
