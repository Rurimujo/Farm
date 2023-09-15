using System.Collections.Generic;
using UnityEngine;
using YooAsset;


/*--------脚本描述-----------
电子邮箱：
    2605218730@qq.com
作者:
    琉璃无常
描述:
    UI管理类
-----------------------*/


namespace MyFrameworkCore
{
    public class UIManager :SingletonInit<UIManager>, ICore
    {
        public void ICroeInit()
        {
            _DicALLUIForms = new Dictionary<string, UIBase>();
            _DicCurrentShowUIForms = new Dictionary<string, UIBase>();
            YooAssetHdnleDic = new Dictionary<string, AssetOperationHandle>();
        }

        public Transform CanvasTransfrom = null;                //UI根节点    
        public Camera UICamera = null;                                  //UI摄像机
        public Camera MainCamera = null;                               //主摄像机

        private Dictionary<string, UIBase> _DicALLUIForms;          //缓存所有UI窗体
        private Dictionary<string, UIBase> _DicCurrentShowUIForms;  //当前显示的UI窗体
        private Dictionary<string, AssetOperationHandle> YooAssetHdnleDic;//资源加载句柄
        private Transform Normal = null;                        //普通的节点
        private Transform Mobile = null;                         //独立的窗口可移动的
        private Transform Fade = null;                         //渐变过度窗体

        //初始化
        public void Init()
        {
            AssetOperationHandle handle = YooAssetLoadExpsion.YooaddetLoadAsync<GameObject>(ConfigUIPanel.GlobalPanel);
            GameObject gameObject = handle.InstantiateSync();
            //实例化
            CanvasTransfrom = gameObject.transform;
            GameObject.DontDestroyOnLoad(CanvasTransfrom);
            //获取子节点
            Normal = GetUITypeTransform(EUIType.Normal);
            Mobile = GetUITypeTransform(EUIType.Mobile);
            Fade = GetUITypeTransform(EUIType.Fade);
            //UICamera = CanvasTransfrom.GetChildComponent<Camera>("UICamera");
            //MainCamera = CanvasTransfrom.GetChildComponent<Camera>("MainCamera");
            RDebug.Log("UI管理初始化完毕");

            
        }


        //界面增删改查方法
        public T ShwoUIPanel<T>(string uiFormName) where T : UIBase, new()
        {
            //是否存在UI类
            _DicALLUIForms.TryGetValue(uiFormName, out UIBase uIBase);
            T t = uIBase == null ? LoadUIPanel<T>(uiFormName) : uIBase as T;//UI类
            //根据不同的UI窗体的显示模式，分别作不同的加载处理
            switch (t.mode)
            {
                case EUIMode.Normal: LoadUIToCurrentCache<T>(uiFormName); break; //“普通显示”窗口模式//把当前窗体加载到“当前窗体”集合中。
                case EUIMode.HideOther: EnterUIFormsAndHideOther(uiFormName); break;//“隐藏其他”窗口模式
            }
            return t;
        }//显示界面
        public T GetUIPanl<T>(string uiFormName) where T :UIBase
        {
            _DicALLUIForms.TryGetValue(uiFormName, out UIBase baseUiForm);
            return baseUiForm as T;
        }
        public void CloseUIForms(string uiFormName)
        {
            //“所有UI窗体”集合中，如果没有记录，则直接返回
            _DicALLUIForms.TryGetValue(uiFormName, out UIBase baseUiForm);
            if (baseUiForm == null) return;

            //根据窗体不同的显示类型，分别作不同的关闭处理
            MonoManager.Instance.OnRemoveUpdateEvent(baseUiForm.UIUpdate);
            switch (baseUiForm.mode)
            {
                case EUIMode.Normal: ExitUIForms(uiFormName); break;//普通窗体的关闭
                case EUIMode.HideOther: ExitUIFormsAndDisplayOther(uiFormName); break;//隐藏其他窗体关闭
            }
        }//界面关闭
        public void RemoveUIFroms(string uiFormName)
        {
            //“所有UI窗体”集合中，如果没有记录，则直接返回
            _DicALLUIForms.TryGetValue(uiFormName, out UIBase baseUIForm);
            if (baseUIForm == null) return;
            MonoManager.Instance.OnRemoveUpdateEvent(baseUIForm.UIUpdate);
            baseUIForm.UIOnDestroy();
            //资源卸载
            YooAssetHdnleDic.TryGetValue(uiFormName, out AssetOperationHandle yooassetHandle);
            yooassetHandle?.Dispose();

        }//界面移除

