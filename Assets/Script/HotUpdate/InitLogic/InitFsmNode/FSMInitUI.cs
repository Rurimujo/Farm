using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MyFrameworkCore
{
	public class FSMInitUI : IStateNode
	{
		private StateMachine _machine;

		public void OnCreate(StateMachine machine)
		{
			this._machine = machine;
		}
		public void OnEnter()
		{
            ConfigUIPanel.UIPlayerBagPanel.ShwoUIPanel<UIPlayerBagPanel>();
            ConfigUIPanel.UIActionBarPanel.ShwoUIPanel<UIActionBarPanel>();
			ConfigUIPanel.UIFadePanel.ShwoUIPanel<UIFadePanel>();

			ConfigEvent.UIFade.EventTriggerUniTask(0f).Forget();
			ConfigUIPanel.UIPlayerBagPanel.CloseUIPanel();

			InitGame.Instance.UILoading.SetActive(false);
			RDebug.Log("游戏开始");
			Text().Forget();


		}
		public async UniTask Text()
        {
			await UniTask.DelayFrame(40);
			RDebug.Log("创建物体中");
			GameObject gameObject = await ResourceExtension.LoadAsyncUniTask<GameObject>(ConfigPrefab.WorldItemPrefab);

			GameObject go1 = GameObject.Instantiate(gameObject);
			Item item = go1.GetComponent<Item>();
			item.LoadItem(1007, 3).Forget();
		}
	}
}