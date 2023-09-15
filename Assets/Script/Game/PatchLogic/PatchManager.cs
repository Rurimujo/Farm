using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

/*--------脚本描述-----------
电子邮箱：
    2605218730@qq.com
作者:
    琉璃无常
描述:
    资源加载状态管理
-----------------------*/

namespace MyFrameworkCore
{
	public class PatchManager : SingletonInit<PatchManager>, ICore
    {
        public void ICroeInit()
        {
            _machine = new StateMachine(this);
            _machine.AddNode<FsmPatchPrepare>();
            _machine.AddNode<FsmInitialize>();
            _machine.AddNode<FsmUpdateVersion>();
            _machine.AddNode<FsmUpdateManifest>();
            _machine.AddNode<FsmCreateDownloader>();
            _machine.AddNode<FsmDownloadFiles>();
            _machine.AddNode<FsmDownloadOver>();
            _machine.AddNode<FsmClearCache>();
            _machine.AddNode<FsmPatchDone>();
            _machine.AddNode<FsmLoadHotDll>();
		}
        /// <summary>
        /// 运行模式
        /// </summary>
        public EPlayMode PlayMode { private set; get; }

		/// <summary>
		/// 包裹的版本信息
		/// </summary>
		public string PackageVersion { set; get; }

		/// <summary>
		/// 下载器
		/// </summary>
		public ResourceDownloaderOperation Downloader { set; get; }

		/// <summary>
		/// UI加载界面
		/// </summary>
		public GameObject UILoading { set; get; }
		/// <summary>
		/// UI加载文字显示
		/// </summary>
		public Text LoadingText { set; get; }


		private bool _isRun = false;
		private StateMachine _machine;

		/// <summary>
		/// 开启流程
		/// </summary>
		public void Run(EPlayMode playMode)
		{
			if (_isRun == false)
			{
				_isRun = true;
				PlayMode = playMode;

				Debug.Log("开启补丁更新流程...");
				_machine.Run<FsmPatchPrepare>();
			}
			else
			{
				Debug.LogWarning("补丁更新已经正在进行中!");
			}
		}
		#region UI加载文字回调
		/// <summary>
		/// 下载出错
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="error"></param>
		/// <exception cref="NotImplementedException"></exception>
		public void OnDownloadErrorFunction(string fileName, string error)
		{
			Debug.LogError(string.Format("下载出错：文件名：{0}, 错误信息：{1}", fileName, error));
			LoadingText.text = string.Format("下载出错：文件名：{0}, 错误信息：{1}", fileName, error);
		}

		/// <summary>
		/// 更新中
		/// </summary>
		/// <param name="totalDownloadCount"></param>
		/// <param name="currentDownloadCount"></param>
		/// <param name="totalDownloadBytes"></param>
		/// <param name="currentDownloadBytes"></param>
		/// <exception cref="NotImplementedException"></exception>
		public void OnDownloadProgressUpdateFunction(int totalDownloadCount, int currentDownloadCount, long totalDownloadBytes, long currentDownloadBytes)
		{
			//Debug.Log(string.Format("文件总数：{0}, 已下载文件数：{1}, 下载总大小：{2}, 已下载大小：{3}", totalDownloadCount, currentDownloadCount, totalDownloadBytes, currentDownloadBytes));
			//LoadingText.text = string.Format($"文件总数：{totalDownloadCount}, 已下载文件数：{currentDownloadCount}, 下载总大小：{GetMB(totalDownloadBytes)}, 已下载大小：{GetMB(currentDownloadBytes)}");

			LoadingText.text = string.Format($"已下载文件数：{currentDownloadCount}/{totalDownloadCount}," +
				$"已下载大小：{(currentDownloadBytes)}/{(totalDownloadBytes)}");
			Debug.Log("需要下载的:" + currentDownloadBytes);
		}

		/// <summary>
		/// 开始下载
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="sizeBytes"></param>
		/// <exception cref="NotImplementedException"></exception>
		public void OnStartDownloadFileFunction(string fileName, long sizeBytes)
		{
			Debug.Log(string.Format("开始下载：文件名：{0}, 文件大小：{1}", fileName, sizeBytes));
			LoadingText.text = string.Format($"开始下载：文件名：{fileName}, 文件大小：{GetMB(sizeBytes)}");
		}

		/// <summary>
		/// 下载完成
		/// </summary>
		/// <param name="isSucceed"></param>
		/// <exception cref="NotImplementedException"></exception>
		public void OnDownloadOverFunction(bool isSucceed)
		{
			//Debug.Log("下载" + (isSucceed ? "成功" : "失败"));
			LoadingText.text = "下载" + (isSucceed ? "成功" : "失败");
		}
        #endregion

        #region 辅助函数
        /// <summary>
        /// 将B转换为MB
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private string GetMB(long b)
		{
			for (int i = 0; i < 2; i++)
				b /= 1024;
			return $"{b}MB";
		}

        #endregion
    }
}