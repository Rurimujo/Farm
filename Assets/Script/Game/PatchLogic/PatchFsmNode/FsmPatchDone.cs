using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using YooAsset;

/// <summary>
/// 流程更新完毕
/// </summary>
namespace MyFrameworkCore
{
	public class FsmPatchDone : IStateNode
	{
		private StateMachine _machine;

        public void OnCreate(StateMachine machine)
		{
			this._machine = machine;
		}
		public void OnEnter()
		{
			PatchManager.Instance.LoadingText.text = "补丁加载完成";
			_machine.ChangeState<FsmLoadHotDll>();
		}
    }
}