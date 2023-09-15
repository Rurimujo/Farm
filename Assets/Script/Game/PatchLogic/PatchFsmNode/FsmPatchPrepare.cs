using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

/// <summary>
/// 流程准备工作
/// </summary>
namespace MyFrameworkCore
{
	internal class FsmPatchPrepare : IStateNode
	{
		private StateMachine _machine;

		void IStateNode.OnCreate(StateMachine machine)
		{
			_machine = machine;
		}
		 void IStateNode.OnEnter()
		{
			Debug.Log("流程准备工作");
			// 初始化资源系统
			YooAssets.Initialize();
			YooAssets.SetOperationSystemMaxTimeSlice(30);//设置异步系统参数，每帧执行消耗的最大时间切片
			GameObject UILoadingPrefab = Resources.Load<GameObject>("UILoading");
			PatchManager.Instance.UILoading = GameObject.Instantiate(UILoadingPrefab);
			var UIComponent = PatchManager.Instance.UILoading.GetComponent<UIComponent>();
			PatchManager.Instance.LoadingText = UIComponent.Get<GameObject>("T_Text").GetComponent<Text>();
			PatchManager.Instance.LoadingText.text = "流程准备工作";

			_machine.ChangeState<FsmInitialize>();
		}

	}
}