using System;
using UnityEngine;

namespace BattleCity.Player
{
	public class ShieldPlayer : MonoBehaviour
	{
		public event EventHandler DestroyEvent;
		public float TimeLife = 3.5f;

		private void Start()
		{
			Destroy(gameObject, TimeLife);
		}

		private void OnDestroy()
		{
			if (DestroyEvent != null)
				DestroyEvent(this, EventArgs.Empty);
		}

		private void OnApplicationQuit()
		{
			DestroyEvent = null;
		}
	}
}