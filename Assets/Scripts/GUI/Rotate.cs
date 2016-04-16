using UnityEngine;

namespace BattleCity.GUI.Main
{
	public class Rotate : MonoBehaviour
	{
		[SerializeField]
		private float Speed = 5f;

		private float _tick;

		private void Update()
		{
			if (++_tick < Speed)
				return;
			transform.Rotate(Vector3.back, 30);
			_tick = 0;
		}
	}
}