        //显示界面
        private T LoadUIPanel<T>(string uiFormName) where T : UIBase, new()
        {
            T t = new T();
            //创建的UI克隆体预设
            //YooAssetLoadExpsion
            AssetOperationHandle handle = YooAssetLoadExpsion.YooaddetLoadSyncAOH(uiFormName);
            YooAssetHdnleDic.Add(uiFormName, handle);
            GameObject goCloneUIPrefabs = handle.InstantiateSync();//创建物体
            t.panelGameObject = goCloneUIPrefabs;
            t.UIName = uiFormName;
            t.UIAwake();
            MonoManager.Instance.OnAddUpdateEvent(t.UIUpdate);
            if (goCloneUIPrefabs == null)
                RDebug.Error("加载预制体失败");
            switch (t.type)
            {
                case EUIType.Normal: goCloneUIPrefabs.transform.SetParent(Normal, false); break;//普通窗体节点
                case EUIType.Mobile: goCloneUIPrefabs.transform.SetParent(Mobile, false); break;//独立的窗口可移动的
                case EUIType.Fade: goCloneUIPrefabs.transform.SetParent(Fade, false); break;//渐变过度窗体
            }
            //设置隐藏
            goCloneUIPrefabs.SetActive(false);
            //把克隆体，加入到“所有UI窗体”（缓存）集合中。
            _DicALLUIForms.Add(uiFormName, t);
            return t;
        }
        /// <summary>
        /// 把当前窗体加载到“当前窗体”集合中
        /// </summary>
        /// <param name="uiFormName">窗体预设的名称</param>
	    private void LoadUIToCurrentCache<T>(string uiFormName) where T : UIBase
        {
            UIBase baseUiForm;                          //UI窗体基类
            UIBase baseUIFormFromAllCache;              //从“所有窗体集合”中得到的窗体

            //如果“正在显示”的集合中，存在整个UI窗体，则直接返回
            _DicCurrentShowUIForms.TryGetValue(uiFormName, out baseUiForm);
            RDebug.Log("LoadUIToCurrentCache"+baseUiForm);
            if (baseUiForm != null) return;
            //把当前窗体，加载到“正在显示”集合中
            _DicALLUIForms.TryGetValue(uiFormName, out baseUIFormFromAllCache);
            if (baseUIFormFromAllCache != null)
            {
                _DicCurrentShowUIForms.Add(uiFormName, baseUIFormFromAllCache as T);
                RDebug.Log("LoadUIToCurrentCache加进字典" + baseUIFormFromAllCache);
                baseUIFormFromAllCache.UIOnEnable();           //显示当前窗体
            }
        }
        /// <summary>
        /// (“隐藏其他”属性)打开窗体，且隐藏其他窗体
        /// </summary>
        /// <param name="strUIName">打开的指定窗体名称</param>
        private void EnterUIFormsAndHideOther(string strUIName)
        {
            UIBase baseUIForm;                          //UI窗体基类
            UIBase baseUIFormFromALL;                   //从集合中得到的UI窗体基类


            //参数检查
            if (string.IsNullOrEmpty(strUIName)) return;

            _DicCurrentShowUIForms.TryGetValue(strUIName, out baseUIForm);
            if (baseUIForm != null) return;

            //把“正在显示集合”中所有窗体都隐藏。
            foreach (UIBase baseUI in _DicCurrentShowUIForms.Values)
                baseUI.UIOnDisable();

            //把当前窗体加入到“正在显示窗体”集合中，且做显示处理。
            _DicALLUIForms.TryGetValue(strUIName, out baseUIFormFromALL);
            if (baseUIFormFromALL != null)
            {
                _DicCurrentShowUIForms.Add(strUIName, baseUIFormFromALL);
                //窗体显示
                baseUIFormFromALL.UIOnEnable();
            }
        }

        //界面关闭
        /// <summary>
        /// 退出指定UI窗体
        /// </summary>
        /// <param name="strUIFormName"></param>
        private void ExitUIForms(string strUIFormName)
        {
            //"正在显示集合"中如果没有记录，则直接返回。
            _DicCurrentShowUIForms.TryGetValue(strUIFormName, out UIBase baseUIForm);
            if (baseUIForm == null) return;
            //指定窗体，标记为“隐藏状态”，且从"正在显示集合"中移除。
            baseUIForm.UIOnDisable();
            _DicCurrentShowUIForms.Remove(strUIFormName);
        }
        /// <summary>
        /// (“隐藏其他”属性)关闭窗体，且显示其他窗体
        /// </summary>
        /// <param name="strUIName">打开的指定窗体名称</param>
        private void ExitUIFormsAndDisplayOther(string strUIName)
        {
            _DicCurrentShowUIForms.TryGetValue(strUIName, out UIBase baseUIForm);
            if (baseUIForm == null) return;

            //当前窗体隐藏状态，且“正在显示”集合中，移除本窗体
            baseUIForm.UIOnDisable();
            _DicCurrentShowUIForms.Remove(strUIName);

            //把“正在显示集合”中所有窗体都定义重新显示状态。
            foreach (UIBase baseUI in _DicCurrentShowUIForms.Values)
                baseUI.UIOnEnable();
        }

        //其他
        public Transform GetUITypeTransform(EUIType UIType)
        {
            return Instance.CanvasTransfrom.GetChild(UIType.ToString());
        }

        public bool IsShow(string uiFormName)
        {
            _DicCurrentShowUIForms.TryGetValue(uiFormName, out UIBase uIBase);
            RDebug.Log("IsShow" + uIBase);
            return uIBase == null ? false : true;
        }
    }
}
