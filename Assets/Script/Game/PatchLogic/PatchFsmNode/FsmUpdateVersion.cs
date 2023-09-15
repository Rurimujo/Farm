using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

/// <summary>
/// 更新资源版本号
/// </summary>
namespace MyFrameworkCore
{
	internal class FsmUpdateVersion : IStateNode
	{
		private StateMachine _machine;

		void IStateNode.OnCreate(StateMachine machine)
		{
			_machine = machine;
		}
		void IStateNode.OnEnter()
		{
			PatchManager.Instance.LoadingText.text = "更新资源版本号";
            GetStaticVersion().Forget();
            //UniSingleton.StartCoroutine(GetStaticVersion());
        }
        private async UniTaskVoid GetStaticVersion()
        {
            await UniTask.Delay(300);

            var package = YooAssets.GetPackage("PC");
            var operation = package.UpdatePackageVersionAsync();
            await operation;

            if (operation.Status != EOperationStatus.Succeed)
            {
                Debug.LogWarning($"更新资源版本号失败: {operation.Error}");
                Debug.Log($"将会使用旧版版本");
                _machine.ChangeState<FsmPatchDone>();
                return;
            }
            PatchManager.Instance.PackageVersion = operation.PackageVersion;
            Debug.Log($"更新版号成功,远端最新版本为: {operation.PackageVersion}");
            _machine.ChangeState<FsmUpdateManifest>();
        }
        //旧协程
        //private IEnumerator GetStaticVersion()
        //{
        //	yield return new WaitForSecondsRealtime(0.3f);

        //	var package = YooAssets.GetPackage("PC");
        //	var operation = package.UpdatePackageVersionAsync();
        //	yield return operation;

        //	if (operation.Status != EOperationStatus.Succeed)
        //	{
        //		Debug.LogWarning($"更新资源版本号失败: {operation.Error}");
        //		Debug.Log($"将会使用旧版版本");
        //		_machine.ChangeState<FsmPatchDone>();
        //		yield break;
        //	}
        //	PatchManager.Instance.PackageVersion = operation.PackageVersion;
        //	Debug.Log($"更新版号成功,远端最新版本为: {operation.PackageVersion}");
        //	_machine.ChangeState<FsmUpdateManifest>();
        //}
    }
}