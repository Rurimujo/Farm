using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

namespace MyFrameworkCore
{
    public class Init : MonoBehaviour
    {
        /// <summary>
        /// 资源系统运行模式
        /// </summary>
        public EPlayMode PlayMode = EPlayMode.EditorSimulateMode;
        protected void Awake()
        {
            Debug.Log($"资源系统运行模式：{PlayMode}");
            Application.targetFrameRate = 60;
            Application.runInBackground = true;
            //初始化游戏框架
            this.InitFramework();
            //检查资源更新
            this.CheckHotUpdate();
        }

        private void InitFramework()
        {

        }
        private void CheckHotUpdate()
        {
            // 开始补丁更新流程
            PatchManager.Instance.Run(PlayMode);
        }

    }
}
