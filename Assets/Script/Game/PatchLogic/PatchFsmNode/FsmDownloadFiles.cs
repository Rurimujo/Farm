using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using YooAsset;

/// <summary>
/// 下载更新文件
/// </summary>
namespace MyFrameworkCore
{
	public class FsmDownloadFiles : IStateNode
	{
		private StateMachine _machine;

        public void OnCreate(StateMachine machine)
		{
			_machine = machine;
		}
        public void OnEnter()
		{
            PatchManager.Instance.LoadingText.text = "开始下载补丁文件！";
            //UniSingleton.StartCoroutine(BeginDownload());
            BeginDownload().Forget();

        }
        private async UniTaskVoid BeginDownload()
        {
            var downloader = PatchManager.Instance.Downloader;

            //注册回调方法
            downloader.OnStartDownloadFileCallback = PatchManager.Instance.OnStartDownloadFileFunction;
            downloader.OnDownloadErrorCallback = PatchManager.Instance.OnDownloadErrorFunction;
            downloader.OnDownloadProgressCallback = PatchManager.Instance.OnDownloadProgressUpdateFunction;
            downloader.OnDownloadOverCallback = PatchManager.Instance.OnDownloadOverFunction;
            downloader.BeginDownload();
            await downloader;

            // 检测下载结果
            if (downloader.Status != EOperationStatus.Succeed)
            {
                Debug.LogError($"更新失败！{downloader.Error}");
                _machine.ChangeState<FsmCreateDownloader>();
                return;
            }
            Debug.Log("更新完成!");
            _machine.ChangeState<FsmPatchDone>();

        }
        //旧协程
        //private IEnumerator BeginDownload()
        //{
        //	var downloader = PatchManager.Instance.Downloader;

        //	//注册回调方法
        //	downloader.OnStartDownloadFileCallback = PatchManager.Instance.OnStartDownloadFileFunction;
        //	downloader.OnDownloadErrorCallback = PatchManager.Instance.OnDownloadErrorFunction;
        //	downloader.OnDownloadProgressCallback = PatchManager.Instance.OnDownloadProgressUpdateFunction;
        //	downloader.OnDownloadOverCallback = PatchManager.Instance.OnDownloadOverFunction;
        //	downloader.BeginDownload();
        //	yield return downloader;

        //	// 检测下载结果
        //	if (downloader.Status != EOperationStatus.Succeed)
        //          {
        //		Debug.LogError($"更新失败！{downloader.Error}");
        //		_machine.ChangeState<FsmCreateDownloader>();
        //		yield break;
        //	}
        //	Debug.Log("更新完成!");
        //	_machine.ChangeState<FsmPatchDone>();

        //}

    }
}