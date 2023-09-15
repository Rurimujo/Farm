using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 下载完毕
/// </summary>
namespace MyFrameworkCore
{
	public class FsmDownloadOver : IStateNode
	{
		private StateMachine _machine;

		public void OnCreate(StateMachine machine)
		{
			_machine = machine;
		}
		public void OnEnter()
		{
			Debug.Log("下载完毕");
			_machine.ChangeState<FsmClearCache>();
		}
	}
}