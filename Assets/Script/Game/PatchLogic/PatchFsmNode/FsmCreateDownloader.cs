using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

/// <summary>
/// 创建文件下载器
/// </summary>
namespace MyFrameworkCore
{
	public class FsmCreateDownloader : IStateNode
	{
		private StateMachine _machine;

		void IStateNode.OnCreate(StateMachine machine)
		{
			_machine = machine;
		}
		void IStateNode.OnEnter()
		{
			PatchManager.Instance.LoadingText.text = "创建补丁下载器！";
            CreateDownloader().Forget();

        }
        private async UniTaskVoid CreateDownloader()
        {
            await UniTask.Delay(300);

            int downloadingMaxNum = 10;
            int failedTryAgain = 3;
            var downloader = YooAssets.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);
            PatchManager.Instance.Downloader = downloader;

            if (downloader.TotalDownloadCount == 0)
            {
                Debug.Log("没有找到需要下载的文件!");
                _machine.ChangeState<FsmDownloadOver>();
                return;
            }
            Debug.Log($"找到需要下载{downloader.TotalDownloadCount}的文件!");

            // 发现新更新文件后，挂起流程系统
            // 注意：需要在下载前检测磁盘空间不足
            int totalDownloadCount = downloader.TotalDownloadCount;
            long totalDownloadBytes = downloader.TotalDownloadBytes;

            float sizeMB = totalDownloadBytes / 1048576f;
            sizeMB = Mathf.Clamp(sizeMB, 0.1f, float.MaxValue);
            string totalSizeMB = sizeMB.ToString("f1");
            Debug.Log($"找到下载文件路径, 数量{totalDownloadCount} 大小{totalSizeMB}MB");

            _machine.ChangeState<FsmDownloadFiles>();

        }
        //旧协程
        //IEnumerator CreateDownloader()
        //{
        //	yield return new WaitForSecondsRealtime(0.5f);

        //	int downloadingMaxNum = 10;
        //	int failedTryAgain = 3;
        //	var downloader = YooAssets.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);
        //	PatchManager.Instance.Downloader = downloader;

        //	if (downloader.TotalDownloadCount == 0)
        //	{
        //		Debug.Log("没有找到需要下载的文件!");
        //		_machine.ChangeState<FsmDownloadOver>();
        //		yield break;
        //	}
        //	//A total of 10 files were found that need to be downloaded
        //	Debug.Log($"找到需要下载{downloader.TotalDownloadCount}的文件!");

        //	// 发现新更新文件后，挂起流程系统
        //	// 注意：开发者需要在下载前检测磁盘空间不足
        //	int totalDownloadCount = downloader.TotalDownloadCount;
        //	long totalDownloadBytes = downloader.TotalDownloadBytes;

        //	float sizeMB = totalDownloadBytes / 1048576f;
        //	sizeMB = Mathf.Clamp(sizeMB, 0.1f, float.MaxValue);
        //	string totalSizeMB = sizeMB.ToString("f1");
        //	Debug.Log($"找到下载文件路径, 数量{totalDownloadCount} 大小{totalSizeMB}MB");

        //	_machine.ChangeState<FsmDownloadFiles>();

        //}
    }
}