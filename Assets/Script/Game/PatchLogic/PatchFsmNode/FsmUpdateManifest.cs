using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

/// <summary>
/// 更新资源清单
/// </summary>
namespace MyFrameworkCore
{
	public class FsmUpdateManifest : IStateNode
	{
		private StateMachine _machine;

		void IStateNode.OnCreate(StateMachine machine)
		{
			_machine = machine;
		}
		void IStateNode.OnEnter()
		{
			PatchManager.Instance.LoadingText.text = "更新资源清单";
            //UniSingleton.StartCoroutine(UpdateManifest());
            UpdateManifest().Forget();

        }
        private async UniTaskVoid UpdateManifest()
        {
            await UniTask.Delay(300);

            bool savePackageVersion = true;
            var package = YooAssets.GetPackage("PC");
            Debug.Log("当前的版本号是:" + PatchManager.Instance.PackageVersion);
            var operation = package.UpdatePackageManifestAsync(PatchManager.Instance.PackageVersion, savePackageVersion);
            await operation;

            if (operation.Status != EOperationStatus.Succeed)
            {
                Debug.LogWarning($"更新资源清单失败: {operation.Error}");
                return;
            }
            Debug.Log($"更新资源清单成功: {operation.Status}!");
            _machine.ChangeState<FsmCreateDownloader>();

        }
        //旧协程
        //private IEnumerator UpdateManifest()
        //{
        //	yield return new WaitForSecondsRealtime(0.5f);

        //	bool savePackageVersion = true;
        //	var package = YooAssets.GetPackage("PC");
        //	Debug.Log("当前的版本号是:" + PatchManager.Instance.PackageVersion);
        //	var operation = package.UpdatePackageManifestAsync(PatchManager.Instance.PackageVersion, savePackageVersion);
        //	yield return operation;

        //	if (operation.Status != EOperationStatus.Succeed)
        //	{
        //		Debug.LogWarning($"更新资源清单失败: {operation.Error}");
        //		yield break;
        //	}
        //	Debug.Log($"更新资源清单成功: {operation.Status}!");
        //	_machine.ChangeState<FsmCreateDownloader>();

        //}
    }
}