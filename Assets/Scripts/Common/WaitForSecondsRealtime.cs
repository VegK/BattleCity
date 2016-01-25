using UnityEngine;

namespace BattleCity
{
	public class WaitForSecondsRealtime : CustomYieldInstruction
	{
		private float _waitTime;

		public override bool keepWaiting
		{
			get
			{
				return (Time.realtimeSinceStartup < _waitTime);
			}
		}

		public WaitForSecondsRealtime(float time)
		{
			_waitTime = Time.realtimeSinceStartup + time;
		}
	}
}