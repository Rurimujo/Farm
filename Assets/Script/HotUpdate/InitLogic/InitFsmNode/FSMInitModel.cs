namespace MyFrameworkCore
{
	public class FSMInitModel : IStateNode
	{
		private StateMachine _machine;

		public void OnCreate(StateMachine machine)
		{
			this._machine = machine;
		}
		public void OnEnter()
		{
			InventorySystem.Instance.Init();
			SceneTransitionSystem.Instance.Init();

			_machine.ChangeState<FSMInitUI>();
		}
	}
}