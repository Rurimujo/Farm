using Cysharp.Threading.Tasks;

namespace MyFrameworkCore
{
	public class FSMInitBaseCore : IStateNode
	{
		private StateMachine _machine;

		public void OnCreate(StateMachine machine)
		{
			this._machine = machine;
		}
		public void OnEnter()
		{
			DebugManager.Instance.Init();
			MonoManager.Instance.Init();

			UIManager.Instance.Init();
			DataManager.Instance.Init();
			PoolManager.Instance.Init();
			AduioManager.Instance.Init();

			_machine.ChangeState<FSMInitModel>();
		}
	}
}