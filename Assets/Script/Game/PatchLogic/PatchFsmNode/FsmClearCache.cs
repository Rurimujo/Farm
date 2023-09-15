using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 清理未使用的缓存文件
/// </summary>
namespace MyFrameworkCore
{
	internal class FsmClearCache : IStateNode
	{
		private StateMachine _machine;

		void IStateNode.OnCreate(StateMachine machine)
		{
			_machine = machine;
		}
		void IStateNode.OnEnter()
		{
            PatchManager.Instance.LoadingText.text = "清理未使用的缓存文件！";
			var package = YooAsset.YooAssets.GetPackage("PC");
			var operation = package.ClearUnusedCacheFilesAsync();
			operation.Completed += Operation_Completed;
		}
		private void Operation_Completed(YooAsset.AsyncOperationBase obj)
		{
			Debug.Log("清理缓存完毕");
			_machine.ChangeState<FsmPatchDone>();
		}
	}
}