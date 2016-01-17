using System;

namespace BattleCity
{
	public interface IDestroy
	{
		event EventHandler DestroyEvent;

		void ClearEvent();
	}
}